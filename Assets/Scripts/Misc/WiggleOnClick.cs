using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleOnClick : MonoBehaviour
{
    private Rigidbody m_rigidBody;
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        Vector3 randomDirection = Random.onUnitSphere;
        m_rigidBody.AddForce(randomDirection, ForceMode.Impulse);
    }
}
