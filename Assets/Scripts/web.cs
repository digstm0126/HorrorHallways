using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class web : MonoBehaviour
{
    
    public Material wavey;
    public Vector4 postion;

    public float intesaty;
    public int count = 1;

    public bool inside = false;

    public float speed = 1;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        //wavey = gameObject.GetComponent<Renderer>().material;
        timer =  Time.time;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void sendinfo() {

        if (inside)
        {
            if (count == 1)
            {
                wavey.SetVector("_startPoint1", postion);
                wavey.SetFloat("_intesaty", intesaty);
                count = 1;
            }

            else if (count == 2)
            {
                wavey.SetVector("_startPoint2", postion);
                count += 1;
            }

            else if (count == 3)
            {
                wavey.SetVector("_startPoint3", postion);
                count += 1;
            }
            else if (count == 4)
            {
                wavey.SetVector("_startPoint4", postion);
                count = 1;
            }
        }
        
    

    }

    private void OnTriggerEnter(Collider other) 
    {
        
        if (other.tag == "Player")
        {

            inside = true;
            float Xval = other.transform.position.x;
            float zval = other.transform.position.z;
            float Yval = other.transform.position.y;
            float timeLog = Time.time;
            postion = new Vector4(Xval, Yval, zval, timeLog); 
            intesaty = 5f;
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
            
            float Xval = other.transform.position.x;
            float zval = other.transform.position.z;
            float Yval = other.transform.position.y;
            float timeLog = Time.time;
            postion = new Vector4(Xval, Yval, zval, timeLog);
            intesaty = 5f;
            
            if (Time.time >= timer + speed)
            {
                timer = Time.time;
                sendinfo();
            }  

        }
    }
}
