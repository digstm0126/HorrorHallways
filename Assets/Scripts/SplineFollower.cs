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

        /*
        if (Input.GetKey(KeyCode.Space))
        {
            StopMoving();
        }
        else
        {
            currentSpeed += scroll * baseSpeed;
        }*/
    }

    void MoveAlongSpline()
    {
        
        splineProgress += (currentSpeed * Time.deltaTime) / splineContainer.Spline.GetLength();

        
        splineProgress %= 1f;

        
        float3 position = splineContainer.EvaluatePosition(splineProgress);
        transform.position = new Vector3(position.x, position.y, position.z);
    }


    void StopMoving()
    {
        currentSpeed = 0f;
    }
}
