using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        var originalPosition = transform.localPosition;

        var elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.localPosition = new Vector3(
                Random.Range(-1f, 1f) * magnitude,
                Random.Range(-1f, 1f) * magnitude,
                originalPosition.z
            );

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
