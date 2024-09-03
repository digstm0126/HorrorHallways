using UnityEngine;

public class PositionLocker : MonoBehaviour
{
    private Vector3 initialPosition;

    void Start()
    {
        // Store the initial position of the character
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        // Reset the character's position to the initial position every frame
        transform.position = initialPosition;

        // Optionally, lock rotation as well if you only want walking in place with no turning
        // transform.rotation = Quaternion.identity;
    }
}
