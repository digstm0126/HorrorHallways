using System.Collections;
using UnityEngine;

public class InvisibleWallScript : MonoBehaviour
{
    public float detectionRadius = 5f; // Radius to detect explosive barrels
    private Player playerScript;
    private bool playerTouchingWall = false;
    public bool zed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched invisible wall");

            playerScript = other.GetComponent<Player>();
            if (playerScript != null)
            {
                playerTouchingWall = true;
                playerScript.SetSpeed(0f); // Stop the player's movement
                StartCoroutine(CheckForExplosiveBarrels());
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && playerTouchingWall)
        {
            if (playerScript)
            {
                playerScript.SetSpeed(0f);
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

                if (playerTouchingWall && !zed)
                {
                    playerScript.SetSpeed(0); // Resume the player's speed
                }
                else
                {
                    Debug.Log("Untick zed option on invisible wall");
                }

                yield break; // Exit coroutine
            }

            yield return new WaitForSeconds(0.5f); // Check every 0.5 seconds
        }
    }
}
