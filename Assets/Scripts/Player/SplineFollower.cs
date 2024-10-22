using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplineFollower : MonoBehaviour
{
    public SplineContainer splineContainer;
    private Player playerScript;
    private float splineProgress = 0f;

    private Vector3 targetPosition;
    private bool movingPerpendicular = false;
    public float perpendicularMoveSpeed = 2f;
    public float returnToSplineDuration = 0.2f;
    public bool hasHidden;
    public bool debugLog;

    void Start()
    {
        playerScript = GetComponent<Player>();
        targetPosition = transform.position;
    }

    void Update()
    {
        if (!movingPerpendicular && !hasHidden)
        {
            MoveAlongSpline();
        }
    }

    void LateUpdate()
    {
        if (movingPerpendicular)
        {
            Log("Moving perpendicular to target position: " + targetPosition);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * perpendicularMoveSpeed);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                Log("Reached perpendicular target position, stopping perpendicular movement.");
                movingPerpendicular = false;
                hasHidden = true;
            }
        }
    }

    void MoveAlongSpline()
    {
        float currentSpeed = playerScript.GetSpeed();
        splineProgress += (currentSpeed * Time.deltaTime) / splineContainer.Spline.GetLength();
        splineProgress %= 1f;

        float3 position = splineContainer.EvaluatePosition(splineProgress);
        float3 tangent = splineContainer.EvaluateTangent(splineProgress);

        transform.position = new Vector3(position.x, position.y, position.z);

        if (math.length(tangent) > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(tangent.x, tangent.y, tangent.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            Log("Updated rotation based on tangent: " + transform.rotation);

        }
    }

    public void MovePerpendicular(float distance)
    {
        Log("MovePerpendicular called with distance: " + distance);
        float3 tangent = splineContainer.EvaluateTangent(splineProgress);
        Vector3 up = Vector3.up;
        
        Vector3 perpendicular = Vector3.Cross(new Vector3(tangent.x, tangent.y, tangent.z), up).normalized;

        targetPosition = transform.position + (perpendicular * distance);
        
        movingPerpendicular = true;
        
        Log("Perpendicular movement started. Target position: " + targetPosition);
        
    }
    
    public IEnumerator ReturnToSpline()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;  // Capture the current position at the start of the return

        while (elapsedTime < returnToSplineDuration)
        {
            // Dynamically calculate the current spline position
            float3 splinePosition = splineContainer.EvaluatePosition(splineProgress);
            Vector3 currentSplinePosition = new Vector3(splinePosition.x, splinePosition.y, splinePosition.z);

            // Lerp back to the current spline position
            transform.position = Vector3.Lerp(startPosition, currentSplinePosition, elapsedTime / returnToSplineDuration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure exact position at the spline at the end
        float3 finalSplinePosition = splineContainer.EvaluatePosition(splineProgress);
        transform.position = new Vector3(finalSplinePosition.x, finalSplinePosition.y, finalSplinePosition.z);
        hasHidden = false;
        Log("Returned to spline at position: " + transform.position);
    }



    public void StopMoving()
    {
        playerScript.SetSpeed(0f);
    }

    private void Log(string message)
    {
        if (debugLog)
        {
            Debug.Log(message);
        }
    }
}
