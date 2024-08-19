using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHandlerWrapper : MonoBehaviour
{
    private SkeletonHandler skhandler;
    private Vector3 initialTargetPosition;
    private bool positionLocked = false;

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

        Debug.Log("SkeletonHandler is ready to use.");
    }

    void Update()
    {
        if (skhandler != null)
        {
            if (!positionLocked)
            {
                initialTargetPosition = skhandler.TargetBodyPositionWithHipOffset; 
                positionLocked = true;
            }

            skhandler.TargetBodyPositionWithHipOffset = initialTargetPosition;

            transform.position = initialTargetPosition;

            Debug.Log($"Locked Target Position: {initialTargetPosition}");
        }
    }
}