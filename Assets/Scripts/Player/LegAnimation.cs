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

    private bool leftMechLegUp = false;
    private bool rightMechLegUp = false;

    void Start(){
        
    }

    void Update()
    {

        if (!detectStep)
        {
            detectStep = FindObjectOfType<DetectStep>();
        }

        if(detectStep){

            detectLeftLeg();
            detectRightLeg();
        }
    }

    private void detectRightLeg()
    {
        if (detectStep.RightFootOnGround)
        {   
            if(rightMechLegUp){
                MoveLeg(mechRightLeg, mechLegDownHeight);
                rightMechLegUp = false;
            
            }
        }
        else
        {
            if (!rightMechLegUp){
                MoveLeg(mechRightLeg, mechLegUpHeight);

                rightMechLegUp = true;
            }
        }
    }

    private void detectLeftLeg()
    {
        if (detectStep.LeftFootOnGround)
        {
            if (leftMechLegUp){
                MoveLeg(mechLeftLeg, mechLegDownHeight);
                leftMechLegUp = false;
            }
        }
        else
        {
            if (!leftMechLegUp){
                MoveLeg(mechLeftLeg, mechLegUpHeight);
                leftMechLegUp = true;
            }
        }
    }

    private void MoveLeg(Transform legTransform, float heightChange)
    {
        Vector3 targetPosition = new Vector3(legTransform.position.x, legTransform.position.y + heightChange, legTransform.position.z);
        legTransform.position = Vector3.Lerp(legTransform.position, targetPosition, legSpeed * Time.deltaTime);
    }
}
