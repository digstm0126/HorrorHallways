using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monster; // Reference to the monster object

    private void Start()
    {
        // Ensure the monster is disabled initially
        if (monster != null)
        {
            monster.SetActive(false);
        }
    }

    // Detect when the player enters the spawner's trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the collider belongs to the player
        {
            Debug.Log("Player entered spawner. Activating monster!");
            if (monster != null)
            {
                monster.SetActive(true); // Enable the monster object
            }
        }
    }
}
