using UnityEngine;
using System.Collections;

public class MonsterRoamer : MonoBehaviour
{
    public float moveDistance = 13f;
    public float secondMoveDistance = 20f;
    public float moveSpeed = 8f;
    public float rotationAngle = 105f;
    public float rotationSpeed = 60f;
    public float waitTime = 1f;

    private bool isMoving;
    private bool actionTriggered = false;

    public Transform detectionRange; // Child object for detection range

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !actionTriggered)
        {
            actionTriggered = true;
            StartCoroutine(MonsterBehavior());
        }
    }

    private void MonsterWalking()
    {
        Debug.Log(isMoving ? "Triggered Moving Animation" : "Not moving right now");
    }

    private bool IsPlayerInRange()
    {
        Collider[] hits = Physics.OverlapSphere(detectionRange.position, detectionRange.localScale.x / 2);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator MonsterBehavior()
    {
        // First forward movement
        isMoving = true;
        MonsterWalking();
        yield return StartCoroutine(MoveForward(moveDistance));
        isMoving = false;

        yield return new WaitForSeconds(waitTime);

        // Rotate right
        isMoving = true;
        yield return StartCoroutine(Rotate(rotationAngle));
        isMoving = false;

        yield return new WaitForSeconds(waitTime);

        // Check for player in range
        bool playerDetected = IsPlayerInRange();

        if (playerDetected)
        {
            Debug.Log("Player detected! Moving forward.");
            isMoving = true;
            yield return StartCoroutine(MoveForward(secondMoveDistance));
        }
        else
        {
            Debug.Log("Player not detected. Rotating back.");
            isMoving = true;

            // Rotate back to original direction
            yield return StartCoroutine(Rotate(-rotationAngle));

            // Ensure monster moves forward even when no player is detected
            yield return StartCoroutine(MoveForward(secondMoveDistance));
        }

        isMoving = false;

        // Final action: Destroy the monster
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
