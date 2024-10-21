using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideDetectionLeft : MonoBehaviour
{
    public HideMechanicController hideMechanicController;

    
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hideMechanicController)
            {
                hideMechanicController = other.gameObject.GetComponent<HideMechanicController>();
            }

            hideMechanicController.mechanicTriggered = false;
        }
    }
}
