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
    public RectTransform needle; // Assign the needle image
    public TextMeshProUGUI waketext; // Assign the speed text UI element
    public float maxWake = 60f; // Max speed value for scaling
    private float currentSpeed;

    public float wakeMetter;

    private float activeHear = 20;
    public GameObject Player;
 
    public GameObject monster;

    void Update()
    {


    }


    private void OnTriggerStay(Collider other) 
    {

        if (wakeMetter > 0)
        {
            wakeMetter -= Time.deltaTime * 5;
        }

        if (wakeMetter > maxWake)
        {
           monster.GetComponent<ZombieScript>().awake = true;

        }
        

        currentSpeed = Player.GetComponent<Player>().speed;

        
        wakeMetter += Time.deltaTime * math.abs(currentSpeed);

        if (monster.GetComponent<ZombieScript>().awake == false)
        {
            //shacks acroding to speed and such.
            var temp = monster.GetComponent<Transform>().rotation;
        
            temp.x = Mathf.Sin(Time.time * (wakeMetter * 1.0f)) * 0.01f;
            temp.z = Mathf.Sin(Time.time * (wakeMetter * 1.0f)) * 0.01f;

            monster.transform.rotation = temp;
        
        }

        //update text
        waketext.text = wakeMetter.ToString("F0") + "waking up";
        
        // Calculate the rotation angle of the needle
        float angle = Mathf.Lerp(-90, 90, wakeMetter / maxWake);
        needle.localEulerAngles = new Vector3(0, 0, -angle); 
          
    }


}
