
using UnityEngine;

public class TriggerFireball : MonoBehaviour
{
    public Player player;
    
    public GameObject leftHand;
    public GameObject rightHand;
    public Transform center; 

    public float triggerDistance = 1.0f;
    public float resetDistance = 0.5f;

    private bool isLeftHandReady;
    private bool isRightHandReady;

    void Update()
    {
        if (!player)
        {
            player = FindObjectOfType<Player>();
        }

        LeftHandShoot();
        RightHandShoot();
    }

    private void LeftHandShoot()
    {
        if (IsHandBeyondDistance(leftHand))
        {
            if (!isLeftHandReady)
            {
                Debug.Log("Left hand extended - Shooting Fireball!");
                player.ShootFireball();
                isLeftHandReady = true;
                //Debug.Break();
            }
            else
            {
                Debug.Log("Left hand is still extended - Waiting for reset.");
            }
        }

        if (IsHandWithinResetDistance(leftHand))
        {
            if (isLeftHandReady)
            {
                Debug.Log("Left hand reset - Ready to shoot again.");
            }
            isLeftHandReady = false;
        }
    }
    
    private void RightHandShoot()
    {
        if (IsHandBeyondDistance(rightHand))
        {
            if (!isRightHandReady)
            {
                Debug.Log("Right hand extended - Shooting Fireball!");
                player.ShootFireball();
                isRightHandReady = true;
                //Debug.Break();
                
            }
            else
            {
                Debug.Log("Right hand is still extended - Waiting for reset.");
            }
        }

        if (IsHandWithinResetDistance(rightHand))
        {
            if (isRightHandReady)
            {
                Debug.Log("Right hand reset - Ready to shoot again.");
            }
            isRightHandReady = false;
        }
    }

    private bool IsHandBeyondDistance(GameObject hand)
    {
        float distance = CalculateTriggerDistance(hand);
        Debug.Log($"{hand.name} distance from {center.name}: {distance}");
        return distance > triggerDistance;
    }

    private bool IsHandWithinResetDistance(GameObject hand)
    {
        float distance = CalculateTriggerDistance(hand);
        return distance < resetDistance;
    }

    private float CalculateTriggerDistance(GameObject hand)
    {
        return Vector2.Distance(
            new Vector2(hand.transform.position.x, hand.transform.position.z),
            new Vector2(center.position.x, center.position.z)
        );
    }
}