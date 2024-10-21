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
    public bool debugLog;

    void Start()
    {
        playerScript = GetComponent<Player>();
        targetPosition = transform.position;
    }

    void Update()
    {
        MoveAlongSpline();
    }

    void LateUpdate()
    {
        if (movingPerpendicular)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * perpendicularMoveSpeed);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                movingPerpendicular = false;
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
        }
    }

    public void MovePerpendicular(float distance)
    {
        Log("called MovePerpendicular()");
        float3 tangent = splineContainer.EvaluateTangent(splineProgress);
        Vector3 up = Vector3.up;
        
        Vector3 perpendicular = Vector3.Cross(new Vector3(tangent.x, tangent.y, tangent.z), up).normalized;

        targetPosition = transform.position + (perpendicular * distance);
        
        movingPerpendicular = true;
        Log("MovePerpendicular script from Spline follower: moving perpendicular is called and reaches end");
        
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
