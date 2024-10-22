using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HideMechanicController : MonoBehaviour
{
    
    public SplineFollower splineFollower;
    public float distanceToShift;
    public bool mechanicTriggered = false;
    public bool debugLogsEnabled = false;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && mechanicTriggered && !splineFollower.hasHidden)
        {
            Log("hide input received");
            TriggerPlayerHide();
        }

        if (Input.GetKeyDown(KeyCode.R) && splineFollower.hasHidden && mechanicTriggered)
        {
            Log("return input received");
            StartCoroutine(splineFollower.ReturnToSpline());
        }
    }

    public void TriggerPlayerHide()
    {
        Log("Triggering player hide");
        if (mechanicTriggered && !splineFollower.hasHidden)
        {
            Log("Past if statement in TriggerPlayerHide");
            splineFollower.MovePerpendicular(distanceToShift);
        }
    }

    public void TriggerPlayerReturn()
    {
        if (mechanicTriggered && splineFollower.hasHidden)
        {
            splineFollower.ReturnToSpline();
        }
    }

    private void Log(string message)
    {
        if (debugLogsEnabled)
        {
            Debug.Log(message);
        }
    }
}
