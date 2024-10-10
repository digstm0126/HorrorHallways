using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderScript : MonoBehaviour
{
    public float leapSpeed;
    public float detectionRange = 10f;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= detectionRange)
            {
                Vector3 leapDirection = (player.transform.position - transform.position).normalized;
                GetComponent<Rigidbody>().AddForce(leapDirection * leapSpeed, ForceMode.VelocityChange);
            }
        }
    }
}
