using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wick : MonoBehaviour
{
    private Material[] m_materials = new Material[0];
    public int startingLength = 5;
    private int m_length = 0;
    private int m_remainder = 0;
    private float m_threshold = 0.0f;
    private float m_turnRemainder = 1.0f;

    void Start()
    {
        m_materials = GetComponent<MeshRenderer>().materials;
        Reset();
    }

    public void Reset()
    {
        SetLength(startingLength);
        SetRemainingTurns(startingLength);
    }

    public void Burn()
    {
        SetRemainingTurns(m_remainder - 1);
    }

    public bool Burnt()
    {
        return m_remainder == 0;
    }

    public void SetLength(int length)
    {
        m_length = length;
        for (int id = 0; id < m_materials.Length; ++id)
        {
            m_materials[id].SetInt("_RoundDuration", length);
        }
    }

    public void SetTurnRemainder(float remainder = 1.0f)
    {
        m_turnRemainder = remainder;
    }

    public void SetRemainingTurns(int remainder)
    {
        m_remainder = Math.Max(remainder, 0);
    }

    void Update()
    {
        m_threshold = Mathf.Lerp(((float)m_remainder - 1.0f + m_turnRemainder) / m_length, m_threshold, Mathf.Pow(0.7f, Time.deltaTime * 10.0f));
        for (int id = 0; id < m_materials.Length; ++id)
        {
            m_materials[id].SetFloat("_Threshold", m_threshold);
        }
    }
}
