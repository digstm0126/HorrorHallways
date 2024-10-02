using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speedIncreaseAmount = 1f;

    private float speed;

    public float Speed {
        get { return speed; }
        set { speed = value; }
    }

    private enum FootState
    {
        LeftFootLanding,
        LeftFootLifting,
        RightFootLanding,
        RightFootLifting
    }

    private FootState currentFootState = FootState.LeftFootLanding;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            CycleFootState();
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 2, ForceMode.Impulse);

        }
    }
    // testing helper method
    private void CycleFootState()
    {
        switch (currentFootState)
        {
            case FootState.LeftFootLanding:
                LandingSpeed();
                currentFootState = FootState.LeftFootLifting;
                break;
            case FootState.LeftFootLifting:
                LiftingSpeed();
                currentFootState = FootState.RightFootLanding;
                break;
            case FootState.RightFootLanding:
                LandingSpeed();
                currentFootState = FootState.RightFootLifting;
                break;
            case FootState.RightFootLifting:
                LiftingSpeed();
                currentFootState = FootState.LeftFootLanding;
                break;
        }
    }

        
    

    public IEnumerator LandingSpeed(){
        


    }
    public void LiftingSpeed(){

    }
}
