using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LightControler : MonoBehaviour
{
    public int Index = -1;
    public float Intensity;
    public Light LightSource;

    public web WaveConroler;

    // Start is called before the first frame update
    void Start()
    {
        WaveConroler = FindObjectOfType<web>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (Index != -1)
        {

            Intensity = WaveConroler.intesaty[Index];

            if (Intensity != 0)
            {
                LightSource.intensity = Mathf.Lerp(20.0f, 150.0f, Intensity);
            }
            else 
            {
                Index = -1;
            }

        }
        
            
    }
}
