using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Vector4 = UnityEngine.Vector4;

public class web : MonoBehaviour
{

    public Material wavey;
    //the gameobject the wave comes from (player)

    public Light hanger;
    public GameObject sourse;
    //postion of curnt wave being trigured
    public Vector4 CurtPostion;
    //the list of intestirys for waves
    public float CurtIntesaty = 1;
    // proper timer for counting
    public float propCount;
    public float[] intesaty;
    //the list of names to refrace for each wace

    public float[] count;
    // index for sending new decay
    public int index = 0;
    // index for sending the start postion to the right wave
    public int positionIndex = 0;
    // max time a wave lasts
    public float Maxtime = 1f;
    // min time a wave lasts
    public float MinTime = 0;
    // is inside the affect arrea 
    public bool inside = false;
    // the speed the waves spawn
    public float speed = 1;
    // timer
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        //wavey = gameObject.GetComponent<Renderer>().material;
        timer =  Time.time;
        index = 0;
    }

    private void FixedUpdate() 
    {
        
        float bigcount = Mathf.InverseLerp(MinTime, Maxtime, Time.time - count[index]);
        Debug.Log(bigcount);
        if (bigcount != 1)
        {
            
            //count[index] += Time.time - count[index];
            intesaty[index] = Mathf.Lerp(0.0f, 1f, Mathf.InverseLerp(Maxtime, MinTime, Time.time - count[index]));
            Shader.SetGlobalFloat("_intesaty" + index.ToString(), intesaty[index]);

        }
        else
        {
            intesaty[index] = 0.0f;
            Shader.SetGlobalFloat("_intesaty" + index.ToString(), intesaty[index]);
        }

        index += 1;
        if (index > intesaty.Length - 1)
        {
            index = 0;
        }

        hanger.intensity = Mathf.Lerp(0.2f, 0.8f, intesaty[positionIndex - 1]);
        //wavey.SetVector("_startPoint" + positionIndex.ToString(), CurtPostion);

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void sendinfo() {

        if (inside)
        {
            float Xval = sourse.transform.position.x;
            float zval = sourse.transform.position.z;
            float Yval = sourse.transform.position.y;
            float timeLog = Time.time;
            CurtPostion = new Vector4(Xval, Yval, zval, timeLog); 
            Shader.SetGlobalVector("_startPoint" + positionIndex.ToString(), CurtPostion);
            count[positionIndex] = Time.time;

            positionIndex += 1;

            if (positionIndex > intesaty.Length - 1)
            {
                positionIndex = 0;

            }

        }
        
    

    }

    private void OnTriggerEnter(Collider other) 
    {
        
        if (other.tag == "Player")
        {

            inside = true;
            sendinfo();

        }
        
    }

    private void OnTriggerExit(Collider other) 
    {
        
        if (other.tag == "Player")
        {
            inside = false;
        }

    }

    private void OnTriggerStay(Collider other) 
    {
        if (other.tag == "Player")
        {

            if (Time.time >= timer + speed)
            {
                timer = Time.time;
                sendinfo();
            }  

        }
    }
}
