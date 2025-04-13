using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeBlank : NodeBehavior
{
    public int radius = 1;
    void Update()
    {
        if (hostingSlot is not null)
        {
            NodeSlot left = hostingSlot;
            NodeSlot right = hostingSlot;
            for (int id = 0; id < radius; ++id)
            {
                left.Clear();
                right.Clear();
                left = left.GetPrevOrThis();
                right = right.GetNextOrThis();
            }
        }
    }
}
