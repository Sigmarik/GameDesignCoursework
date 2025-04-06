using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NodeSlot : MonoBehaviour
{
    static GameObject sActiveSlot = null;
    static bool sTips = false;

    private Vector3 m_originalPos;
    public GameObject visual;

#nullable enable
    public GameObject? nextSlot = null;
    private NodeSlot? m_nextSlot = null;
    private NodeSlot? m_prevSlot = null;
    private NodeBehavior? m_node = null;

    public static GameObject GetActiveSlot()
    {
        return sActiveSlot;
    }

    void Start()
    {
        if (nextSlot == gameObject) nextSlot = null;
        m_originalPos = visual.transform.localPosition;
        m_nextSlot = nextSlot?.GetComponent<NodeSlot>();
        if (m_nextSlot is not null)
        {
            m_nextSlot.m_prevSlot = this;
        }
    }

    public void BindNode(NodeBehavior nodeBeh)
    {
        if (m_node is not null)
        {
            Destroy(m_node.gameObject);
            m_node = null;
        }
        m_node = nodeBeh;
    }

    public NodeBehavior? GetNode()
    {
        return m_node;
    }

    public NodeSlot? GetNext()
    {
        return m_nextSlot;
    }

    public NodeSlot GetNextOrThis()
    {
        return m_nextSlot is null ? this : m_nextSlot;
    }
    public NodeSlot? GetPrev()
    {
        return m_prevSlot;
    }

    public NodeSlot GetPrevOrThis()
    {
        return m_prevSlot is null ? this : m_prevSlot;
    }

    void OnMouseOver()
    {
        sActiveSlot = gameObject;
    }

    void OnMouseExit()
    {
        if (sActiveSlot == gameObject)
        {
            sActiveSlot = null;
        }
    }

    void Update()
    {
        if (sTips && sActiveSlot == gameObject)
        {
            visual.transform.localPosition = m_originalPos + new Vector3(0.0f, 0.0f, -0.002f);
        }
        else
        {
            visual.transform.localPosition = m_originalPos;
        }
    }

    public static void EnableTips()
    {
        sTips = true;
    }

    public static void DisableTips()
    {
        sTips = false;
    }
}
