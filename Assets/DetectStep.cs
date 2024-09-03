
using System;
using UnityEngine;

public class DetectStep : MonoBehaviour
{
    public GameObject leftFoot;
    public GameObject rightFoot;
    public GameObject baseHeight;

    
    [SerializeField] private float stepHeightThreshold = 0.1f;
    [SerializeField] private float groundCloseThreshold = 0.02f;

    private float defaultFootHeight;

    private bool leftFootInAir;
    private bool rightFootInAir;

    private void Start()
    {
        defaultFootHeight = leftFoot.transform.position.y;
        stepHeightThreshold += defaultFootHeight;
        groundCloseThreshold += defaultFootHeight;
    }

    void Update()
    {
        CheckStep(leftFoot, ref leftFootInAir, defaultFootHeight);
        CheckStep(rightFoot, ref rightFootInAir, defaultFootHeight);
        
    }

    private void CheckStep(GameObject foot, ref bool footInAir, float groundHeight)
    {
        float footHeight = foot.transform.position.y;
        float heightAboveGround = footHeight - groundHeight;
        
        Debug.Log($"{foot.name} height: {footHeight:F3}, Ground height: {groundHeight:F3}, Height above ground: {heightAboveGround:F3}");

        if (heightAboveGround > stepHeightThreshold)
        {
            footInAir = true;
        }

        if (footInAir && Mathf.Abs(heightAboveGround) <= groundCloseThreshold)
        {
            footInAir = false;
            OnStepDetected(foot);
        }
    }

    private void OnStepDetected(GameObject foot)
    {
        Debug.Log($"{foot.name} has taken a step!");
    }


}
