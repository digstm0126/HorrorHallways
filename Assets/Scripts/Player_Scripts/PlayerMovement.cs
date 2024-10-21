using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float liftingSpeedIncreaseRate = 2f;  
    public float landingSpeedBurstRate = 30;       
    public float maxSpeed = 3;                   
    public float liftingDuration = .1f;          
    public float landingDuration = 0.2f;               

    private float speed = 0f;
    public bool debugLog;
    public CameraShake cameraShake;
    public float Speed {
        get { return speed; }
        set { speed = Mathf.Clamp(value, 0, maxSpeed); }
    }

    public enum FootState
    {
        LeftFootLifting,
        LeftFootLanding,
        RightFootLifting,
        RightFootLanding
    }

    private FootState currentFootState = FootState.LeftFootLifting;
    public FootState CurrentFootState
    {
        get { return currentFootState; }
        set { currentFootState = value; }
    }
    
    private Coroutine currentCoroutine;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            CycleFootState();
        }
    }

    // testing helper method
    public void CycleFootState()
    {
        if (currentCoroutine != null){
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
            speed = 0f;
        }
        switch (CurrentFootState)
        {
            case FootState.LeftFootLifting:
                if (debugLog)
                {
                    Debug.Log("Left foot lifting");
                }
                currentCoroutine = StartCoroutine(LiftingSpeed());
                CurrentFootState = FootState.LeftFootLanding;
                break;
            case FootState.LeftFootLanding:
                if (debugLog)
                {
                    Debug.Log("Left foot landing");
                }
                currentCoroutine = StartCoroutine(LandingSpeed());
                CurrentFootState = FootState.RightFootLifting;
                break;
            case FootState.RightFootLifting:
                if (debugLog)
                {
                    Debug.Log("Right foot lifting");
                }
                currentCoroutine = StartCoroutine(LiftingSpeed());
                CurrentFootState = FootState.RightFootLanding;
                break;
            case FootState.RightFootLanding:
                if (debugLog)
                {
                    Debug.Log("Right foot landing");
                }
                currentCoroutine = StartCoroutine(LandingSpeed());
                CurrentFootState = FootState.LeftFootLifting;
                break;
        }
    }

    public void SetInitialFootState(FootState initialState)
    {
        CurrentFootState = initialState;
    }

        
    

    private IEnumerator LandingSpeed()
    {
        if (debugLog) {
            Debug.Log("Landing");
        }
        float elapsedTime = 0f;
        float initialSpeed = speed;  
        float targetSpeed = initialSpeed + landingSpeedBurstRate; 

        cameraShake.ShakeCamera();
        while (elapsedTime < landingDuration)
        {
      
            Speed = Mathf.Lerp(initialSpeed, targetSpeed, elapsedTime / landingDuration);
            elapsedTime += Time.deltaTime;
            if (Math.Abs(elapsedTime % 1) < 0.01f && debugLog){
                //Debug.Log("Speed from landing currently happening");
            }
            yield return null;
        }

        Speed = targetSpeed; 
        if (debugLog) {
            //Debug.Log($"The current time is {elapsedTime:F2}");
        }
        elapsedTime = 0f;
        
        while (elapsedTime < landingDuration)
        {
            
            Speed = Mathf.Lerp(initialSpeed, 0, elapsedTime / landingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Speed = 0f;
        elapsedTime = 0f;
    }

    private IEnumerator LiftingSpeed()
    {
        if (debugLog) {
            Debug.Log("Lifting");
        }
        float elapsedTime = 0f;
        float initialSpeed = speed;  
        float targetSpeed = initialSpeed + liftingSpeedIncreaseRate; 

        while (elapsedTime < liftingDuration)
        {
            
            Speed = Mathf.Lerp(initialSpeed, targetSpeed, elapsedTime / liftingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        

        Speed = targetSpeed;  
        elapsedTime = 0f;
        

        while (elapsedTime < liftingDuration)
        {
            
            Speed = Mathf.Lerp(initialSpeed, 0, elapsedTime / liftingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Speed = 0f;
    }
}
