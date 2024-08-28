using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDirection : MonoBehaviour
{
    public GameObject leftForearm;
    public GameObject rightForearm;

    public Vector3 GetRightArmForwardVector()
    {
        return rightForearm.transform.right;
    }

    public Vector3 GetLeftArmForwardVector()
    {
        return -leftForearm.transform.right;
    }
}
