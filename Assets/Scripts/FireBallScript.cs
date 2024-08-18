using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    [SerializeField] public GameObject fireBallObject;
    [SerializeField] public GameObject explosiveBarrelObject;

    private MeshRenderer barrelMesh;
    private MeshRenderer zombieMesh;
    private ParticleSystem explosionEffect;
    private ParticleSystem zombieEffect;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            zombieMesh = other.GetComponent<MeshRenderer>();
            zombieEffect = other.GetComponent<ParticleSystem>();

            zombieMesh.enabled = false;
            Destroy(fireBallObject);

            zombieEffect.Play();

            // Destroy(other.gameObject);
            // Destroy(fireBallObject);
        }

        if (other.CompareTag("ExplosiveBarrel"))
        {
            barrelMesh = other.GetComponent<MeshRenderer>();
            explosionEffect = other.GetComponent<ParticleSystem>();

            barrelMesh.enabled = false;
            Destroy(fireBallObject);

            explosionEffect.Play();
        }
    }
}
