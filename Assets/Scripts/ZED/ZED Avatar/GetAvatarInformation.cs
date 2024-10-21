
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GetAvatarInformation : MonoBehaviour
{
    public Boolean enableLogging;
    
    
    private readonly Dictionary<string, string> bonePathCache = new Dictionary<string, string>();
    

    
    
    
    //Finding the transform of a particular body part in the avatar
    //
    //Caches a programmatically create a full path name. Then uses that path name to find the correct objects
    //transform and returns it.
    public Transform GetBodyPartTransform(String avatarBoneName)
    {
        // Checks for existing cache and uses it if possible
        
        Transform avatarBone;
        if (bonePathCache.TryGetValue(avatarBoneName, out string cachedPath))
        {
            Transform cachedTransform = transform.Find(cachedPath);
            avatarBone = cachedTransform;
            
            if (!cachedTransform)
            {
                bonePathCache.Remove(avatarBoneName);
                if (enableLogging)
                {
                    Debug.LogWarning("Cached path invalid for bone: " + avatarBoneName);
                }
            }
            else
            {
                return avatarBone;
            }
        }
        
        // if no cache, create one and use that
        
        string path = "";
        avatarBone = FindDeepChild(transform, avatarBoneName, ref path);

        if (avatarBone)
        {
            bonePathCache[avatarBoneName] = path;
            if (enableLogging)
            {
                Debug.Log("Found bone: " + avatarBoneName + " at path: " + path);
            }
            return avatarBone;
        }
        
        if (enableLogging)
        {
            Debug.LogWarning("Bone not found: " + avatarBoneName);
        }
        return null;
        
    }


    

    
    
    
    
    
    
    
    
    
    
    //helpers
    //Method to find the right child nested in the avatar, because with nesting the name isn't enough, you need
    //the path to it as well.
    private Transform FindDeepChild(Transform parent, string childName, ref string path)
    {
        foreach (Transform child in parent)
        {
            string originalPath = path;
            path = string.IsNullOrEmpty(path) ? child.name : $"{path}/{child.name}";

            if (child.name == childName)
            {
                return child;
            }

            Transform foundChild = FindDeepChild(child, childName, ref path);
            if (foundChild)
            {
                return foundChild;
            }

            path = originalPath;
        }

        return null;
    }
    
    
    
    
}
