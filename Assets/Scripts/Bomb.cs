using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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

    private Vector3 m_targetPos, m_ogPos;
    private Quaternion m_targetRot, m_ogRot;

    public List<GameObject> explodingLocators;
    public GameObject passingLocator;

    public GameObject evilGuy;
    private BBEG m_evilGuy;
    private List<NodeSlot> m_slots = new List<NodeSlot>();

    void Start()
    {
        m_firstSlot = firstSlot.GetComponent<NodeSlot>();
        m_camMovement = cam.GetComponent<CameraMovement>();

        m_passBtn = passBtn.GetComponent<PassBtn>();
        m_explodeBtn = explodeBtn.GetComponent<ExplodeBtn>();
        m_ogPos = m_targetPos = transform.position;
        m_ogRot = m_targetRot = transform.rotation;

        m_evilGuy = evilGuy.GetComponent<BBEG>();

        NodeSlot slot = m_firstSlot;
        while (slot is not null)
        {
            m_slots.Add(slot);
            slot.SetBomb(this);
            slot = slot.nextSlot?.GetComponent<NodeSlot>();
            if (slot?.nextSlot?.GetComponent<NodeSlot>() == slot) break;
        }
    }

    void Update()
    {
        transform.position = Vector3.Lerp(m_targetPos, transform.position, Mathf.Pow(0.7f, Time.deltaTime * 10.0f));
    }

    public void Explode()
    {
        StartCoroutine(ActuallyExplode());
        NodeVisibility.sReveal = true;
        m_camMovement.focused = false;
        m_targetPos = explodingLocators[NodeVisibility.sCurrentPlayer].transform.position;
        m_explodeBtn.Disable();
        m_passBtn.Disable();
    }

    public void Pass()
    {
        NodeVisibility.sCurrentPlayer = 1 - NodeVisibility.sCurrentPlayer;
        m_camMovement.focused = false;
        m_targetPos = passingLocator.transform.position;
        m_explodeBtn.Disable();
        m_passBtn.Disable();

        StartCoroutine(MakeAutoTurn());
    }

    private IEnumerator MakeAutoTurn()
    {
        yield return new WaitForSeconds(1.0f);

        if (Random.Range(0, 100) < 10)
        {
            Explode();
            yield break;
        }

        int pick = Random.Range(2, 3);
        m_evilGuy.StartTweakingAnimation(pick);
        for (int id = 0; id < pick; ++id)
        {
            yield return new WaitForSeconds(0.3f);

            int slotId = Random.Range(1, m_slots.Count);
            if (m_slots[slotId].m_node is not null)
            {
                slotId = Random.Range(1, m_slots.Count);
            }

            GameObject nodePrefab = NodePool.pickOne(1);
            GameObject node = Instantiate(nodePrefab, transform);
            NodeSlot slot = m_slots[slotId];
            node.transform.position = slot.transform.position;
            node.transform.rotation = slot.transform.rotation;
            node.GetComponent<NodeMovement>().BindToSlot(m_slots[slotId].gameObject);
        }

        yield return new WaitForSeconds(1.0f);

        ReturnToThePlayer();
    }

    private void ReturnToThePlayer()
    {
        NodeVisibility.sCurrentPlayer = 1 - NodeVisibility.sCurrentPlayer;
        m_camMovement.focused = true;

        m_targetPos = m_ogPos;
        m_targetRot = m_ogRot;

        m_passBtn.Enable();
        m_explodeBtn.Enable();
    }

    public void SwitchExplodePosition()
    {
        NodeVisibility.sCurrentPlayer = 1 - NodeVisibility.sCurrentPlayer;
        m_targetPos = explodingLocators[NodeVisibility.sCurrentPlayer].transform.position;
    }

    public void Modify()
    {
        m_explodeBtn.Disable();
    }

    public void DealDamage(int amount)
    {
        if (NodeVisibility.sCurrentPlayer == 1)
        {
            m_evilGuy.Hit(amount);
        }

        // TODO: Complete
    }

    public IEnumerator ActuallyExplode()
    {
        yield return new WaitForSeconds(1.0f);

        m_execState = new ExecutionState();
        m_execState.slot = m_firstSlot;
        m_execState.bomb = this;
        m_execState.Run();
    }
}
