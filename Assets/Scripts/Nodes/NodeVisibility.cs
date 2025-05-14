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

    private NodeMovement m_nodeMovement;

    void FillDependencies()
    {
        m_hiddenText = hidden.GetComponent<TextMeshPro>();
        m_visibleText = visible.GetComponent<TextMeshPro>();

        m_nodeBeh = GetComponent<NodeBehavior>();
        m_nodeMovement = GetComponent<NodeMovement>();

        m_descriptorText = description.GetComponent<TextMeshPro>();
        m_hintText = hint.GetComponent<TextMeshPro>();
    }

    void Start()
    {
        FillDependencies();
        m_owningPlayer = sCurrentPlayer;
        description.SetActive(false);
        StartCoroutine(UpdateInitialTextVisibility());
        hint.SetActive(false);
        m_descriptorText.SetText(m_nodeBeh.description);
        UpdateColors();
    }

    IEnumerator UpdateInitialTextVisibility()
    {
        yield return new WaitForEndOfFrame();

        description.SetActive(m_owningPlayer == sCurrentPlayer || sReveal);
    }

    void OnValidate()
    {
        FillDependencies();
        m_descriptorText.SetText(m_nodeBeh.description);
        description.SetActive(false);
        UpdateColors();
    }

    void UpdateColors()
    {
        m_hintText.SetText(m_nodeBeh.hint);
        m_hintText.color = m_nodeBeh.color;

        m_hiddenText.color = m_nodeBeh.color;
        m_visibleText.color = m_nodeBeh.color;
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
        if (m_nodeBeh.hostingSlot is not null || m_nodeMovement.isDragging)
        {
            description.SetActive(false);
        }

        hint.SetActive(false);
    }

    void Update()
    {
        if (m_owningPlayer == 0 || sReveal)
        {
            hidden.SetActive(false);
            visible.SetActive(true);
            hint.SetActive(false);
        }
        else
        {
            hidden.SetActive(true);
            visible.SetActive(false);
            description.SetActive(false);
        }
    }
}
