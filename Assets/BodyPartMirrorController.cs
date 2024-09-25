using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartMirrorController : MonoBehaviour
{
    private GetAvatarInformation getAvatarInformation;

    public GameObject leftHandObject;
    public GameObject rightHandObject;
    
    // Add a public variable to control logging
    public bool enableLogging;



    void Update()
    {
        if (!getAvatarInformation)
        {
            getAvatarInformation = FindObjectOfType<GetAvatarInformation>();
            if (getAvatarInformation)
            {
                SetObjectLocation(leftHandObject, "Character1_LeftHandMiddle1");
                SetObjectLocation(rightHandObject, "Character1_RightHandMiddle1");
            }
        }
        else
        {
            SetObjectLocation(leftHandObject,   "Character1_LeftHandMiddle1");
            SetObjectLocation(rightHandObject, "Character1_RightHandMiddle1");
        }
    }

    private void SetObjectLocation(GameObject bodyPartObject, String bodyPartName)
    {
        Vector3 newPosition = GetBodyPartLocation(bodyPartName);
    
        if (enableLogging)
        {
            Debug.Log("Setting position of " + bodyPartObject.name + " to " + newPosition);
        }

        bodyPartObject.transform.position = newPosition;
    }
    
    private Vector3 GetBodyPartLocation(String bodyName)
    {
        Transform bodyTransform = getAvatarInformation.GetBodyPartTransform(bodyName);

        if (bodyTransform)
        {
            if (enableLogging)
            {
                Debug.Log("Body part found: " + bodyName + " at position " + bodyTransform.position);
            }
            return bodyTransform.position;
        }
    
        // Log the warning only if logging is enabled
        if (enableLogging)
        {
            Debug.LogWarning("Body part not found: " + bodyName);
        }
    
        return Vector3.zero;
    }
    
    

}