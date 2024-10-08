using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float liftingSpeedIncreaseRate = 2f;  // Rate of speed increase for lifting
    public float landingSpeedBurstRate = 50f;       // Rate of speed burst for landing
    public float maxSpeed = 100f;                   // Cap the maximum speed
    public float liftingDuration = 10f;           // Duration for lifting
    public float landingDuration = 0.2f;               // How long the speed adjustment lasts

    private float speed = 0f;

    public float Speed {
        get { return speed; }
        set { speed = Mathf.Clamp(value, 0, maxSpeed); }
    }

    private enum FootState
    {
        LeftFootLifting,
        LeftFootLanding,
        RightFootLifting,
        RightFootLanding
    }

    private FootState currentFootState = FootState.LeftFootLifting;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            CycleFootState();
            

        }
    }
    // testing helper method
    private void CycleFootState()
    {
        switch (currentFootState)
        {
            case FootState.LeftFootLanding:
                StartCoroutine(LandingSpeed());
                currentFootState = FootState.LeftFootLifting;
                break;
            case FootState.LeftFootLifting:
                StartCoroutine(LiftingSpeed());
                currentFootState = FootState.RightFootLanding;
                break;
            case FootState.RightFootLanding:
                StartCoroutine(LandingSpeed());
                currentFootState = FootState.RightFootLifting;
                break;
            case FootState.RightFootLifting:
                StartCoroutine(LiftingSpeed());
                currentFootState = FootState.LeftFootLanding;
                break;
        }
    }

        
    

    private IEnumerator LandingSpeed()
    {
        float elapsedTime = 0f;
        float initialSpeed = speed;  
        float targetSpeed = initialSpeed + liftingSpeedIncreaseRate; 

        while (elapsedTime < landingDuration)
        {
      
            Speed = Mathf.Lerp(initialSpeed, targetSpeed, elapsedTime / landingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Speed = targetSpeed; 
        elapsedTime = 0f;
        

        while (elapsedTime < 1)
        {
            
            Speed = Mathf.Lerp(initialSpeed, 0, elapsedTime / liftingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Speed = 0f;
    }

    private IEnumerator LiftingSpeed()
    {
        float elapsedTime = 0f;
        float initialSpeed = speed;  
        float targetSpeed = initialSpeed + landingSpeedBurstRate; 

        while (elapsedTime < liftingDuration)
        {
            
            Speed = Mathf.Lerp(initialSpeed, targetSpeed, elapsedTime / liftingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        

        Speed = targetSpeed;  
        elapsedTime = 0f;
        

        while (elapsedTime < 1)
        {
            
            Speed = Mathf.Lerp(initialSpeed, 0, elapsedTime / liftingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Speed = 0f;
    }
}
