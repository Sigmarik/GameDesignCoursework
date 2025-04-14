using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInvert : NodeBehavior
{
    protected override bool IsSingleUse()
    {
        return true;
    }

    protected override void PerformActions(ExecutionState state)
    {
        state.inverted = !state.inverted;
    }
}
