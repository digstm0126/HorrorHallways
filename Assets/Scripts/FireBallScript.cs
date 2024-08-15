using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    [SerializeField] public GameObject fireBallObject;

    [SerializeField] public GameObject explosiveBarrelObject;

    [SerializeField] private MeshRenderer barrelMesh;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Destroy(other.gameObject);
            Destroy(fireBallObject);

        }

        if (other.CompareTag("ExplosiveBarrel"))
        {
            barrelMesh = other.GetComponent<MeshRenderer>();
            barrelMesh.enabled = false;
        }
    }


}
