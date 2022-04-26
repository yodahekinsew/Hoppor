using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    private float shakeMangitude;
    private float shakeDamping;
    private float shakeDuration;
    private Vector3 initialPosition;
    private bool shaking;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (!shaking) return;

        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMangitude;
            shakeDuration -= Time.deltaTime * shakeDamping;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
            shaking = false;
        }
    }

    public void TriggerShake(float duration, float magnitude, float damping = 1.0f)
    {
        shakeDuration = duration;
        shakeMangitude = magnitude;
        shakeDamping = damping;
        shaking = true;
    }
}
