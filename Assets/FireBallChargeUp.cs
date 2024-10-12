using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallChargeUp : MonoBehaviour
{

    public float chargeUpDuration;
    public float cancelChargeDuration;
    public float maxScale;

    public bool isScalingUp;

    public event Action OnFullyCharged;
    
    private Coroutine chargeUpCoroutine;


    public void StartChargeUp()
    {
        if (chargeUpCoroutine != null)
        {
            StopCoroutine(chargeUpCoroutine);
        }
        
        
        chargeUpCoroutine = StartCoroutine(ScaleOverTime(0f, maxScale, chargeUpDuration));
        isScalingUp = true;
            
        
    }

    public void CancelChargeUp()
    {
        if (isScalingUp)
        {
            if (chargeUpCoroutine != null)
            {
                StopCoroutine(chargeUpCoroutine);
            }
            
            chargeUpCoroutine = StartCoroutine(ScaleOverTime(transform.localScale.x, 0f, cancelChargeDuration));
            isScalingUp = false;
        }
        
        
    }
    
    

    private IEnumerator ScaleOverTime(float startScale, float endScale, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float newScale = Mathf.Lerp(startScale, endScale, elapsedTime / duration);
            transform.localScale = new Vector3(newScale, newScale, newScale);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.localScale = new Vector3(endScale, endScale, endScale);

        if (endScale == maxScale && OnFullyCharged != null)
        {
            OnFullyCharged.Invoke();
        }

        if (endScale == 0f)
        {
            Destroy(gameObject);
        }
        
    }
}
