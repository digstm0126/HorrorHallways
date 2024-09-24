
using UnityEngine;

public class GetAvatarInformation : MonoBehaviour
{
    public GameObject leftForearm;
    public GameObject rightForearm;

    public GameObject leftShoulder;
    public GameObject rightShoulder;

    public Vector3 GetRightArmForwardVector()
    {
        return rightForearm.transform.right;
    }

    public Vector3 GetLeftArmForwardVector()
    {
        return -leftForearm.transform.right;
    }

    public Transform GetBodyPartTransform(string avatarBoneName)
    {
        Transform avatarBone = transform.Find(avatarBoneName);
        if (avatarBone)
        {
            return avatarBone;
        }
        
        return null;
    }
}
