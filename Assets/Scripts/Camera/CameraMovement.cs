using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 m_focused;
    private Vector3 m_looking;
    private Camera m_camera;

    public float wobbleStrength = 0.06f;

    public bool focused = true;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = GetComponent<Camera>();
        m_focused = m_camera.transform.forward;
        m_looking = (Vector3.Scale(m_focused, new Vector3(1.0f, 0.0f, 1.0f))).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = focused ? m_focused : m_looking;

        Vector3 mouseScreenPosition = Input.mousePosition;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Normalize to -1 to 1
        float normalizedX = (mouseScreenPosition.x / screenWidth) * 2 - 1;
        float normalizedY = (mouseScreenPosition.y / screenHeight) * 2 - 1;

        target.y += normalizedY * wobbleStrength;
        target.x -= normalizedX * wobbleStrength;

        target = Vector3.Lerp(target, m_camera.transform.forward, Mathf.Pow(0.01f, Time.deltaTime));
        m_camera.transform.rotation = Quaternion.LookRotation(target, Vector3.up);
    }
}
