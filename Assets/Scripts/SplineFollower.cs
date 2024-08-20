using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class SplineFollower : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float baseSpeed = 5f;
    private float currentSpeed;
    private float splineProgress = 0f;


    void Start()
    {
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentSpeed += scroll * baseSpeed;

        currentSpeed = Mathf.Clamp(currentSpeed, 0f, baseSpeed * 10f);

        MoveAlongSpline();

    }

    void MoveAlongSpline()
    {
        // ChatGPT helped a lot with smoothing out the rotation of the splines
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


    void StopMoving()
    {
        currentSpeed = 0f;
    }
}
