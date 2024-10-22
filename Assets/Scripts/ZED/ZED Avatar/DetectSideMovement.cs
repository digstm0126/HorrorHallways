using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSideMovement : MonoBehaviour
{
    private float startXPosition = 0f;
    
    public HideMechanicController hideMechanicController;
    private bool playerHasMovedToSide;
    public float distanceThreshold = 0.5f;
    public float returnThreshold = 0.4f;
    void Update()
    {
        if (!hideMechanicController)
        {
            hideMechanicController = FindObjectOfType<HideMechanicController>();
        }
        
        float distanceMovedX = transform.position.x - startXPosition;

        if (distanceMovedX >= distanceThreshold && !playerHasMovedToSide)
        {
            hideMechanicController.TriggerPlayerHide();
            playerHasMovedToSide = true;
        }
        else if (distanceMovedX <= -distanceThreshold && !playerHasMovedToSide)
        {
            hideMechanicController.TriggerPlayerHide();
        }

        if (distanceMovedX < returnThreshold && distanceMovedX > -returnThreshold && playerHasMovedToSide)
        {
            hideMechanicController.TriggerPlayerReturn();
            playerHasMovedToSide = false;
        }
    }


}
