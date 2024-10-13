using UnityEngine;
using System.Collections;

public class MonsterScript : MonoBehaviour
{
    public float moveDistance = 5f;  // Distance the monster will move forward
    public float moveSpeed = 2f;     // Speed at which the monster moves forward
    public float rotationAngle = 90f; // Angle the monster will rotate to the right
    public float rotationSpeed = 100f; // Speed of the rotation
    public float waitTime = 1f;       // Time to wait after each action

    private bool isMoving = false;    // Tracks if the monster is already performing the action
    private bool actionTriggered = false; // Tracks if the action is triggered by the player

    void Update()
    {
        // Check if the "E" key is pressed and the action has not yet been triggered
        if (Input.GetKeyDown(KeyCode.E) && !actionTriggered)
        {
            actionTriggered = true;
            StartCoroutine(MonsterBehavior()); // Start the monster behavior
        }
    }

    IEnumerator MonsterBehavior()
    {
        // Move forward
        isMoving = true;
        yield return StartCoroutine(MoveForward(moveDistance));
        isMoving = false;

        // Wait before rotating
        yield return new WaitForSeconds(waitTime);

        // Rotate to the right
        isMoving = true;
        yield return StartCoroutine(Rotate(rotationAngle));
        isMoving = false;

        // Wait before rotating back
        yield return new WaitForSeconds(waitTime);

        // Rotate back to the original direction
        isMoving = true;
        yield return StartCoroutine(Rotate(-rotationAngle));
        isMoving = false;

        // Wait before moving forward again
        yield return new WaitForSeconds(waitTime);

        // Move forward again
        isMoving = true;
        yield return StartCoroutine(MoveForward(moveDistance));
        isMoving = false;

        // Destroy the game object after the second movement
        Destroy(gameObject);
    }

    IEnumerator MoveForward(float distance)
    {
        Vector3 targetPosition = transform.position + transform.forward * distance;
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Rotate(float angle)
    {
        Quaternion targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + angle, 0);
        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
