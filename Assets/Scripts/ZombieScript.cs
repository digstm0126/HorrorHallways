using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 10f;

    private Transform player;

    void Start()
    {
        // Find the player object in the scene
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        // Check the distance between the zombie and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // If the player is within the detection range, move towards the player
        if (distanceToPlayer <= detectionRange)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction from the zombie to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Move the zombie towards the player
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Optionally, you can also rotate the zombie to face the player
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
    }
}
