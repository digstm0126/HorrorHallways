using sl;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public int maxHealth = 10;
    public Rigidbody fireballPrefab;

    private int currentHealth;
    private float fireBallSpeed = 10f;
    private int fireBallCooldown = 0;

    public TakeDamageScript takeDamageScript;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        //moves player forward when pressing W key
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (fireBallCooldown == 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                ShootFireball();
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        StartCoroutine(takeDamageScript.TakeDamageEffect());

        Debug.Log("Lost " + amount + " HP");

        Debug.Log("You Now have " + currentHealth + " HP");

        //if player health <= 0 then player die
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        //checks if player health isnt already max
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void ShootFireball()
    {
        if (fireBallCooldown < 1)
        {
            //player shoots fireball
            var projectile = Instantiate(fireballPrefab,
                transform.position, transform.rotation);

            projectile.velocity = transform.forward * fireBallSpeed;

            fireBallCooldown = 0;
        }
    }

    private void Die()
    {
        //print that the player dead
        Debug.Log("Player has died.");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered OnTriggerEnter method"); // Add this line for debugging
        if (other.CompareTag("Zombie"))
        {
            Debug.Log("You touched the zombie");
            TakeDamage(2);
        }
    }
}
