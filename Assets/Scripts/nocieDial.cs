using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Speedometer : MonoBehaviour
{
    public RectTransform needle; // Assign the needle image
    public TextMeshProUGUI speedText; // Assign the speed text UI element
    public float maxSpeed = 5f; // Max speed value for scaling
    private float currentSpeed;

    public Player Player;
    

    void Update()
    {
        // Example of speed value; replace with actual speed logic
        currentSpeed = Player.speed;


        
        // Update speed text
        speedText.text = currentSpeed.ToString("F0") + " km/h";
        
        // Calculate the rotation angle of the needle
        float angle = Mathf.Lerp(-90, 90, currentSpeed / maxSpeed);
        needle.localEulerAngles = new Vector3(0, 0, -angle);
    }
}
