using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float shakeDuration = 0.5f;   // Duration of the shake effect
    public float shakeIntensity = 0.2f;  // Intensity of the shake effect

    private Vector3 originalPosition;     // Original position of the camera
    private float shakeTimer = 0f;        // Timer for the shake effect

    void Start()
    {
        // Save the original position of the camera
        originalPosition = transform.position;
    }

    void Update()
    {
        // Check if the shake effect is active
        if (shakeTimer > 0)
        {
            // Generate a random offset based on Perlin noise
            Vector3 randomOffset = Random.insideUnitSphere * shakeIntensity;

            // Apply the offset to the camera position
            transform.position = originalPosition + randomOffset;

            // Decrease the shake timer
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            // Reset the camera position to its original position
            transform.position = originalPosition;
        }
    }

    // Call this method to trigger the camera shake
    public void StartShake()
    {
        shakeTimer = shakeDuration;
    }
}
