using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigZombieScript : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public bool awake = true;
    private Transform player;
    public float health = 5f; // HP for the BigZombie

    private ScoreManager scoreManager;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && awake)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        transform.position += direction * moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (scoreManager != null)
        {
            scoreManager.AddScore(1000);
        }

        Destroy(gameObject); // Destroy the BigZombie object
    }
}
