using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Unity.VisualScripting;


public class wakeDial : MonoBehaviour
{
    // public RectTransform needle; // Assign the needle image
    // public TextMeshProUGUI waketext; // Assign the speed text UI element
    public float maxWake = 10f; 
    private float currentSpeed;

    public float wakeMetter;
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
        foreach (GameObject curntmonster in monsters)
        {
            

            if (wakeMetter > 0)
            {
                wakeMetter -= Time.deltaTime * 5;
            }

            if (wakeMetter > maxWake)
            {
            curntmonster.GetComponent<ZombieScript>().awake = true;

            }
            

            currentSpeed = player.GetSpeed();

            
            wakeMetter += Time.deltaTime * math.abs(currentSpeed);

            if (curntmonster.GetComponent<ZombieScript>().awake == false)
            {
                //shacks acroding to speed and such
                var temp = curntmonster.GetComponent<Transform>().rotation;
            
                temp.x = Mathf.Sin(Time.time * (wakeMetter * 1.0f)) * 0.01f;
                temp.z = Mathf.Sin(Time.time * (wakeMetter * 1.0f)) * 0.01f;

                curntmonster.transform.rotation = temp;
            
            }

        }

        // //update text
        // waketext.text = wakeMetter.ToString("F0") + "waking up";
        
        // // Calculate the rotation angle of the needle
        // float angle = Mathf.Lerp(-90, 90, wakeMetter / maxWake);
        // needle.localEulerAngles = new Vector3(0, 0, -angle); 
          
    }


}
