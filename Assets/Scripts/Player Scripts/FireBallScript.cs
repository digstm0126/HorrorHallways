using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    [SerializeField] public GameObject fireBallPrefab;
    [SerializeField] public GameObject zombieEffectPrefab;
    [SerializeField] public GameObject spiderEffectPrefab;
    [SerializeField] private GameObject healingEffectPrefab;
    [SerializeField] private GameObject bigZombieEffectPrefab;

    //the gameobject with the wave script to send waves on fireball colision
    public web WaveContolor;

    [SerializeField] private int pointsForZombie = 100;
    [SerializeField] private int pointsForSpider = 100;

    [SerializeField] private LayerMask fireBallProofLayer;


    private ScoreManager scoreManager;
    private Player player;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        player = FindObjectOfType<Player>();
        WaveContolor = FindObjectOfType<web>();
    }

    void OnTriggerEnter(Collider other)
    {
        //sends info to wave script and then trigers a wave 
        if (other.gameObject.name != "Player" & other.gameObject.name != "Eyes" & other.gameObject.name != "InvisibleWall" & other.gameObject.tag != "breakable")
        {
            

            Vector3 Vec = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            WaveContolor.sourse = Vec;
            int PositionIndex = WaveContolor.positionIndex;
            WaveContolor.sendinfo();

            if (other.gameObject.name == "light")
            {
                LightControler controler = other.GetComponent<LightControler>();
                controler.Index = PositionIndex;
                Destroy(gameObject);
            }


        }
        

        if ((fireBallProofLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (other.GetComponent<ZombieScript>().awake)
            {
                Destroy(gameObject);
                return;
            }
            
        }


        if (other.CompareTag("Zombie"))
        {
            scoreManager.AddScore(pointsForZombie);


            Vector3 zombiePosition = other.transform.position;
            Quaternion zombieRotation = other.transform.rotation;

            Instantiate(zombieEffectPrefab, zombiePosition, zombieRotation);

            Destroy(other.gameObject);
            Destroy(gameObject);
        }

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

        if (other.CompareTag("Spider"))
        {
            scoreManager.AddScore(pointsForSpider);

            Vector3 spiderPosition = other.transform.position;
            Quaternion spiderRotation = other.transform.rotation;

            Instantiate(spiderEffectPrefab, spiderPosition, spiderRotation);

            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        if (other.CompareTag("ExplosiveBarrel"))
        {
            ExplosiveBarrelScript barrelScript = other.GetComponent<ExplosiveBarrelScript>();

            barrelScript.Explode();
            
            Destroy(gameObject);
        }

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
}
