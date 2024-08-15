using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public int maxHealth = 10;
    public Rigidbody fireballPrefab;

    private int currentHealth;
    private float fireBallSpeed = 10f;
    private bool canShoot = true;
    public float fireBallCooldownDuration = 0.4f;

    public TakeDamageScript takeDamageScript;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Move player forward when pressing W key
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        // Shoot fireball when pressing Space and canShoot is true
        if (Input.GetKey(KeyCode.Space) && canShoot)
        {
            ShootFireball();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        StartCoroutine(takeDamageScript.TakeDamageEffect());

        Debug.Log("Lost " + amount + " HP");
        Debug.Log("You Now have " + currentHealth + " HP");

        // If player health <= 0 then player dies
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        // Check if player health isn't already max
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void ShootFireball()
    {
        // Instantiate the fireball
        var projectile = Instantiate(fireballPrefab, transform.position, transform.rotation);
        projectile.velocity = transform.forward * fireBallSpeed;

        // Start the cooldown coroutine
        StartCoroutine(FireballCooldown());
    }

    private IEnumerator FireballCooldown()
    {
        // Disable shooting
        canShoot = false;

        // Wait for the cooldown duration
        yield return new WaitForSeconds(fireBallCooldownDuration);

        // Re-enable shooting
        canShoot = true;
    }

    private void Die()
    {
        // Print that the player is dead
        Debug.Log("Player has died.");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered OnTriggerEnter method");
        if (other.CompareTag("Zombie"))
        {
            Debug.Log("You touched the zombie");
            TakeDamage(2);
        }
    }
}
