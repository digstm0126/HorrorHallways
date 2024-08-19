using UnityEngine;
using System.Collections.Generic;

public class ZEDBodyTrackingManagerWrapper : MonoBehaviour
{
    private ZEDBodyTrackingManager zedBodyTrackingManager;

    // Public transform to assign the desired GameObject in the Inspector
    public Transform targetTransform;

    void Start()
    {
        zedBodyTrackingManager = GetComponent<ZEDBodyTrackingManager>();

        if (zedBodyTrackingManager == null)
        {
            Debug.LogError("ZEDBodyTrackingManager component is missing.");
            return;
        }

        // Subscribe to the event to intercept the data
        zedBodyTrackingManager.zedManager.OnBodyTracking += InterceptUpdateSkeletonData;
    }

    private void InterceptUpdateSkeletonData(BodyTrackingFrame dframe)
    {
        List<int> remainingKeyList = new List<int>(zedBodyTrackingManager.AvatarControlList.Keys);
        List<DetectedBody> newBodies = dframe.GetFilteredObjectList(zedBodyTrackingManager.showON, zedBodyTrackingManager.showSEARCHING, zedBodyTrackingManager.showOFF);

        foreach (DetectedBody dbody in newBodies)
        {
            int person_id = dbody.rawBodyData.id;

            if (zedBodyTrackingManager.AvatarControlList.ContainsKey(person_id))
            {
                SkeletonHandler handler = zedBodyTrackingManager.AvatarControlList[person_id];
                
                // Intercept the UpdateAvatarControl method
                InterceptedUpdateAvatarControl(handler, dbody.rawBodyData);

                remainingKeyList.Remove(person_id);
            }
            else
            {
                if (zedBodyTrackingManager.AvatarControlList.Count < zedBodyTrackingManager.maximumNumberOfDetections)
                {
                    StartCoroutine(WrapperInstanciateAvatarWithDelay(zedBodyTrackingManager.delayBeforeSpawn, dbody));
                }
            }
        }

        foreach (int index in remainingKeyList)
        {
            SkeletonHandler handler = zedBodyTrackingManager.AvatarControlList[index];
            handler.Destroy();
            zedBodyTrackingManager.AvatarControlList.Remove(index);
        }
    }

    private void InterceptedUpdateAvatarControl(SkeletonHandler handler, sl.BodyData data)
    {
        // Intercept the worldJointsPos
        Vector3[] interceptedWorldJointsPos = InterceptWorldJointsPos(data.keypoint);

        // Call the original method with the intercepted data
        Vector3[] worldJointsPos = interceptedWorldJointsPos; // Use your intercepted data
        Quaternion[] normalizedLocalJointsRot = new Quaternion[handler.currentKeypointsCount];

        for (int i = 0; i < worldJointsPos.Length; i++)
        {
            // Adjust the position based on the targetTransform
            if (targetTransform != null)
            {
                worldJointsPos[i] = targetTransform.TransformPoint(data.keypoint[i]);
            }
            else
            {
                worldJointsPos[i] = zedBodyTrackingManager.zedManager.GetZedRootTransform().TransformPoint(data.keypoint[i]);
            }

            normalizedLocalJointsRot[i] = data.localOrientationPerJoint[i].normalized;
        }

        // Original global rotation from the ZED SDK
        Quaternion worldGlobalRotation = zedBodyTrackingManager.zedManager.GetZedRootTransform().rotation * data.globalRootOrientation;

        // Apply a 180-degree rotation around the Y-axis to flip the orientation
        Quaternion flipRotation = Quaternion.Euler(0, 180, 0);
        worldGlobalRotation *= flipRotation;

        if (data.localOrientationPerJoint.Length > 0 && data.keypoint.Length > 0 && data.keypointConfidence.Length > 0)
        {
            handler.SetConfidences(data.keypointConfidence);
            handler.SetControlWithJointPosition(
                worldJointsPos,
                normalizedLocalJointsRot, 
                worldGlobalRotation,
                zedBodyTrackingManager.useAvatar, 
                zedBodyTrackingManager.mirrorMode);

            if (zedBodyTrackingManager.EnableFootLocking)
            {
                handler.CheckFootLockAnimator();
            }

            handler.rootVelocity = data.velocity;
        }
    }

    private Vector3[] InterceptWorldJointsPos(Vector3[] originalPositions)
    {
        // Modify or log the originalPositions as needed
        Vector3[] modifiedPositions = new Vector3[originalPositions.Length];
        for (int i = 0; i < originalPositions.Length; i++)
        {
            modifiedPositions[i] = originalPositions[i]; // Or apply some transformation
        }
        return modifiedPositions;
    }

    private System.Collections.IEnumerator WrapperInstanciateAvatarWithDelay(float delay, DetectedBody dbody)
    {
        SkeletonHandler handler = ScriptableObject.CreateInstance<SkeletonHandler>();
        handler.Create(zedBodyTrackingManager.avatar, zedBodyTrackingManager.zedManager.bodyFormat, zedBodyTrackingManager);
        handler.InitSkeleton(dbody.rawBodyData.id, new Material(zedBodyTrackingManager.skeletonBaseMaterial));

        zedBodyTrackingManager.AvatarControlList.Add(dbody.rawBodyData.id, handler);
        InterceptedUpdateAvatarControl(handler, dbody.rawBodyData);
        handler.GetAnimator().gameObject.SetActive(false);
        yield return new WaitForSeconds(delay);

        if (zedBodyTrackingManager.AvatarControlList.ContainsKey(dbody.rawBodyData.id)) 
        {
            handler.zedSkeletonAnimator.canSpawn = true;
        }
    }

    void OnDestroy()
    {
        if (zedBodyTrackingManager != null && zedBodyTrackingManager.zedManager != null)
        {
            zedBodyTrackingManager.zedManager.OnBodyTracking -= InterceptUpdateSkeletonData;
        }
    }
}