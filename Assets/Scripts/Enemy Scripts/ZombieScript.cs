using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;

public class ZombieScript : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float detectionRange = 10f;
    public float lightMax = 50;
    public bool awake = true;

    public SphereCollider range;

    public GameObject currentLight;

    public GameObject webController;
    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        //range.radius = detectionRange;
        currentLight = GameObject.FindObjectOfType<Web>().defaultLight;
        
    }

    void Update()
    {


        
        float distanceToTarget = Vector3.Distance(transform.position, currentLight.transform.position);

        if (currentLight.GetComponentInChildren<Light>().intensity > lightMax)
        {
            MoveTowardsPlayer();
        }


        // if (distanceToTarget <= detectionRange && awake && webs != null)
        // {
        //     webs.SetActive(false);
        //     MoveTowardsPlayer();

        // }

        // else if (distanceToTarget <= detectionRange && awake)
        // {
        //     MoveTowardsPlayer();
        // }



    }

    private void OnTriggerStay(Collider other) 
    {
        
    
        
        if (other.CompareTag("light"))
        {

            Light spotlight = other.gameObject.GetComponentInChildren<Light>();

            if (spotlight.intensity > currentLight.GetComponentInChildren<Light>().intensity)
            {
                Debug.Log("new light");
                currentLight = other.gameObject;
            }

        }

    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (currentLight.transform.position - transform.position).normalized;
        direction.y = 0;

        transform.position += direction * moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
        }
    }
}
