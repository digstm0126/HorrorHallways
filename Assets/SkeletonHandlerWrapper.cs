using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHandlerWrapper : MonoBehaviour
{
    private SkeletonHandler skhandler;
    private Vector3 fixedPosition;

    void Start()
    {
        
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

        
        fixedPosition = skhandler.TargetBodyPositionWithHipOffset;

        Debug.Log("SkeletonHandler is ready to use.");
    }

    void Update()
    {
        if (skhandler != null)
        {
            
            skhandler.TargetBodyPositionWithHipOffset = fixedPosition;

            
            Quaternion hipRotation = skhandler.TargetBodyOrientationSmoothed;

            Debug.Log($"Hip Position (fixed): {fixedPosition}, Hip Rotation: {hipRotation}");
        }
    }
}