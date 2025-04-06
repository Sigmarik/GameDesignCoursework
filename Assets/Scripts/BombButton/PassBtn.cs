using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassBtn : BombBtn
{
    public override void OnPushed()
    {
        NodeVisibility.sCurrentPlayer = 1 - NodeVisibility.sCurrentPlayer;
    }
}
