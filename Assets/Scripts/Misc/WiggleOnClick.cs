using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleOnClick : MonoBehaviour
{
    private Rigidbody m_rigidBody;
    public GameObject root;
    public BBEG m_evilGuy;
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_evilGuy = root.GetComponent<BBEG>();
    }

    void OnMouseDown()
    {

    }
}
