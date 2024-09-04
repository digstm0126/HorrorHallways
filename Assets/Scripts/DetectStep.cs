using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectStep : MonoBehaviour
{
    public Transform leftFoot;
    public Transform rightFoot;
    public float stepThreshold = 0.4f;  
    private bool leftFootOnGround = false;
    private bool rightFootOnGround = false;

    void Update()
    {
        float leftFootDistance = GetVerticalDistanceFromRoot(leftFoot);
        float rightFootDistance = GetVerticalDistanceFromRoot(rightFoot);

        CheckStep(leftFootDistance, ref leftFootOnGround, "Left Foot");

        CheckStep(rightFootDistance, ref rightFootOnGround, "Right Foot");
    }

    float GetVerticalDistanceFromRoot(Transform foot)
    {
        Vector3 footPosition = foot.position;

        Vector3 rootPosition = transform.position;

        return rootPosition.y - footPosition.y;
    }

    void CheckStep(float footDistance, ref bool footOnGround, string footName)
    {
        if (footDistance > stepThreshold && !footOnGround)
        {
            footOnGround = true;
            Debug.Log($"{footName} has touched the ground.");
        }
        else if (footDistance <= stepThreshold && footOnGround)
        {
            footOnGround = false;
            Debug.Log($"{footName} has taken a step.");
        }
    }
    
    
    
    
}