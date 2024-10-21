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
        if (Input.GetKeyDown(KeyCode.H) && mechanicTriggered)
        {
            Log("input received");
            TriggerPlayerHide();
        }
    }

    public void TriggerPlayerHide()
    {
        Log("Triggering player hide");
        if (mechanicTriggered)
        {
            Log("Past if statement in TriggerPlayerHide");
            splineFollower.MovePerpendicular(distanceToShift);
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
