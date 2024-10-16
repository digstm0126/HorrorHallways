using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMechanicEntered : MonoBehaviour
{
    
    HideMechanicController hideMechanicController;
    // Start is called before the first frame update


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            HideMechanicController hideMechanicController = other.gameObject.GetComponent<HideMechanicController>();

            hideMechanicController.mechanicTriggered = true;
        }
    }
}
