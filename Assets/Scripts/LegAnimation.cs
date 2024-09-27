using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegAnimation : MonoBehaviour
{
    public Transform mechLeftLeg;
    public Transform mechRightLeg;


    public float mechLegUpHeight;
    public float mechLegDownHeight;

    public DetectStep detectStep;

    public float legSpeed;

    void Update()
    {
        if (detectStep.LeftFootStatus)
        {
            MoveLeg(mechLeftLeg, mechLegDownHeight);
        }
        else
        {
            MoveLeg(mechLeftLeg, mechLegUpHeight); 
        }

        if (detectStep.RightFootStatus)
        {
            MoveLeg(mechRightLeg, mechLegDownHeight);
        }
        else
        {
            MoveLeg(mechRightLeg, mechLegUpHeight);
        }
    }

    private void MoveLeg(Transform legTransform, float targetHeight)
    {
        Vector3 targetPosition = new Vector3(legTransform.position.x, targetHeight, legTransform.position.z);
        legTransform.position = Vector3.Lerp(legTransform.position, targetPosition, legSpeed * Time.deltaTime);
    }
}
