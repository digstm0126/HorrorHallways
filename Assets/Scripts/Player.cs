using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float speed;
    public Rigidbody fireballPrefab;
    public GameObject youLoseTextTMP;

    public bool scrollControl;

    public int mouseScrollScale = 1;

    private float fireBallSpeed = 10f;
    private bool canShoot = true;
    public float fireBallCooldownDuration = 0.4f;
    private bool isGameOver = false;
    private float speeddecrease = 1f;

    public HealthManager healthManager; // Reference to HealthManager
    public TakeDamageScript takeDamageScript;

    private ScoreManager scoreManager;

    GetAvatarInformation getAvatarInformation;

    private Vector3 alignedDirectionRight;
    private Vector3 alignedDirectionLeft;

    

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        // Initialize HealthManager if not set in the Inspector
        if (!healthManager)
        {
            healthManager = FindObjectOfType<HealthManager>();
        }
    }

    void Update()
    {
        if (!getAvatarInformation)
        {
            getAvatarInformation = FindObjectOfType<GetAvatarInformation>();
        }
        

        if (Input.GetKey(KeyCode.Z) && canShoot)
        {
            ShootFireball(true);
        }
        
        if (scrollControl == true){
            speed += Input.mouseScrollDelta.y * mouseScrollScale;
            GetComponent<Rigidbody>().AddForce(transform.forward * speed);
            
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * (speed * Time.deltaTime));

                //GetComponent<Rigidbody>().AddForce(transform.forward * speed);
            }
        }

        if (getAvatarInformation)
        {
            //right arm debug lines
            {
                Vector3 armForwardVectorRight = getAvatarInformation.GetRightArmForwardVector();

                Vector3 transformDirectionFromAvatarToPlayer = TransformUtilities.TransformDirectionFromAvatarToPlayer(
                    armForwardVectorRight,
                    transform,
                    flattenToXZPlane: true,
                    invertDirection: true
                );

                Vector3 avatarRightShoulderPosition = getAvatarInformation.rightShoulder.transform.position;

                Vector3 startPointRight = TransformUtilities.TransformPositionFromAvatarToPlayer(
                    avatarRightShoulderPosition,
                    getAvatarInformation.GetBodyPartTransform("Character1_Hips"),
                    transform
                );

                Vector3 endPointRight = startPointRight + transformDirectionFromAvatarToPlayer * 10;

                Debug.DrawLine(startPointRight, endPointRight, Color.red, 0f, false);
            }

            // Left Arm Debug Line
            {
                Vector3 armForwardVectorLeft = getAvatarInformation.GetLeftArmForwardVector();

                Vector3 transformDirectionFromAvatarToPlayer = TransformUtilities.TransformDirectionFromAvatarToPlayer(
                    armForwardVectorLeft,
                    transform,
                    flattenToXZPlane: true,
                    invertDirection: true
                );

                Vector3 avatarLeftShoulderPosition = getAvatarInformation.leftShoulder.transform.position;

                Vector3 startPointLeft = TransformUtilities.TransformPositionFromAvatarToPlayer(
                    avatarLeftShoulderPosition,
                    getAvatarInformation.GetBodyPartTransform("Character1_Hips"),
                    transform
                );

                Vector3 endPointLeft = startPointLeft + transformDirectionFromAvatarToPlayer * 10;

                Debug.DrawLine(startPointLeft, endPointLeft, Color.green, 0f, false);
            }

            

        }


    }

    public IEnumerator SpeedIncrease()
    {
        speed += 1.0f;
        
        yield return new WaitForSeconds(2f);
        
        if (speed - speeddecrease <= 0.0f)
        {
            speed = 0;
        }
        else
        {
            speed -= speeddecrease;
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
        if (!getAvatarInformation)
        {
            var projectile = Instantiate(fireballPrefab, transform.position, transform.rotation);
            projectile.velocity = transform.forward * fireBallSpeed;

            StartCoroutine(FireballCooldown()); 
        }
        else
        {
            
            Rigidbody projectile;
            Vector3 spawnPosition;
            Vector3 armForwardVector;

            if (!isLeft)
            {
                spawnPosition = TransformUtilities.TransformPositionFromAvatarToPlayer(
                    getAvatarInformation.rightShoulder.transform.position,
                    getAvatarInformation.GetBodyPartTransform("Character1_Hips"),
                    transform);
                armForwardVector = getAvatarInformation.GetRightArmForwardVector();
            }   
            else
            {
                spawnPosition = TransformUtilities.TransformPositionFromAvatarToPlayer(
                    getAvatarInformation.leftShoulder.transform.position,
                    getAvatarInformation.GetBodyPartTransform("Character1_Hips"),
                    transform);

                armForwardVector = getAvatarInformation.GetLeftArmForwardVector();
            }

            Vector3 alignedDirection = TransformUtilities.TransformDirectionFromAvatarToPlayer(
                armForwardVector, 
                transform, 
                flattenToXZPlane: true, 
                invertDirection: true
            );

            projectile = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
            projectile.transform.rotation = Quaternion.LookRotation(alignedDirection);
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

        if (other.CompareTag("BigZombie"))
        {
            TakeDamage(9);
            Destroy(other.gameObject);
        }
    }
    
    
    
    
}
