using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassBtn : BombBtn
{
    public override void OnPushed()
    {
        m_bomb.Pass();
    }
}
