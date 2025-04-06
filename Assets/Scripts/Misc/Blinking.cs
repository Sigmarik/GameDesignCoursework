using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinking : MonoBehaviour
{
    private Renderer objectRenderer;
    private float blinkInterval = 0.5f;
    private float nextBlinkTime;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        nextBlinkTime = Time.time + blinkInterval;
    }

    void Update()
    {
        if (Time.time >= nextBlinkTime)
        {
            // Toggle the visibility
            objectRenderer.enabled = !objectRenderer.enabled;

            // Schedule the next blink
            nextBlinkTime += blinkInterval;
        }
    }
}
