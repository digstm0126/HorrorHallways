using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartMirrorController : MonoBehaviour
{
    private GetAvatarInformation getAvatarInformation;

    public GameObject leftHandObject;
    public GameObject rightHandObject;
    
    // Add a public variable to control logging
    public bool enableLogging = false;

    void Start()
    {
        getAvatarInformation = FindObjectOfType<GetAvatarInformation>();
    }

    void Update()
    {
        SetObjectLocation(leftHandObject, "Character1_LeftHandMiddle1");
        SetObjectLocation(rightHandObject, "Character1_RightHandMiddle1");
    }

    private void SetObjectLocation(GameObject bodyPartObject, string bodyPartName)
    {
        bodyPartObject.transform.position = GetBodyPartLocation(bodyPartName);
    }
    
    private Vector3 GetBodyPartLocation(string bodyName)
    {
        Transform bodyTransform = getAvatarInformation.GetBodyPartTransform(bodyName);

        if (bodyTransform)
        {
            // Log only if logging is enabled
            if (enableLogging)
            {
                Debug.Log("Setting position for: " + bodyName);
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