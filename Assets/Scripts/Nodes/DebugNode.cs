using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugNode : NodeBehavior
{
    public String message = "Hello, world!";

    protected override void PerformActions(ExecutionState state)
    {
        Debug.Log(message);
    }
}
