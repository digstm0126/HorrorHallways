using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSideMovement : MonoBehaviour
{
    private float startXPosition = 0f;
    void Update()
    {
        float distanceMovedX = transform.position.x - startXPosition;

        if (distanceMovedX >= 0.5f)
        {
            //do something to the left - call function
        }
        else if (distanceMovedX <= -0.5f)
        {
            // do something to the right - call function
        }
    }


}
