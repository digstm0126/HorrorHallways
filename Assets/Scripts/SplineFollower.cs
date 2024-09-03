using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplineFollower : MonoBehaviour
{
    public SplineContainer splineContainer;
    private Player playerScript;
    private float splineProgress = 0f;

    void Start()
    {
        playerScript = GetComponent<Player>();
    }

    void Update()
    {
        MoveAlongSpline();
    }

    void MoveAlongSpline()
    {
        float currentSpeed = playerScript.speed;
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

    public void StopMoving()
    {
        playerScript.speed = 0f;
    }
}
