using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeVisibility : MonoBehaviour
{
    public GameObject hidden;
    public GameObject visible;
    public GameObject description;
    public GameObject hint;

    private TextMeshPro m_hiddenText;
    private TextMeshPro m_visibleText;
    private TextMeshPro m_descriptorText;
    private TextMeshPro m_hintText;

    public static int sCurrentPlayer = 0;
    public static bool sReveal = false;
    private int m_owningPlayer = 0;
    private NodeBehavior m_nodeBeh;

    void FillDependencies()
    {
        m_hiddenText = hidden.GetComponent<TextMeshPro>();
        m_visibleText = visible.GetComponent<TextMeshPro>();

        m_nodeBeh = GetComponent<NodeBehavior>();

        m_descriptorText = description.GetComponent<TextMeshPro>();
        m_hintText = hint.GetComponent<TextMeshPro>();
    }

    void Start()
    {
        FillDependencies();
        m_owningPlayer = sCurrentPlayer;
        description.SetActive(false);
        m_descriptorText.SetText(m_nodeBeh.GetDescription());
        UpdateColors();
    }

    void OnValidate()
    {
        FillDependencies();
        m_descriptorText.SetText(m_nodeBeh.GetDescription());
        UpdateColors();
    }

    void UpdateColors()
    {
        m_hintText.SetText(m_nodeBeh.GetHint());
        m_hintText.color = m_nodeBeh.GetColor();

        m_hiddenText.color = m_nodeBeh.GetColor();
        m_visibleText.color = m_nodeBeh.GetColor();
    }

    void OnMouseEnter()
    {
        if (m_owningPlayer == sCurrentPlayer || sReveal)
            description.SetActive(true);
        else
            hint.SetActive(true);
    }

    void OnMouseExit()
    {
        description.SetActive(false);
        hint.SetActive(false);
    }

    void Update()
    {
        if (m_owningPlayer == sCurrentPlayer || sReveal)
        {
            m_hiddenText.enabled = false;
            m_visibleText.enabled = true;
            hint.SetActive(false);
        }
        else
        {
            m_hiddenText.enabled = true;
            m_visibleText.enabled = false;
            description.SetActive(false);
        }
    }
}
