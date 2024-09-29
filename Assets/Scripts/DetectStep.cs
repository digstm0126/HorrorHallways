using UnityEngine;

public class DetectStep : MonoBehaviour
{
    public Transform leftFoot;
    public Transform rightFoot;
    public float stepThreshold = 0.4f;
    private bool leftFootOnGround = true;
    private bool rightFootOnGround = true;

    private Player player;

    private enum LastFootStepped { None, Left, Right }
    private LastFootStepped lastFootStepped = LastFootStepped.None;

    // Toggle for debug logs
    public bool enableDebugLogs = true;

    void Start()
    {
        player = FindObjectOfType<Player>();
        if (player)
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
        float leftFootDistance = GetVerticalDistanceFromRoot(leftFoot);
        float rightFootDistance = GetVerticalDistanceFromRoot(rightFoot);

        Log($"Update: Left Foot Distance: {leftFootDistance}, Right Foot Distance: {rightFootDistance}");

        CheckStep(leftFootDistance, ref leftFootOnGround, "Left Foot", LastFootStepped.Left);
        CheckStep(rightFootDistance, ref rightFootOnGround, "Right Foot", LastFootStepped.Right);
    }

    public bool LeftFootOnGround
    {
        get { return leftFootOnGround; }
    }

    public bool RightFootOnGround
    {
        get { return rightFootOnGround; }
    }


    float GetVerticalDistanceFromRoot(Transform foot)
    {
        Vector3 footPosition = foot.position;
        Vector3 rootPosition = transform.position;

        float distance = rootPosition.y - footPosition.y;
        Log($"GetVerticalDistanceFromRoot: Foot {foot.name}, Position: {footPosition.y}, Root Position: {rootPosition.y}, Distance: {distance}");

        return distance;
    }

    void CheckStep(float footDistance, ref bool footOnGround, string footName, LastFootStepped currentFoot)
    {
        if (footDistance > stepThreshold && !footOnGround)
        {
            
            
            footOnGround = true;
            
            
            Log($"{footName} has touched the ground. Foot Distance: {footDistance}, Step Threshold: {stepThreshold}");
        }
        else if (footDistance <= stepThreshold && footOnGround)
        {
            
            
            footOnGround = false;
            
            
            Log($"{footName} has taken a step. Foot Distance: {footDistance}, Step Threshold: {stepThreshold}");

            if (IsValidStep(currentFoot))
            {
                Log($"Valid Step Detected: {footName}. Last Foot Stepped: {lastFootStepped}, Current Foot: {currentFoot}");
                
                
                lastFootStepped = currentFoot;

                if (player)
                {
                    Log($"Calling SpeedIncrease Coroutine for {footName}");
                    
                    
                    player.StartCoroutine(player.SpeedIncrease());
                    
                }
            }
            else
            {
                Log($"Invalid Step: {footName}. Last Foot Stepped: {lastFootStepped}, Current Foot: {currentFoot}");
            }
        }
        else
        {
            Log($"{footName}: No state change. Foot Distance: {footDistance}, Foot On Ground: {footOnGround}");
        }
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
        bool isValid = (currentFoot == LastFootStepped.Left && lastFootStepped == LastFootStepped.Right) ||
                       (currentFoot == LastFootStepped.Right && lastFootStepped == LastFootStepped.Left);
        Log($"IsValidStep Check: Current Foot: {currentFoot}, Last Foot: {lastFootStepped}, IsValid: {isValid}");
        Log($"Player speed is {player.speed}");
        return isValid;
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