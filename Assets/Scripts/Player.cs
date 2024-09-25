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

    public GameObject leftShoulder;
    public GameObject rightShoulder;
    
    


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
            Vector3 armForwardVector;
            
            armForwardVector = getAvatarInformation.GetRightArmForwardVector(); 
            armForwardVector.y = 0;
            armForwardVector = armForwardVector.normalized;

            Quaternion playerRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            alignedDirectionRight = playerRotation * armForwardVector;
            alignedDirectionRight = -alignedDirectionRight;

            //Vector3 positionOffset = transform.position - getDirection.rightShoulder.transform.position ;
            //Vector3 rotatedOffset = playerRotation * positionOffset;
            //rotatedOffset += new Vector3(0, 0.2f, 0.5f);

            Vector3 startPoint = rightShoulder.transform.position;

            Vector3 endPoint = startPoint + alignedDirectionRight * 10;

            Debug.DrawLine(startPoint, endPoint, Color.red, 0f, false);

            Vector3 armForwardVectorLeft;
            
            armForwardVectorLeft = getAvatarInformation.GetLeftArmForwardVector(); 
            armForwardVectorLeft.y = 0;
            armForwardVectorLeft = armForwardVectorLeft.normalized;

            Quaternion playerRotationLeft = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            alignedDirectionLeft = playerRotationLeft * armForwardVectorLeft;
            alignedDirectionLeft = -alignedDirectionLeft;

            Vector3 positionOffsetLeft = transform.position - getAvatarInformation.leftShoulder.transform.position;
            Vector3 rotatedOffsetLeft = playerRotation * positionOffsetLeft;
            rotatedOffsetLeft += new Vector3(0, 0.2f, 0.5f);

            Vector3 startPointLeft = leftShoulder.transform.position;
            Vector3 endPointLeft = startPoint + alignedDirectionLeft * 10;

            Debug.DrawLine(startPointLeft, endPointLeft, Color.green, 0f, false);

            

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
            /*
            // can be called in TriggerFireball.cs on the avatar prefab
            Quaternion playerRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            
            //rotatedOffset += new Vector3(0, 0.2f, 0.5f);
            Rigidbody projectile;
            if (!isLeft){
                projectile = Instantiate(fireballPrefab, rightShoulder.transform.position, transform.rotation);
            } else {
                projectile = Instantiate(fireballPrefab, leftShoulder.transform.position, transform.rotation);
            }

            Vector3 armForwardVector;

            if (!isLeft) {
                armForwardVector = getAvatarInformation.GetRightArmForwardVector();
            } else {
                armForwardVector = getAvatarInformation.GetLeftArmForwardVector();
            }

            armForwardVector.y = 0;
            armForwardVector = armForwardVector.normalized;

            Vector3 alignedDirection = playerRotation * armForwardVector;
            alignedDirection = -alignedDirection;

            
            projectile.velocity = alignedDirection * fireBallSpeed;
            */
            Rigidbody projectile;
            Vector3 spawnPosition;
            Vector3 armForwardVector;

            if (!isLeft)
            {
                spawnPosition = rightShoulder.transform.position;
                armForwardVector = getAvatarInformation.GetRightArmForwardVector();
            }
            else
            {
                spawnPosition = leftShoulder.transform.position;
                armForwardVector = getAvatarInformation.GetLeftArmForwardVector();
            }

            Vector3 alignedDirection = TransformUtilities.TransformDirectionFromAvatarToPlayer(
                armForwardVector, 
                transform, 
                flattenToXZPlane: true, 
                invertDirection: true
            );

            projectile = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);
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
