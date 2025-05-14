using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMovement : MonoBehaviour
{
    public Camera mainCamera;

    [HideInInspector]
    public bool isDragging = false;
    public Vector3 m_restingPosition = new Vector3();
    private Collider m_collider;

    private NodeSlot m_owningSlot;

    private NodeBehavior m_nodeBeh = null;
    private Quaternion m_ogRotation;

    private static bool sHolding = false;

    private static bool sDragging = false;

    public bool m_tilted = false;

    void Start()
    {
        mainCamera = Camera.main;
        m_restingPosition = transform.localPosition;
        m_ogRotation = transform.localRotation;
        m_collider = GetComponent<Collider>();
        m_nodeBeh = GetComponent<NodeBehavior>();
    }

    void OnMouseDown()
    {
        if (m_owningSlot != null)
        {
            return;
        }

        isDragging = true;
        sDragging = true;
        NodeSlot.EnableTips();
        sHolding = true;
    }

    void OnMouseUp()
    {
        if (m_owningSlot != null)
        {
            return;
        }

        isDragging = false;
        sDragging = false;
        NodeSlot.DisableTips();
        if (NodeSlot.GetActiveSlot() != null)
        {
            BindToSlot(NodeSlot.GetActiveSlot());
        }

        sHolding = false;
    }

    public void BindToSlot(GameObject slot)
    {
        transform.SetParent(slot.transform);
        m_restingPosition = Vector3.zero;
        m_ogRotation = Quaternion.identity;
        transform.rotation = slot.transform.rotation;
        transform.position = slot.transform.position;
        m_owningSlot = slot.GetComponent<NodeSlot>();
        m_nodeBeh = GetComponent<NodeBehavior>();
        m_owningSlot.BindNode(m_nodeBeh);
        m_nodeBeh.hostingSlot = m_owningSlot;
    }

    void Update()
    {
        m_collider.enabled = !sHolding;

        if (isDragging)
        {
            DragObject();
        }
        else
        {
            if (!m_nodeBeh.IsAlive() || m_tilted)
            {
                float angle = m_nodeBeh.IsRunning() ? 40.0f : 20.0f;
                transform.localRotation = Quaternion.Euler(0.0f, angle, 0.0f) * m_ogRotation;
            }
            else
            {
                transform.localRotation = m_ogRotation;
            }

            if (sDragging && m_owningSlot is not null)
                transform.localRotation = Quaternion.Euler(0.0f, Mathf.Sin(Time.time * 40.0f) * 5.0f, 0.0f) * transform.localRotation;

            Vector3 position = m_restingPosition;
            if (m_nodeBeh.IsRunning())
            {
                position.z += 0.003f;
            }
            transform.localPosition = Vector3.Lerp(transform.localPosition, position, 0.1f);
        }
    }

    private void DragObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(mainCamera.transform.forward, m_restingPosition);

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 direction = hitPoint - mainCamera.transform.position;
            if (NodeSlot.GetActiveSlot() is null)
            {
                direction *= 0.8f;
            }
            transform.position = mainCamera.transform.position + direction;
        }
    }
}
