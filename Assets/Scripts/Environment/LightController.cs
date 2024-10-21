using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

public class LightController : MonoBehaviour
{
    public int index = -1;
    public float intensity;
    public Light lightSource;

    public Web waveController;

    // Start is called before the first frame update
    void Start()
    {
        waveController = FindObjectOfType<Web>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (index != -1)
        {

            intensity = waveController.intensity[index];

            if (intensity != 0)
            {
                lightSource.intensity = Mathf.Lerp(20.0f, 150.0f, intensity);
            }
            else 
            {
                index = -1;
            }

        }
        
            
    }
}
