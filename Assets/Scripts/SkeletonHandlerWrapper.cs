using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHandlerWrapper : MonoBehaviour
{
    private SkeletonHandler skhandler;
    private Vector3 fixedPosition;

    void Start()
    {
        // Assuming ZEDSkeletonAnimator is on the same GameObject or you have a reference to it
        ZEDSkeletonAnimator zedSkeletonAnimator = GetComponent<ZEDSkeletonAnimator>();
        
        if (zedSkeletonAnimator != null)
        {
            skhandler = zedSkeletonAnimator.Skhandler;
        }

        if (skhandler == null)
        {
            Debug.LogError("SkeletonHandler not found or not initialized.");
            return;
        }

        // Store the initial position to keep the avatar running in place
        fixedPosition = skhandler.TargetBodyPositionWithHipOffset;

        Debug.Log("SkeletonHandler is ready to use.");
    }

    void Update()
    {
        if (skhandler != null)
        {
            // Override the position with the fixed position
            skhandler.TargetBodyPositionWithHipOffset = fixedPosition;

            // You can still modify or log the hip rotation as needed
            Quaternion hipRotation = skhandler.TargetBodyOrientationSmoothed;

            Debug.Log($"Hip Position (fixed): {fixedPosition}, Hip Rotation: {hipRotation}");
        }
    }
}