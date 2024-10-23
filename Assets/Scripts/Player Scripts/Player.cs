using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    
    public GameObject youLoseTextTMP;
    
    
    private bool isGameOver = false;

    public HealthManager healthManager; // Reference to HealthManager
    public TakeDamageScript takeDamageScript;

    private ScoreManager scoreManager;
    
    private FireBallShooter fireBallShooter;
    
    public FireBallShooter FireBallShooter
    {
        get { return fireBallShooter; }
    }

    
    PlayerMovement playerMovement;
    

    
    

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        // Initialize HealthManager if not set in the Inspector
        if (!healthManager)
        {
            healthManager = FindObjectOfType<HealthManager>();
        }
        if (!playerMovement){
            playerMovement = FindAnyObjectByType<PlayerMovement>();
        }
        
        fireBallShooter = GetComponent<FireBallShooter>();
    }

    void Update()
    {
        
    }

    public void SetSpeed(float speed){
        playerMovement.Speed = speed;
    }
    public float GetSpeed(){
        return playerMovement.Speed;
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
    

    private void GameOver()
    {
        isGameOver = true;

        scoreManager.scoreText.gameObject.SetActive(false);

        youLoseTextTMP.SetActive(true);
        int finalScore = scoreManager.GetScore();
        youLoseTextTMP.GetComponent<TextMeshProUGUI>().text = $"You Lose! You scored: {finalScore}";

        Debug.Log("Player has died. Displaying score");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.CompareTag("Zombie"))
        {
            Debug.Log(gameObject.name);
            if ()
            {
                
            }
            TakeDamage(2);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Spider"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("BigZombie"))
        {
            TakeDamage(9);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("MonsterRoamer"))
        {
            TakeDamage(healthManager.CurrentHealth);
            Destroy(other.gameObject);
        }

    }


    public bool GetAttachedToHand()
    {
        return fireBallShooter.attachedToHand;
    }
    

    
}
