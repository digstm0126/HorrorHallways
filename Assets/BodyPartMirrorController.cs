using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartMirrorController : MonoBehaviour
{
    
    private GetAvatarInformation getAvatarInformation;

    public GameObject leftHandObject;
    public GameObject rightHandObject;
    
    
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
            return bodyTransform.position;
        }

        Debug.LogWarning("Body part not found: " + bodyName);
        return Vector3.zero;
    }
}
