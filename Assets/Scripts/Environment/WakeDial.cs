using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Unity.VisualScripting;
using UnityEngine.Serialization;


public class WakeDial : MonoBehaviour
{
    // public RectTransform needle; // Assign the needle image
    // public TextMeshProUGUI waketext; // Assign the speed text UI element
    public float maxWake = 10f; 
    private float currentSpeed;

    public float wakeMeter;
    public Player player;
 
    public GameObject[] monsters;


    void Start()
    {
        if (!player){
            player = FindAnyObjectByType<Player>();
        }

    }


    private void OnTriggerStay(Collider other) 
    {
        foreach (GameObject currentMonster in monsters)
        {
            

            if (wakeMeter > 0)
            {
                wakeMeter -= Time.deltaTime * 5;
            }

            if (wakeMeter > maxWake)
            {
                currentMonster.GetComponent<ZombieScript>().awake = true;

            }
            

            currentSpeed = player.GetSpeed();

            
            wakeMeter += Time.deltaTime * math.abs(currentSpeed);

            if (currentMonster.GetComponent<ZombieScript>().awake == false)
            {
                //shacks according to speed and such
                var temp = currentMonster.GetComponent<Transform>().rotation;
            
                temp.x = Mathf.Sin(Time.time * (wakeMeter * 1.0f)) * 0.01f;
                temp.z = Mathf.Sin(Time.time * (wakeMeter * 1.0f)) * 0.01f;

                currentMonster.transform.rotation = temp;
            
            }

        }

        // //update text
        // waketext.text = wakeMeter.ToString("F0") + "waking up";
        
        // // Calculate the rotation angle of the needle
        // float angle = Mathf.Lerp(-90, 90, wakeMeter / maxWake);
        // needle.localEulerAngles = new Vector3(0, 0, -angle); 
          
    }


}
