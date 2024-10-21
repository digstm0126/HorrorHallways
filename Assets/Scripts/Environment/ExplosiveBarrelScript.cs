using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrelScript : MonoBehaviour
{
    [SerializeField] public GameObject explosionEffectPrefab;
    [SerializeField] public GameObject zombieEffectPrefab;
    [SerializeField] public GameObject spiderEffectPrefab;

    public float explosionRadius = 5f;
    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void Explode()
    {
        Vector3 barrelPosition = transform.position;
        Quaternion barrelRotation = transform.rotation;

        Instantiate(explosionEffectPrefab, barrelPosition, barrelRotation);

        DestroyNearbyObjects(barrelPosition);

        Destroy(gameObject);
    }

    void DestroyNearbyObjects(Vector3 explosionPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Zombie") || nearbyObject.CompareTag("Spider") || nearbyObject.CompareTag("Breakable"))
            {
                if (nearbyObject.CompareTag("Zombie"))
                {
                    Instantiate(zombieEffectPrefab, nearbyObject.transform.position, nearbyObject.transform.rotation);
                    scoreManager.AddScore(75);
                }

                if (nearbyObject.CompareTag("Spider"))
                {
                    Instantiate(zombieEffectPrefab, nearbyObject.transform.position, nearbyObject.transform.rotation);
                    scoreManager.AddScore(50);
                }

                Destroy(nearbyObject.gameObject);
            }
        }
    }
}
