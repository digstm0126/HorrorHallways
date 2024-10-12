using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallShooter : MonoBehaviour
{

    public Rigidbody fireBallPrefab;

    private float fireBallSpeed = 10f;

    private bool canShoot = true;
    public float fireBallCooldownDuration = 0.4f;
    
    private GetAvatarInformation getAvatarInformation;
    
    
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!getAvatarInformation)
        {
            getAvatarInformation = FindObjectOfType<GetAvatarInformation>();
        }
        
        if (Input.GetKey(KeyCode.Z) && canShoot)
        {
            ShootFireball(true);
        }
        
        if (getAvatarInformation)
        {
            //right arm debug lines
            {
                Vector3 armForwardVectorRight = getAvatarInformation.GetRightArmForwardVector();

                Vector3 transformDirectionFromAvatarToPlayer = TransformUtilities.TransformDirectionFromAvatarToPlayer(
                    armForwardVectorRight,
                    transform,
                    flattenToXZPlane: true,
                    invertDirection: true
                );

                Vector3 avatarRightShoulderPosition = getAvatarInformation.rightShoulder.transform.position;

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
                Vector3 armForwardVectorLeft = getAvatarInformation.GetLeftArmForwardVector();

                Vector3 transformDirectionFromAvatarToPlayer = TransformUtilities.TransformDirectionFromAvatarToPlayer(
                    armForwardVectorLeft,
                    transform,
                    flattenToXZPlane: true,
                    invertDirection: true
                );

                Vector3 avatarLeftShoulderPosition = getAvatarInformation.leftShoulder.transform.position;

                Vector3 startPointLeft = TransformUtilities.TransformPositionFromAvatarToPlayer(
                    avatarLeftShoulderPosition,
                    getAvatarInformation.GetBodyPartTransform("Character1_Hips"),
                    transform
                );

                Vector3 endPointLeft = startPointLeft + transformDirectionFromAvatarToPlayer * 10;

                Debug.DrawLine(startPointLeft, endPointLeft, Color.green, 0f, false);
            }

            

        }
    }
    
    
    public FireBallChargeUp ShootFireball(bool isLeft)
    {
        if (!getAvatarInformation)
        {
            var projectile = Instantiate(fireBallPrefab, transform.position, transform.rotation);
            FireBallChargeUp fireBallChargeUp = projectile.GetComponent<FireBallChargeUp>();

            fireBallChargeUp.OnFullyCharged += () =>
            {
                projectile.velocity = transform.forward * fireBallSpeed;
            };
            
            StartCoroutine(FireballCooldown()); 
            return fireBallChargeUp;
        }
        else
        {
            
            Rigidbody projectile;
            Vector3 spawnPosition;
            Vector3 armForwardVector;

            if (!isLeft)
            {
                spawnPosition = TransformUtilities.TransformPositionFromAvatarToPlayer(
                    getAvatarInformation.rightShoulder.transform.position,
                    getAvatarInformation.GetBodyPartTransform("Character1_Hips"),
                    transform);
                armForwardVector = getAvatarInformation.GetRightArmForwardVector();
            }   
            else
            {
                spawnPosition = TransformUtilities.TransformPositionFromAvatarToPlayer(
                    getAvatarInformation.leftShoulder.transform.position,
                    getAvatarInformation.GetBodyPartTransform("Character1_Hips"),
                    transform);

                armForwardVector = getAvatarInformation.GetLeftArmForwardVector();
            }

            Vector3 alignedDirection = TransformUtilities.TransformDirectionFromAvatarToPlayer(
                armForwardVector, 
                transform, 
                flattenToXZPlane: true, 
                invertDirection: true
            );

            projectile = Instantiate(fireBallPrefab, spawnPosition, Quaternion.identity);

            FireBallChargeUp fireBallChargeUp = projectile.GetComponent<FireBallChargeUp>();

            fireBallChargeUp.OnFullyCharged += () =>
            {
                projectile.transform.rotation = Quaternion.LookRotation(alignedDirection);
                projectile.velocity = alignedDirection * fireBallSpeed;
                
            };
            
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
}
