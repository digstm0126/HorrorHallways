using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LightControler : MonoBehaviour
{
    public int Index;
    public float Intensity;
    public GameObject LightSource;

    public web WaveConroler;

    // Start is called before the first frame update
    void Start()
    {
        WaveConroler = FindObjectOfType<web>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
