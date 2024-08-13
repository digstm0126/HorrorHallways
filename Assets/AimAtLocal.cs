using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtLocal : MonoBehaviour
{
    public ZEDManager zedManager = null;
    public ZEDBodyTrackingManager zedBodyTrackingManager = null;
    public Vector3 target = Vector3.zero;
    public Transform parentTransform = null; // Add this line to reference a parent transform
    private bool mirrored = false;

    private void Awake()
    {
        if (zedManager == null)
        {
            Debug.LogError("zedManager is null. Please set a ZedManager.");
            Application.Quit();
        }
        if (zedBodyTrackingManager == null)
        {
            Debug.LogError("zedBodyTrackingManager is null. Please set a zedBodyTrackingManager.");
            Application.Quit();
        }
    }

    private void Start()
    {
        zedManager.OnBodyTracking += OnBodyTrackingFrame;
        mirrored = zedBodyTrackingManager.mirrorMode;
    }

    private void OnBodyTrackingFrame(BodyTrackingFrame bodyFrame)
    {
        if(bodyFrame.bodyCount > 0)
        {
            target = bodyFrame.detectedBodies[0].Get3DWorldPosition();
            if (mirrored)
            {
                target = new Vector3(-target.x, target.y, target.z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 finalTarget = target;

        // If a parent transform is provided, calculate the target relative to it
        if (parentTransform != null)
        {
            finalTarget = parentTransform.TransformPoint(target);
        }

        transform.LookAt(finalTarget);
    }
}
