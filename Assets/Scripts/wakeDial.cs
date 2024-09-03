using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Mathematics;


public class wakeDial : MonoBehaviour
{
    public RectTransform needle; // Assign the needle image
    public TextMeshProUGUI waketext; // Assign the speed text UI element
    public float maxWake = 60f; // Max speed value for scaling
    private float currentSpeed;
    public float dist;
    public float wakeMetter ;

    private float activeHear = 20;
    public GameObject Player;
 
    public GameObject monster;

    void Update()
    {

        dist = Mathf.Abs(Player.transform.position.z - monster.transform.position.z);


        currentSpeed = Player.GetComponent<Player>().speed;
        //wakeMetter += Time.deltaTime * Player.GetComponent<Rigidbody>().velocity.z;
        if (dist < activeHear){

            //if (Player.GetComponent<Rigidbody>().velocity.z > 2){
            wakeMetter += Time.deltaTime * ((activeHear - dist) * math.abs(Player.GetComponent<Rigidbody>().velocity.z));
            //}
            if (wakeMetter > 0){
                wakeMetter -= Time.deltaTime * 50;
            }
        }

        if (wakeMetter > maxWake){
           // Player.GetComponent<Player>().TakeDamage(((int)(Time.deltaTime * 1)));
           monster.GetComponent<ZombieScript>().awake = true;
        }
        

        // Player.GetComponent<Rigidbody>().velocity.z;
        

        // Update speed text
        waketext.text = wakeMetter.ToString("F0") + "waking up";
        
        // Calculate the rotation angle of the needle
        float angle = Mathf.Lerp(-90, 90, wakeMetter / maxWake);
        needle.localEulerAngles = new Vector3(0, 0, -angle);
    }




}
