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
    GetDirection getDirection;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!getDirection)
        {
            getDirection = FindObjectOfType<GetDirection>();
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
            
        }

        if (Input.GetKey(KeyCode.Z) && canShoot)
        {
            ShootFireball();
            StartCoroutine(FireballCooldown());
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        StartCoroutine(takeDamageScript.TakeDamageEffect());

        Debug.Log("Lost " + amount + " HP");
        Debug.Log("You Now have " + currentHealth + " HP");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void ShootFireball()
    {
        /*
        var projectile = Instantiate(fireballPrefab, transform.position, transform.rotation);
        projectile.velocity = transform.forward * fireBallSpeed;

        StartCoroutine(FireballCooldown()); */
        
        var projectile = Instantiate(fireballPrefab, transform.position, transform.rotation);
        
        Vector3 modelArmWorldRight = getDirection.GetRightArmForwardVector();

        modelArmWorldRight.y = 0;
        modelArmWorldRight = modelArmWorldRight.normalized;

        Quaternion playerRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Vector3 alignedDirection = playerRotation * modelArmWorldRight;
        alignedDirection = -alignedDirection;

        
        
        
        projectile.velocity = alignedDirection * fireBallSpeed;
        
        
        
    }

    private IEnumerator FireballCooldown()
    {
        canShoot = false;

        yield return new WaitForSeconds(fireBallCooldownDuration);

        canShoot = true;
    }

    private void Die()
    {
        Debug.Log("Player has died.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            TakeDamage(2);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Spider"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }

    }
}
