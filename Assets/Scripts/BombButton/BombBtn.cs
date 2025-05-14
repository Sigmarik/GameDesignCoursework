using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BombBtn : MonoBehaviour
{
    private Vector3 m_originalPos;
    private Quaternion m_originalRotation;
    private Vector3 m_targetPos;
    private Quaternion m_targetRot;
    public float scale = 1.0f;
    public bool m_enabled = true;

    private bool m_hovered = false;
    private bool m_pushed = false;
    bool m_disabled = false;

    public GameObject bomb;
    protected Bomb m_bomb;

    protected virtual void Start()
    {
        m_originalPos = transform.localPosition;
        m_originalRotation = transform.localRotation;
        m_targetPos = m_originalPos;
        m_targetRot = m_originalRotation;
        m_bomb = bomb.GetComponent<Bomb>();
    }

    void OnMouseEnter()
    {
        m_hovered = true;
        UpdatePositions();
    }

    void OnMouseExit()
    {
        m_hovered = false;
        UpdatePositions();
    }

    void OnMouseDown()
    {
        if (!m_pushed && !m_disabled)
            OnPushed();
        m_pushed = true;
        UpdatePositions();
    }

    void OnMouseUp()
    {
        if (m_pushed && !m_disabled)
            OnReleased();
        m_pushed = false;
        UpdatePositions();
    }

    // Update is called once per frame
    void UpdatePositions()
    {
        m_targetPos = m_originalPos;

        if (m_disabled)
        {
            Quaternion rotationAroundX = Quaternion.Euler(5f, 0f, 0f);
            m_targetRot = rotationAroundX * m_originalRotation;
            // transform.localRotation = m_originalRotation;
            m_targetPos = m_originalPos + new Vector3(0.0f, 0.015f, -0.005f) * scale;
        }
        else if (m_pushed)
        {
            Quaternion rotationAroundX = Quaternion.Euler(20f, 0f, 0f);
            m_targetRot = rotationAroundX * m_originalRotation;
        }
        else if (m_hovered)
        {
            Quaternion rotationAroundX = Quaternion.Euler(10f, 0f, 0f);
            m_targetRot = rotationAroundX * m_originalRotation;
        }
        else
        {
            m_targetRot = m_originalRotation;
        }
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(m_targetPos, transform.localPosition, Mathf.Pow(0.1f, Time.deltaTime * 10.0f));
        transform.localRotation = Quaternion.Lerp(m_targetRot, transform.localRotation, Mathf.Pow(0.2f, Time.deltaTime * 10.0f));
    }

    public void Disable()
    {
        if (m_pushed)
        {
            m_pushed = false;
            OnReleased();
        }
        m_disabled = true;
        UpdatePositions();
    }

    public void Enable()
    {
        m_disabled = false;
        UpdatePositions();
    }

    public bool IsActive()
    {
        return !m_disabled;
    }

    public virtual void OnPushed()
    {
    }

    public virtual void OnReleased()
    {
    }
}
