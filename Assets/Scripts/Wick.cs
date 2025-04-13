using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wick : MonoBehaviour
{
    private Material[] m_materials;
    public int startingLength = 5;
    private int m_remainder;

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
        for (int id = 0; id < m_materials.Length; ++id)
        {
            m_materials[id].SetInt("_RoundDuration", length);
        }
    }

    public void SetRemainingTurns(int remainder)
    {
        m_remainder = Math.Max(remainder, 0);
        for (int id = 0; id < m_materials.Length; ++id)
        {
            m_materials[id].SetInt("_TurnsRemaining", m_remainder);
        }
    }
}
