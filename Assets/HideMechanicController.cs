using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMechanicController : MonoBehaviour
{
    
    SplineFollower splineFollower;
    public float distanceToShift;

    void Start()
    {
        splineFollower = GetComponent<SplineFollower>();
    }
    public void TriggerPlayerHide()
    {
        splineFollower.MovePerpendicular(distanceToShift);
    }
}
