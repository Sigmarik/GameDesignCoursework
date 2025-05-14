using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    private Vector3 m_focused;
    private Vector3 m_looking;
    private Camera m_camera;

    public float wobbleStrength = 0.06f;

    public bool focused = true;
    private float m_tilt = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = GetComponent<Camera>();
        m_focused = m_camera.transform.forward;
        m_looking = (Vector3.Scale(m_focused, new Vector3(1.0f, 0.0f, 1.0f))).normalized;
    }

    public void Hit(int damage)
    {
        float angle = 0.0f;
        if (damage > 0)
        {
            angle = damage * 10.0f;
        }
        else
        {
            angle = damage * 0.5f;
        }
        m_tilt += angle;
        StartCoroutine(DelayedCorrectTilt(angle));
    }

    private IEnumerator DelayedCorrectTilt(float correction)
    {
        yield return new WaitForSeconds(0.05f);
        m_tilt -= correction;
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

        Quaternion targetRotation = Quaternion.Euler(0.0f, 0.0f, m_tilt) * Quaternion.LookRotation(target, Vector3.up);
        transform.rotation = Quaternion.Lerp(targetRotation, transform.rotation, Mathf.Pow(0.6f, Time.deltaTime * 10.0f));
    }
}
