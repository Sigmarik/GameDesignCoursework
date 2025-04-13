using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExecutionState
{
    public NodeSlot slot;
    public Bomb bomb;

    private NodeSlot m_nextSlot = null;
    private bool m_requestedStop = false;

    public void Run()
    {
        if (slot is null) return;

        NodeBehavior nodeBeh = slot.GetNode();
        if (nodeBeh) nodeBeh.Run(this);
        else Advance();
    }

    public void Advance()
    {
        if (m_requestedStop)
        {
            slot = null;
        }
        else
        {
            if (Finished())
            {
                return;
            }
            if (m_nextSlot is null)
            {
                slot = slot.GetNext();
            }
            else
            {
                slot = m_nextSlot;
            }
        }

        if (slot is null)
        {
            bomb.AfterExplosion();
        }

        m_nextSlot = null;

        Run();
    }

    public void SetNext(NodeSlot nextSlot)
    {
        if (nextSlot is null)
        {
            m_requestedStop = true;
        }
        m_nextSlot = nextSlot;
    }

    public bool Finished()
    {
        return slot == null;
    }
}

public class NodeBehavior : MonoBehaviour
{
    public float frequency = 1.0f;
    public int level = 1;

    private bool m_fired = false;
    private bool m_running = false;

    public string description = "[Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.]";
    public string hint = "template";
    public Color color = new Color(1.0f, 0.0f, 1.0f);

    [HideInInspector]
    public NodeSlot hostingSlot = null;

    public void Run(ExecutionState state)
    {
        if (IsSingleUse() && m_fired)
        {
            StartCoroutine(PerformEmpty(state));
        }
        else
        {
            StartCoroutine(Perform(state));
        }
    }

    public bool IsRunning()
    {
        return m_running;
    }

    public bool IsAlive()
    {
        return !IsSingleUse() || !m_fired;
    }

    protected virtual bool IsSingleUse()
    {
        return false;
    }

    private IEnumerator PerformEmpty(ExecutionState state)
    {
        m_running = true;

        yield return new WaitForSeconds(0.3f);

        m_running = false;
        m_fired = true;

        state.Advance();
    }

    private IEnumerator Perform(ExecutionState state)
    {
        m_running = true;

        yield return new WaitForSeconds(0.5f);

        PerformActions(state);

        m_running = false;
        m_fired = true;

        yield return new WaitForSeconds(0.5f);

        state.Advance();
    }

    protected virtual void PerformActions(ExecutionState state)
    {

    }
}
