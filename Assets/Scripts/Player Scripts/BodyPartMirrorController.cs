using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartMirrorController : MonoBehaviour
{
    private GetAvatarInformation getAvatarInformation;

    public GameObject leftHandObject;
    public GameObject rightHandObject;
    public GameObject leftKneeObject;
    public GameObject rightKneeObject;
    
    // Add a public variable to control logging
    public bool enableLogging;



    void Update()
    {
        if (!getAvatarInformation)
        {
            getAvatarInformation = FindObjectOfType<GetAvatarInformation>();
            if (getAvatarInformation)
            {
                CallAllBodyPartLocations();
            }
        }
        else
        {
            CallAllBodyPartLocations();
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

    private void CallAllBodyPartLocations(){
        SetBodyPartObjectLocation(leftHandObject, "Character1_LeftHandMiddle1");
        SetBodyPartObjectLocation(rightHandObject, "Character1_RightHandMiddle1");
        SetBodyPartObjectLocation(leftKneeObject, "Character1_LeftLeg");
        SetBodyPartObjectLocation(rightKneeObject, "Character1_RightLeg");
    }
    
}