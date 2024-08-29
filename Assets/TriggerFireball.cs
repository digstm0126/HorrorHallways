using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFireball : MonoBehaviour
{
    public Player player;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.ShootFireball(); 
        }
    }
}
