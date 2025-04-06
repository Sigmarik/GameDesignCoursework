using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private NodeSlot m_firstSlot;
    public GameObject firstSlot;
    private ExecutionState m_execState;

    public GameObject cam;
    public CameraMovement m_camMovement;

    public GameObject passBtn;
    public GameObject explodeBtn;

    private PassBtn m_passBtn;
    private ExplodeBtn m_explodeBtn;

    void Start()
    {
        m_firstSlot = firstSlot.GetComponent<NodeSlot>();
        m_camMovement = cam.GetComponent<CameraMovement>();

        m_passBtn = passBtn.GetComponent<PassBtn>();
        m_explodeBtn = passBtn.GetComponent<ExplodeBtn>();
    }

    public void Explode()
    {
        StartCoroutine(ActuallyExplode());
        NodeVisibility.sReveal = true;
    }

    public void Pass()
    {
        NodeVisibility.sCurrentPlayer = 1 - NodeVisibility.sCurrentPlayer;
        m_camMovement.focused = false;
    }

    public void Modify()
    {

    }

    public IEnumerator ActuallyExplode()
    {
        yield return new WaitForSeconds(1.0f);

        m_execState = new ExecutionState();
        m_execState.slot = m_firstSlot;
        m_execState.Run();
    }
}
