using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSkip : NodeBehavior
{
    public int count = 1;
    private int m_times_executed = 0;
    protected override bool IsSingleUse()
    {
        return false;
    }
    protected override void PerformActions(ExecutionState state)
    {
        m_times_executed++;

        if (count < 0)
        {
            GetComponent<NodeMovement>().m_tilted = m_times_executed % 2 != 0;

            if (m_times_executed % 2 == 0) return;
        }

        NodeSlot slot = state.slot;
        if (count > 0)
        {
            for (int id = -1; id < count; ++id)
            {
                if (slot == null)
                {
                    break;
                }
                slot = !state.inverted ? slot.GetNext() : slot.GetPrev();
            }
        }
        else
        {
            for (int id = 0; id < -count; ++id)
            {
                slot = !state.inverted ? slot.GetPrevOrThis() : slot.GetNextOrThis();
            }
        }
        state.SetNext(slot);
    }
}
