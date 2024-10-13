using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FireballScript : MonoBehaviour
{
    [SerializeField] public GameObject fireBallPrefab;
    [SerializeField] public GameObject zombieEffectPrefab;
    [SerializeField] public GameObject spiderEffectPrefab;
    [SerializeField] private GameObject healingEffectPrefab;
    [SerializeField] private GameObject bigZombieEffectPrefab;

    //the gameobject with the wave script to send waves on fireball colision
    public Web waveController;

    [SerializeField] private int pointsForZombie = 100;
    [SerializeField] private int pointsForSpider = 100;

    [SerializeField] private LayerMask fireBallProofLayer;


    private ScoreManager scoreManager;
    private Player player;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        player = FindObjectOfType<Player>();
        waveController = FindObjectOfType<Web>();
    }

    void OnTriggerEnter(Collider other)
    {
        //sends info to wave script and then trigers a wave 
        if (other.gameObject.name != "Player" & other.gameObject.name != "Eyes" & other.gameObject.name != "InvisibleWall" & !other.gameObject.CompareTag("Breakable"))
        {
            if (other.gameObject.CompareTag("Breakable"))
            {
                Destroy(gameObject);
            }

            OnHitSendWave(other);
        }
        
        if (other.gameObject.CompareTag("Breakable"))
        {
            Destroy(gameObject);
        }

        if (HandleFireballProofCollision(other)) return;


        HandleZombieCollision(other);

        HandleBigZombieCollision(other);

        HandleSpiderCollision(other);

        HandleExplosiveBarrelCollision(other);

        HandleHealingCrateCollision(other);
    }

    private void HandleHealingCrateCollision(Collider other)
    {
        if (other.CompareTag("HealingCrate"))
        {
            player.healthManager.Heal(player.healthManager.maxHealth - player.healthManager.CurrentHealth);

            if (healingEffectPrefab != null)
            {
                Vector3 playerPosition = player.transform.position;
                Vector3 forwardDirection = player.transform.forward;
                float offsetDistance = 1.5f;

                Vector3 spawnPosition = playerPosition + forwardDirection * offsetDistance;

                Instantiate(healingEffectPrefab, spawnPosition, Quaternion.identity);
            }

            Destroy(other.gameObject);
            Destroy(gameObject);
            
        }
    }

    private void HandleExplosiveBarrelCollision(Collider other)
    {
        if (other.CompareTag("ExplosiveBarrel"))
        {
            ExplosiveBarrelScript barrelScript = other.GetComponent<ExplosiveBarrelScript>();

            barrelScript.Explode();
            
            Destroy(gameObject);
        }
    }

    private void HandleSpiderCollision(Collider other)
    {
        if (other.CompareTag("Spider"))
        {
            scoreManager.AddScore(pointsForSpider);

            Vector3 spiderPosition = other.transform.position;
            Quaternion spiderRotation = other.transform.rotation;

            Instantiate(spiderEffectPrefab, spiderPosition, spiderRotation);

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void HandleBigZombieCollision(Collider other)
    {
        if (other.CompareTag("BigZombie"))
        {
            var bigZombie = other.GetComponent<BigZombieScript>();
            if (bigZombie != null)
            {
                Vector3 bigZombiePosition = other.transform.position;
                Quaternion bigZombieRotation = other.transform.rotation;

                Instantiate(bigZombieEffectPrefab, bigZombiePosition, bigZombieRotation);

                bigZombie.TakeDamage(1);
                Destroy(gameObject);
            }
        }
    }

    private void HandleZombieCollision(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            scoreManager.AddScore(pointsForZombie);


            Vector3 zombiePosition = other.transform.position;
            Quaternion zombieRotation = other.transform.rotation;

            Instantiate(zombieEffectPrefab, zombiePosition, zombieRotation);

            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private bool HandleFireballProofCollision(Collider other)
    {
        if ((fireBallProofLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (other.GetComponent<ZombieScript>().awake)
            {
                Destroy(gameObject);
                return true;
            }
            
        }

        return false;
    }

    private void OnHitSendWave(Collider other)
    {
        Vector3 vec = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        waveController.source = vec;
        int positionIndex = waveController.positionIndex;
        waveController.SendInfo();

        if (other.gameObject.CompareTag("light"))
        {
            LightController controller = other.GetComponent<LightController>();
            controller.index = positionIndex;
            Destroy(gameObject);
        }
    }
    
    
    
    
    
    
}
