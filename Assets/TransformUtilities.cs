using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformUtilities 
{
    
    /// <summary>
    /// Inverts the horizontal (X and Z) components of a vector. This way if the player punches up, they still punch up
    /// just in a different direction.
    /// </summary>
    /// <param name="vector"> Vector to invert </param> 
    /// <returns> Returns inverted vector </returns> 
    public static Vector3 InvertHorizontalComponents(Vector3 vector)
    {
        return new Vector3(-vector.x, vector.y, -vector.z);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="avatarDirection"> Direction vector in the avatars space </param> 
    /// <param name="playerTransform"> Transform component of the player </param> 
    /// <param name="flattenToXZPlane"> Whether to flatten the transform </param> 
    /// <param name="invertDirection"> Whether to invert the direction </param> 
    /// <returns> Returns Vector3 direction of transformed avatar direction. </returns> 
    public static Vector3 TransformDirectionFromAvatarToPlayer(
        Vector3 avatarDirection, 
        Transform playerTransform, 
        bool flattenToXZPlane = false, 
        bool invertDirection = false)
    {
        if (flattenToXZPlane)
        {
            avatarDirection.y = 0;
        }
        
        avatarDirection = avatarDirection.normalized;
        
        Quaternion rotationToApply = playerTransform.rotation;
        if (flattenToXZPlane)
        {
            rotationToApply = Quaternion.Euler(0, playerTransform.eulerAngles.y, 0);
        }

        Vector3 alignedDirection = rotationToApply * avatarDirection;
        
        if (invertDirection)
        {
            alignedDirection = InvertHorizontalComponents(alignedDirection);
        }
        
        return alignedDirection;
        
    }
}
