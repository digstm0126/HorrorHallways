using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    [SerializeField] public GameObject fireBallPrefab;
    [SerializeField] public GameObject zombieEffectPrefab;
    [SerializeField] public GameObject spiderEffectPrefab;
    [SerializeField] private int pointsForZombie = 100;
    [SerializeField] private int pointsForSpider = 100;

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnTriggerEnter(Collider other)
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
    }
}
