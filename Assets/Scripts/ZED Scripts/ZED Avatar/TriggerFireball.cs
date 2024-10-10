using UnityEngine;
using UnityEngine.Serialization;

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

    // Boolean to control debug logs
    public bool enableDebugLogs = true;

    [FormerlySerializedAs("getDirection")] public GetAvatarInformation getAvatarInformation;

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
                LogDebug("Left hand extended - Shooting Fireball!");
                player.ShootFireball(true);
                isLeftHandReady = true;
                //Debug.Break();
            }
            else
            {
                LogDebug("Left hand is still extended - Waiting for reset.");
            }
        }

        if (IsHandWithinResetDistance(leftHand))
        {
            if (isLeftHandReady)
            {
                LogDebug("Left hand reset - Ready to shoot again.");
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
                LogDebug("Right hand extended - Shooting Fireball!");
                player.ShootFireball(false);
                isRightHandReady = true;
                //Debug.Break();
                
            }
            else
            {
                LogDebug("Right hand is still extended - Waiting for reset.");
            }
        }

        if (IsHandWithinResetDistance(rightHand))
        {
            if (isRightHandReady)
            {
                LogDebug("Right hand reset - Ready to shoot again.");
            }
            isRightHandReady = false;
        }
    }

    private bool IsHandBeyondDistance(GameObject hand)
    {
        float distance = CalculateTriggerDistance(hand);
        LogDebug($"{hand.name} distance from {center.name}: {distance}");
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
    

    

    private void LogDebug(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log(message);
        }
    }
}
