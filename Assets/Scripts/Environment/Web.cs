using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Vector4 = UnityEngine.Vector4;

public class Web : MonoBehaviour
{


    //the gameobject the wave comes from (player)

    public Light hanger;

    public GameObject defaultLight; 

    public UnityEngine.Vector3 source;
    //postion of curnt wave being trigured
    public Vector4 currentPosition;
    //the list of intestirys for waves
    public float currentIntensity = 1;
    // proper timer for counting
    public float propCount;
    public float[] intensity;
    //the list of names to refrace for each wace

    public float[] count;
    // index for sending new decay
    public int index = 0;
    // index for sending the start postion to the right wave
    public int positionIndex = 0;
    // max time a wave lasts
    public float maxTime = 1f;
    // min time a wave lasts
    public float minTime = 0;
    // is inside the affect arrea 
    public bool inside = false;
    // the speed the waves spawn
    public float speed = 1;
    // timer
    float timer;

    // Start is called before the first frame update
    void Start()
    {

        timer =  Time.time;
        index = 0;
    }

    private void FixedUpdate() 
    {
        if (Time.time > maxTime)
        {
            if (index > intensity.Length - 1)
            {
                index = 0;
            }
            
            float bigCount = Mathf.InverseLerp(minTime, maxTime, Time.time - count[index]);

            if (bigCount != 1)
            {
                
                //count[index] += Time.time - count[index];
                intensity[index] = Mathf.Lerp(0.0f, 1f, Mathf.InverseLerp(maxTime, minTime, Time.time - count[index]));
                Shader.SetGlobalFloat("_intesaty" + index.ToString(), intensity[index]);

            }
            else
            {
                intensity[index] = 0.0f;
                Shader.SetGlobalFloat("_intesaty" + index.ToString(), intensity[index]);
            }

            index += 1;
            

            //hanger.intensity = Mathf.Lerp(0.2f, 0.8f, intesaty[positionIndex - 1]);

        }


    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void SendInfo() 
    {


        float xVal = source.x;
        float zVal = source.z;
        float yVal = source.y;
        float timeLog = Time.time;
        currentPosition = new Vector4(xVal, yVal, zVal, timeLog); 
        Shader.SetGlobalVector("_startPoint" + positionIndex.ToString(), currentPosition);
        count[positionIndex] = Time.time;
        intensity[positionIndex] = currentIntensity;

        positionIndex += 1;

        if (positionIndex > intensity.Length - 1)
        {
            positionIndex = 0;

        }

        
        
    

    }

    // private void OnTriggerEnter(Collider other) 
    // {
        
    //     if (other.tag == "Player")
    //     {

    //         inside = true;
    //         sendinfo();

    //     }
        
    // }

    // private void OnTriggerExit(Collider other) 
    // {
        
    //     if (other.tag == "Player")
    //     {
    //         inside = false;
    //     }

    // }

    // private void OnTriggerStay(Collider other) 
    // {
    //     if (other.tag == "Player")
    //     {

    //         if (Time.time >= timer + speed)
    //         {
    //             timer = Time.time;
    //             sendinfo();
    //         }  

    //     }
    // }
}
