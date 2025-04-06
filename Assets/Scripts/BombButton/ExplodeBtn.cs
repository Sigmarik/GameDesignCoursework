using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeBtn : BombBtn
{
    public GameObject bomb;
    private Bomb m_bomb = null;

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
