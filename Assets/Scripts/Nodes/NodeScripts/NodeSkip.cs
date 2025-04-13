using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSkip : NodeBehavior
{
    public int count = 1;
    protected override bool IsSingleUse()
    {
        return true;
    }
    protected override void PerformActions(ExecutionState state)
    {
        NodeSlot slot = state.slot;
        if (count > 0)
        {
            for (int id = -1; id < count; ++id)
            {
                if (slot == null)
                {
                    break;
                }
                slot = slot.GetNext();
            }
        }
        else
        {
            for (int id = 0; id < -count; ++id)
            {
                slot = slot.GetPrevOrThis();
            }
        }
        state.SetNext(slot);
    }
}
