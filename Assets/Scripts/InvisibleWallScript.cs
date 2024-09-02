using System.Collections;
using UnityEngine;

public class InvisibleWallScript : MonoBehaviour
{
    public float detectionRadius = 5f; // Radius to detect explosive barrels
    private Player playerScript;
    private bool playerTouchingWall = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched invisible wall");

            playerScript = other.GetComponent<Player>();
            if (playerScript != null)
            {
                playerTouchingWall = true;
                playerScript.speed = 0f; // Stop the player's movement
                StartCoroutine(CheckForExplosiveBarrels());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTouchingWall = false;
        }
    }

    private IEnumerator CheckForExplosiveBarrels()
    {
        while (playerTouchingWall)
        {
            Collider[] barrels = Physics.OverlapSphere(transform.position, detectionRadius);
            bool barrelsNearby = false;

            foreach (Collider barrel in barrels)
            {
                if (barrel.CompareTag("ExplosiveBarrel"))
                {
                    barrelsNearby = true;
                    break;
                }
            }

            if (!barrelsNearby)
            {
                Destroy(gameObject); // Remove the invisible wall

                if (playerTouchingWall)
                {
                    playerScript.speed = 3f; // Resume the player's speed
                }

                yield break; // Exit coroutine
            }

            yield return new WaitForSeconds(0.5f); // Check every 0.5 seconds
        }
    }
}
