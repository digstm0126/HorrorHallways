using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMechanicController : MonoBehaviour
{
    
    SplineFollower splineFollower;
    public float distanceToShift;
    public bool mechanicTriggered = false;

    void Start()
    {
        splineFollower = GetComponent<SplineFollower>();
    }
    public void TriggerPlayerHide()
    {
        if (mechanicTriggered)
        {
            splineFollower.MovePerpendicular(distanceToShift);
        }
    }
}
