using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNode : NodeBehavior
{
    public int amount = 1;
    protected override void PerformActions(ExecutionState state)
    {
        state.bomb.DealDamage(amount);
    }
}
