using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSwap : NodeBehavior
{
    protected override void PerformActions(ExecutionState state)
    {
        state.bomb.SwitchExplodePosition();
    }
}
