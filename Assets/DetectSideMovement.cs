using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSideMovement : MonoBehaviour
{
    private float startXPosition = 0f;
    
    HideMechanicController hideMechanicController;
    private bool playerHasMovedToSide;
    void Update()
    {
        if (!hideMechanicController)
        {
            hideMechanicController = FindObjectOfType<HideMechanicController>();
        }
        
        float distanceMovedX = transform.position.x - startXPosition;

        if (distanceMovedX >= 0.5f && !playerHasMovedToSide)
        {
            hideMechanicController.TriggerPlayerHide();
        }
        else if (distanceMovedX <= -0.5f && !playerHasMovedToSide)
        {
            hideMechanicController.TriggerPlayerHide();
        }
    }


}
