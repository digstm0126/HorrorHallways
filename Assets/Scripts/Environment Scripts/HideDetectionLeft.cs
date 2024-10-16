using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideDetectionLeft : MonoBehaviour
{
    private HideMechanicController hideMechanicController;


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hideMechanicController = other.gameObject.GetComponent<HideMechanicController>();
            hideMechanicController.mechanicTriggered = false;
        }
    }
}
