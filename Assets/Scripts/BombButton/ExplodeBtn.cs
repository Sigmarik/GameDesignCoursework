using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeBtn : BombBtn
{
    protected override void Start()
    {
        base.Start();
        m_bomb = bomb.GetComponent<Bomb>();
    }

    public override void OnPushed()
    {
        m_bomb.Explode();
        Disable();
    }
}
