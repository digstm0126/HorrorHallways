using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    [SerializeField]
    public GameObject fireBallObject;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Destroy(other.gameObject);
            Destroy(fireBallObject);
            
        }
    }
}
