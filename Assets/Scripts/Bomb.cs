using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private NodeSlot m_firstSlot;
    public GameObject firstSlot;
    private ExecutionState m_execState;

    void Start()
    {
        m_firstSlot = firstSlot.GetComponent<NodeSlot>();
    }

    public void Explode()
    {
        StartCoroutine(ActuallyExplode());
        NodeVisibility.sReveal = true;
    }

    public IEnumerator ActuallyExplode()
    {
        yield return new WaitForSeconds(1.0f);

        m_execState = new ExecutionState();
        m_execState.slot = m_firstSlot;
        m_execState.Run();
    }
}
