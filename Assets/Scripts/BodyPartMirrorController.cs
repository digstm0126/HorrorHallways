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
                SetBodyPartObjectLocation(leftHandObject, "Character1_LeftHandMiddle1");
                SetBodyPartObjectLocation(rightHandObject, "Character1_RightHandMiddle1");
            }
        }
        else
        {
            SetBodyPartObjectLocation(leftHandObject,   "Character1_LeftHandMiddle1");
            SetBodyPartObjectLocation(rightHandObject, "Character1_RightHandMiddle1");
        }
    }

    private void SetBodyPartObjectLocation(GameObject bodyPartObject, String bodyPartName)
    {
        Transform bodyTransform = getAvatarInformation.GetBodyPartTransform(bodyPartName);

        if (bodyTransform)
        {
            Vector3 newPosition = TransformUtilities.TransformPositionFromAvatarToPlayer(
                bodyTransform.position,
                getAvatarInformation.GetBodyPartTransform("Character1_Hips"),
                transform);
            
            bodyPartObject.transform.position = newPosition;
        }
        
        else if (enableLogging)
        {
            Debug.LogWarning("Body part not found: " + bodyPartName);
        }
    }
    
}