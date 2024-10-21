using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TakeDamageScript : MonoBehaviour
{
    public float intensity = 0;

    private Volume _volume;
    private Vignette _vignette;

    void Start()
    {
        _volume = GetComponent<Volume>();

        if (_volume.profile.TryGet<Vignette>(out _vignette))
        {
            _vignette.active = false;
        }
        else
        {
            Debug.LogError("Error, vignette not found in volume profile");
        }
    }

    public IEnumerator TakeDamageEffect()
    {
        intensity = 0.4f;

        _vignette.active = true;
        _vignette.intensity.value = 0.4f;

        yield return new WaitForSeconds(0.4f);

        while (intensity > 0)
        {
            intensity -= 0.01f;

            if (intensity < 0) intensity = 0;

            _vignette.intensity.value = intensity;

            yield return new WaitForSeconds(0.1f);
        }

        _vignette.active = false;
    }
}
