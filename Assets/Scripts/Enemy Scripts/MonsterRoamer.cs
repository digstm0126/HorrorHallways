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

    public MonsterZone monsterZone;  // Reference to MonsterZone
    private Animator animator;       // Animator component reference

    void Awake()
    {
        animator = GetComponent<Animator>();  // Get the Animator component
    }

    void OnEnable()
    {
        StartCoroutine(MonsterBehavior());  // Start the behavior when enabled
    }

    IEnumerator MonsterBehavior()
    {
        // Start walking animation and move forward
        PlayWalkAnimation();
        yield return StartCoroutine(MoveForward(moveDistance));
        StopWalkAnimation();

        yield return new WaitForSeconds(waitTime);

        // Rotate right
        yield return StartCoroutine(Rotate(rotationAngle));

        yield return new WaitForSeconds(waitTime);

        // Check if the player is in the monster zone
        if (monsterZone.playerInZone)
        {
            Debug.Log("Player detected! Attacking.");
            yield return new WaitForSeconds(1f);  // Wait for the attack animation to finish
            PlayAttackAnimation();  // Play attack animation if player detected

        }
        else
        {
            Debug.Log("No player detected. Rotating back.");
            yield return StartCoroutine(Rotate(-rotationAngle));
        }

        // Move forward again regardless of detection
        PlayWalkAnimation();
        yield return StartCoroutine(MoveForward(secondMoveDistance));
        StopWalkAnimation();

        // Destroy the monster after its actions
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

    // Animation control methods
    private void PlayWalkAnimation()
    {
        animator.SetBool("isWalking", true);
    }

    private void StopWalkAnimation()
    {
        animator.SetBool("isWalking", false);
    }

    private void PlayAttackAnimation()
    {
        animator.SetTrigger("attack");
    }
}
