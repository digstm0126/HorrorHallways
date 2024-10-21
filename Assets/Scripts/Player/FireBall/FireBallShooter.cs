using System.Collections;
using UnityEngine;

public class FireBallShooter : MonoBehaviour
{

    public Rigidbody fireBallPrefab;

    private float fireBallSpeed = 10f;

    private bool canShoot = true;
    public float fireBallCooldownDuration = 0.4f;
    
    private GetAvatarInformation getAvatarInformation;
    
    
    
    public bool debugLogs;

    public bool attachedToHand;
    private bool isLeftHand;

    private Rigidbody currentProjectile;
    


    // Update is called once per frame
    void Update()
    {
        
        if (!getAvatarInformation)
        {
            getAvatarInformation = FindObjectOfType<GetAvatarInformation>();
        }
        
        if (Input.GetKey(KeyCode.Z) && canShoot)
        {
            Log("is the input even detected");
            ShootFireball(true);
        }
        
        if (getAvatarInformation)
        {
            //right arm debug lines
            {
                Vector3 armForwardVectorRight = getAvatarInformation.GetBodyPartTransform("Character1_RightForeArm").right;

                Vector3 transformDirectionFromAvatarToPlayer = TransformUtilities.TransformDirectionFromAvatarToPlayer(
                    armForwardVectorRight,
                    transform,
                    flattenToXZPlane: false,
                    invertDirection: true
                );

                Vector3 avatarRightShoulderPosition = getAvatarInformation.GetBodyPartTransform("Character1_RightHandMiddle1").position;

                Vector3 startPointRight = TransformUtilities.TransformPositionFromAvatarToPlayer(
                    avatarRightShoulderPosition,
                    getAvatarInformation.GetBodyPartTransform("Character1_Hips"),
                    transform
                );

                Vector3 endPointRight = startPointRight + transformDirectionFromAvatarToPlayer * 10;

                Debug.DrawLine(startPointRight, endPointRight, Color.red, 0f, false);
            }

            // Left Arm Debug Line
            {
                Vector3 armForwardVectorLeft = -getAvatarInformation.GetBodyPartTransform("Character1_LeftForeArm").right;

                Vector3 transformDirectionFromAvatarToPlayer = TransformUtilities.TransformDirectionFromAvatarToPlayer(
                    armForwardVectorLeft,
                    transform,
                    flattenToXZPlane: false,
                    invertDirection: true
                );

                Vector3 avatarLeftShoulderPosition = getAvatarInformation.GetBodyPartTransform("Character1_LeftHandMiddle1").position;

                Vector3 startPointLeft = TransformUtilities.TransformPositionFromAvatarToPlayer(
                    avatarLeftShoulderPosition,
                    getAvatarInformation.GetBodyPartTransform("Character1_Hips"),
                    transform
                );

                Vector3 endPointLeft = startPointLeft + transformDirectionFromAvatarToPlayer * 10;

                Debug.DrawLine(startPointLeft, endPointLeft, Color.green, 0f, false);
            }

            

        }
        
        TrackFireBallToHand();
    }
    
    
    public FireBallChargeUp ShootFireball(bool isLeft)
    {
        Log("ShootFireball has been called at least");
        if (!getAvatarInformation)
        {
            var projectile = Instantiate(fireBallPrefab, transform.position, transform.rotation);
            FireBallChargeUp fireBallChargeUp = projectile.GetComponent<FireBallChargeUp>();
            Log("event subscription reached not ZED");
            fireBallChargeUp.OnFullyCharged += () =>
            {
                Log("event call reached not ZED");
                projectile.velocity = transform.forward * fireBallSpeed;
            };
            fireBallChargeUp.StartChargeUp();
            StartCoroutine(FireballCooldown()); 
            return fireBallChargeUp;
        }
        else
        {
            Vector3 spawnPosition;
            Vector3 armForwardVector;

            isLeftHand = isLeft;

            if (!isLeft)
            {
                spawnPosition = TransformUtilities.TransformPositionFromAvatarToPlayer(
                    getAvatarInformation.GetBodyPartTransform("Character1_RightHandMiddle1").position,
                    getAvatarInformation.GetBodyPartTransform("Character1_Hips"),
                    transform);
            }   
            else
            {
                spawnPosition = TransformUtilities.TransformPositionFromAvatarToPlayer(
                    getAvatarInformation.GetBodyPartTransform("Character1_LeftHandMiddle1").position,
                    getAvatarInformation.GetBodyPartTransform("Character1_Hips"),
                    transform);

            }

            var projectile = Instantiate(fireBallPrefab, spawnPosition, Quaternion.identity);
            currentProjectile = projectile;
            attachedToHand = true;

            FireBallChargeUp fireBallChargeUp = projectile.GetComponent<FireBallChargeUp>();
            Log("reached event subscription");
            
            
            fireBallChargeUp.OnFullyCharged += () =>
            {

                if (!isLeft)
                {
                    armForwardVector = getAvatarInformation.GetBodyPartTransform("Character1_RightForeArm").right;

                }
                else
                {
                    armForwardVector = -getAvatarInformation.GetBodyPartTransform("Character1_LeftForeArm").right;

                }
                Vector3 alignedDirection = TransformUtilities.TransformDirectionFromAvatarToPlayer(
                    armForwardVector, 
                    transform, 
                    flattenToXZPlane: false, 
                    invertDirection: true
                );
                Log("reached event call");
                attachedToHand = false;
                projectile.transform.rotation = Quaternion.LookRotation(alignedDirection);
                projectile.velocity = alignedDirection * fireBallSpeed;

            };
            
            Log("Called StartChargeUp from FireBallShooter");
            fireBallChargeUp.StartChargeUp();
            return fireBallChargeUp;


        }



    }
    
    private IEnumerator FireballCooldown()
    {
        canShoot = false;

        yield return new WaitForSeconds(fireBallCooldownDuration);

        canShoot = true;
    }



    private void TrackFireBallToHand()
    {
        if (attachedToHand && currentProjectile)
        {
            if (isLeftHand){
                currentProjectile.position = TransformUtilities.TransformPositionFromAvatarToPlayer(
                    getAvatarInformation.GetBodyPartTransform("Character1_LeftHandMiddle1").position,
                    getAvatarInformation.GetBodyPartTransform("Character1_Hips"),
                    transform
                    );
            } else if (!isLeftHand)
            {
                currentProjectile.position = TransformUtilities.TransformPositionFromAvatarToPlayer(
                    getAvatarInformation.GetBodyPartTransform("Character1_RightHandMiddle1").position,
                    getAvatarInformation.GetBodyPartTransform("Character1_Hips"),
                    transform
                );
            }
        }
    }

    void Log(string message)
    {
        if (debugLogs)
        {
            Debug.Log(message);
        }
    }
}
