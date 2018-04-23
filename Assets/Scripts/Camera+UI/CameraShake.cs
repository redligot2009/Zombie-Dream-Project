using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float elapsed = 0.0f;
    float oldDuration = 0f;
    public void ShakeCamera(float magnitude, float duration)
    {
        if (elapsed >= oldDuration)
        {
            StartCoroutine(Shake(magnitude, duration));
        }
    }

    IEnumerator Shake(float magnitude, float duration)
    {
        elapsed = 0.0f;
        oldDuration = duration;
        Vector3 originalCamPos = transform.localPosition;

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;

            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= magnitude * damper;
            y *= magnitude * damper;

            transform.localPosition += new Vector3(x, y, 0);

            yield return null;
        }

    }
}