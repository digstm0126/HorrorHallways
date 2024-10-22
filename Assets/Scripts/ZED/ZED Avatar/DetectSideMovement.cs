using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSideMovement : MonoBehaviour
{
    private float startXPosition = 0f;
    
    public HideMechanicController hideMechanicController;
    public GetAvatarInformation getAvatarInformation;
    private bool playerHasMovedToSide;
    public float distanceThreshold = 0.5f;
    public float returnThreshold = 0.4f;

    public bool enableDebugs = false;
    void Update()
    {
        if (!hideMechanicController)
        {
            hideMechanicController = FindObjectOfType<HideMechanicController>();
        }
        
        float distanceMovedX = getAvatarInformation.GetBodyPartTransform("Character1_Hips").position.x - startXPosition;

        if (distanceMovedX >= distanceThreshold && !playerHasMovedToSide)
        {
            Log("Sidemovement detected");
            hideMechanicController.TriggerPlayerHide();
            playerHasMovedToSide = true;
        }
        else if (distanceMovedX <= -distanceThreshold && !playerHasMovedToSide)
        {
            hideMechanicController.TriggerPlayerHide();
            playerHasMovedToSide = true;
        }
        Log("The player has moved to the side boolean is :" + playerHasMovedToSide);
        if (distanceMovedX <= returnThreshold && distanceMovedX >= -returnThreshold && playerHasMovedToSide)
        {
            Log("Sidemovement return detected");
            hideMechanicController.TriggerPlayerReturn();
            playerHasMovedToSide = false;
        }
    }

    private void Log(string message)
    {
        if (enableDebugs)
        {
            Debug.Log(message);
        }
    }


}
