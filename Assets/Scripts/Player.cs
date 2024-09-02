using System.Collections;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public float speed = 3f;
    public Rigidbody fireballPrefab;
    public GameObject youLoseTextTMP;

    private float fireBallSpeed = 10f;
    private bool canShoot = true;
    public float fireBallCooldownDuration = 0.4f;
    private bool isGameOver = false;

    public HealthManager healthManager; // Reference to HealthManager
    public TakeDamageScript takeDamageScript;

    private ScoreManager scoreManager;

    GetDirection getDirection;
    
    


    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        // Initialize HealthManager if not set in the Inspector
        if (healthManager == null)
        {
            healthManager = FindObjectOfType<HealthManager>();
        }
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
            ShootFireball(true);
            
        }
    }

    private void TakeDamage(int amount)
    {
        if (isGameOver) return;

        healthManager.TakeDamage(amount);

        StartCoroutine(takeDamageScript.TakeDamageEffect());

        Debug.Log("Lost " + amount + " HP");
        Debug.Log("You Now have " + healthManager.CurrentHealth + " HP");

        if (healthManager.CurrentHealth <= 0)
        {
            GameOver();
        }
    }

    public void Heal(int amount)
    {
        if (isGameOver) return;

        healthManager.Heal(amount); // Call HealthManager to handle healing
        Debug.Log("Healed " + amount + " HP. Current HP: " + healthManager.CurrentHealth);
    }
    
    public void ShootFireball(bool isLeft)
    {
        if (!getDirection)
        {
            var projectile = Instantiate(fireballPrefab, transform.position, transform.rotation);
            projectile.velocity = transform.forward * fireBallSpeed;

            StartCoroutine(FireballCooldown()); 
        }
        else
        {
            // can be called in TriggerFireball.cs on the avatar prefab
            var projectile = Instantiate(fireballPrefab, transform.position, transform.rotation);

            Vector3 armForwardVector = !isLeft ? getDirection.GetRightArmForwardVector() : getDirection.GetLeftArmForwardVector();

            armForwardVector.y = 0;
            armForwardVector = armForwardVector.normalized;

            Quaternion playerRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            Vector3 alignedDirection = playerRotation * armForwardVector;
            alignedDirection = -alignedDirection;
            
            projectile.velocity = alignedDirection * fireBallSpeed;
            
        }



    }

    private IEnumerator FireballCooldown()
    {
        canShoot = false;

        yield return new WaitForSeconds(fireBallCooldownDuration);

        canShoot = true;
    }

    private void GameOver()
    {
        isGameOver = true;
        speed = 0.0f;

        scoreManager.scoreText.gameObject.SetActive(false);

        youLoseTextTMP.SetActive(true);
        int finalScore = scoreManager.GetScore();
        youLoseTextTMP.GetComponent<TextMeshProUGUI>().text = $"You Lose! You scored: {finalScore}";

        Debug.Log("Player has died. Displaying score");
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
