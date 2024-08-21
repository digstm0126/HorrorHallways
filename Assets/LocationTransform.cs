using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTransform : MonoBehaviour
{
    void Start()
    {
        // Find the object named "Zed" in the scene
        GameObject zedObject = GameObject.Find("Zed");

        if (zedObject != null)
        {
            // Set the "Zed" object as the parent of this model
            transform.SetParent(zedObject.transform, worldPositionStays: true);

            // Move the model to match Zed's XZ coordinates, but keep the current Y coordinate
            Vector3 newPosition = new Vector3(zedObject.transform.position.x, transform.position.y, zedObject.transform.position.z);
            transform.position = newPosition;
        }
        else
        {
            Debug.LogWarning("Zed object not found in the scene. The model will not be parented.");
        }
    }
}
