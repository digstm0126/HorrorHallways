using UnityEngine;
using UnityEngine.Serialization;

public class DetectStep : MonoBehaviour
{
    public Transform leftFoot;
    public Transform rightFoot;
    public float distanceToHips = 0.4f;
    public float distanceFromHips = 0.4f;


    private PlayerMovement playerMovement;
    private GetAvatarInformation getAvatarInformation;


    private enum LastFootStepped { None, Left, Right }

    private LastFootStepped lastFootStepped = LastFootStepped.None;

    
    public bool enableDebugLogs = true;
    
    
    private bool leftFootOnGround = true;
    private bool rightFootOnGround = true;
    bool footCloseToHips;
    bool footFarFromHips;
    private bool leftFootValidStepInitiated = false;
    private bool rightFootValidStepInitiated = false;

    public bool LeftFootOnGround
    {
        get { return leftFootOnGround; }
    }

    public bool RightFootOnGround
    {
        get { return rightFootOnGround; }
    }
    

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement)
        {
            Log("Player script found and assigned.");
        }
        else
        {
            LogError("Player script not found!");
        }
    }

    void Update()
    {

        if (!getAvatarInformation){
            getAvatarInformation = GetComponent<GetAvatarInformation>();
        }
        float leftFootDistance = GetVerticalDistanceFromRoot(leftFoot);
        float rightFootDistance = GetVerticalDistanceFromRoot(rightFoot);

        Log($"Update: Left Foot Distance: {leftFootDistance}, Right Foot Distance: {rightFootDistance}");

        CheckStep(leftFootDistance, ref leftFootOnGround, "Left Foot", LastFootStepped.Left);
        CheckStep(rightFootDistance, ref rightFootOnGround, "Right Foot", LastFootStepped.Right);
    }



    float GetVerticalDistanceFromRoot(Transform foot)
    {
        Vector3 footPosition = foot.position;
        Vector3 hipPosition = getAvatarInformation.GetBodyPartTransform("Character1_Hips").position;

        float distance = hipPosition.y - footPosition.y;
        Log($"GetVerticalDistanceFromRoot: Foot {foot.name}, Position: {footPosition.y}, Root Position: {hipPosition.y}, Distance: {distance}");
        
        return distance;
        
    }

    void CheckStep(float footDistance, ref bool footOnGround, string footName, LastFootStepped currentFoot)
    {
        footCloseToHips = footDistance <= distanceToHips;
        footFarFromHips = footDistance > distanceFromHips;
        if (footCloseToHips && footOnGround)
        {
            footOnGround = FootBegunStep(footDistance, footName, currentFoot);
        }
        else if (footFarFromHips && !footOnGround)
        {
            footOnGround = FootLandedFinishedStep(footDistance, footName, currentFoot);
        }
        else
        {
            Log($"{footName}: No state change. Foot Distance: {footDistance}, Foot On Ground: {footOnGround}");
        }
    }
    // method to encapsulate logic when the step is begun in the check step method
    // returns a boolean flag that will track if the current foot is in the air or not
    private bool FootBegunStep(float footDistance, string footName, LastFootStepped currentFoot)
    {

        //Foot detected to be in the air
        bool footOnGround = false;
        Log($"{footName} has taken a step. Foot Distance: {footDistance}, Step Threshold: {distanceToHips}");

        if (IsValidStep(currentFoot))
        {
            Log($"Valid Step Detected: {footName}. Last Foot Stepped: {lastFootStepped}, Current Foot: {currentFoot}");

            if (playerMovement)
            {
                Log($"Calling SpeedIncrease Coroutine for {footName}");

                playerMovement.CycleFootState();
                Debug.Log("Begun Step - Detect Step foot: " + footName);
            }
            // flagging current step with current foot as valid so that the finishing function doesnt call the state machine unconditionally
            SetValidStepInitiatedFlag(currentFoot, true);
        }
        else
        {
            Log($"Invalid Step: {footName}. Last Foot Stepped: {lastFootStepped}, Current Foot: {currentFoot}");
        }

        return footOnGround;
    }
    // method to encapsulate logic when the step is finished in the check step method

    private bool FootLandedFinishedStep(float footDistance, string footName, LastFootStepped currentFoot)
    {
        //Foot detected to be on the ground
        bool footOnGround = true;
        
        /// lets the state machine know what foot is being stepped with to start with, else it just begins with left no matter what
        if (lastFootStepped == LastFootStepped.None)
        {
            InitializeFirstStep(currentFoot);
        }

        lastFootStepped = currentFoot;
        // dont call valid step logic if not part of valdi step
        if (!WasValidStepInitiated(currentFoot)){
            return footOnGround;
        }

        if (playerMovement)
        {
            Debug.Log("Finished Step - Detect Step foot:" + footName);
            playerMovement.CycleFootState();
        }
        SetValidStepInitiatedFlag(currentFoot, false);

        Log($"{footName} has touched the ground. Foot Distance: {footDistance}, Step Threshold: {distanceFromHips}");
        return footOnGround;
    }

    

    bool IsValidStep(LastFootStepped currentFoot)
    {
        // Accept any step as valid if it's the first step
        if (lastFootStepped == LastFootStepped.None)
        {
            Log($"First Step Detected: {currentFoot}. Accepting as valid.");
            return true;
        }

        // Validate alternating steps
        bool isValid = currentFoot != lastFootStepped;
        Log($"IsValidStep Check: Current Foot: {currentFoot}, Last Foot: {lastFootStepped}, IsValid: {isValid}");
        return isValid;
    }
    // storing the validitiy of each foots stepping status, as to not call the state machine if the player tries stepping with the same leg both times.
    private void SetValidStepInitiatedFlag(LastFootStepped foot, bool value)
    {
        if (foot == LastFootStepped.Left)
        {
            leftFootValidStepInitiated = value;
        }
        else if (foot == LastFootStepped.Right)
        {
            rightFootValidStepInitiated = value;
        }
    }
    // important to check on the landing if its finishing a valid step, or an invalid one.
    private bool WasValidStepInitiated(LastFootStepped foot)
    {
        if (foot == LastFootStepped.Left)
        {
            return leftFootValidStepInitiated;
        }
        else if (foot == LastFootStepped.Right)
        {
            return rightFootValidStepInitiated;
        }
        return false;
    }

    void InitializeFirstStep(LastFootStepped firstFoot){
        if (firstFoot == LastFootStepped.Left){
            //landing instead of lifting because by the time this is called, have already begun a step
            playerMovement.SetInitialFootState(PlayerMovement.FootState.LeftFootLanding);

        } else if(firstFoot == LastFootStepped.Right){
            playerMovement.SetInitialFootState(PlayerMovement.FootState.RightFootLanding);
        }
    }


    // Helper methods for logging
    private void Log(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log(message);
        }
    }

    private void LogError(string message)
    {
        if (enableDebugLogs)
        {
            Debug.LogError(message);
        }
    }
}