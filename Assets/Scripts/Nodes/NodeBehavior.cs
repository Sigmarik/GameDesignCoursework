using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutionState
{
    public NodeSlot slot;

    private NodeSlot m_nextSlot = null;

    public void Run()
    {
        if (slot is null) return;

        NodeBehavior nodeBeh = slot.GetNode();
        if (nodeBeh) nodeBeh.Run(this);
        else Advance();
    }

    public void Advance()
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

        m_nextSlot = null;

        Run();
    }

    public void SetNext(NodeSlot nextSlot)
    {
        m_nextSlot = nextSlot;
    }

    public bool Finished()
    {
        return slot == null;
    }
}

public class NodeBehavior : MonoBehaviour
{
    private bool m_fired = false;
    public bool m_running = false;

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
        return true;
    }

    public virtual string GetDescription()
    {
        return "[Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.]";
    }

    public virtual string GetHint()
    {
        return "template";
    }

    public virtual Color GetColor()
    {
        return new Color(1.0f, 0.0f, 1.0f);  // 1.0-s and 0.4-s work best
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
