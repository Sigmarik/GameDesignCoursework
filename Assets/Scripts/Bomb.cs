using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
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
    public GameObject wick;
    private Wick m_wick;

    private int m_level = 1;

    public int startingPlayerHealth = 7;
    private int m_playerHealth;

    public GameObject playerHealthDisplay;
    public GameObject levelDisplay;

    public int roundLength = 3;

    public GameObject store;

    public float timePerTurn = 70.0f;
    private float m_defaultTimePerTurn = 0.0f;
    public float minTimePerTurn = 12.0f;
    public float timeDecreaseRate = 0.7f;
    private float m_remainingTime = 0.0f;

    private const int preSurvivalLevelCount = 4;
    public GameObject playerCamera;

    void UpdateLevelDisplay()
    {
        if (m_level > preSurvivalLevelCount)
        {
            levelDisplay.GetComponent<TextMeshPro>().text = "survival " + m_level.ToString();
        }
        else
        {
            levelDisplay.GetComponent<TextMeshPro>().text = m_level.ToString() + " / " + preSurvivalLevelCount;
        }
    }

    void Start()
    {
        m_defaultTimePerTurn = timePerTurn;
        m_remainingTime = timePerTurn;

        m_playerHealth = startingPlayerHealth;
        playerHealthDisplay.GetComponent<TextMeshPro>().text = m_playerHealth.ToString();
        UpdateLevelDisplay();

        m_firstSlot = firstSlot.GetComponent<NodeSlot>();
        m_camMovement = cam.GetComponent<CameraMovement>();

        m_passBtn = passBtn.GetComponent<PassBtn>();
        m_explodeBtn = explodeBtn.GetComponent<ExplodeBtn>();
        m_ogPos = m_targetPos = transform.position;
        m_ogRot = m_targetRot = transform.rotation;

        m_wick = wick.GetComponent<Wick>();
        m_wick.startingLength = roundLength;
        m_wick.Reset();

        m_evilGuy = evilGuy.GetComponent<BBEG>();

        StartCoroutine(DetectSlots());
    }

    private IEnumerator DetectSlots()
    {
        yield return new WaitForSeconds(0.01f);
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_passBtn.IsActive())
            {
                Pass();
            }
            else if (m_explodeBtn.IsActive())
            {
                Explode();
            }
        }

        transform.position = Vector3.Lerp(m_targetPos, transform.position, Mathf.Pow(0.7f, Time.deltaTime * 10.0f));

        if (NodeVisibility.sCurrentPlayer == 0 && !m_wick.Burnt())
        {
            m_remainingTime -= Time.deltaTime;
            m_wick.SetTurnRemainder(m_remainingTime / timePerTurn);
            if (m_remainingTime <= 0)
            {
                Pass();
            }
        }
    }

    public void Explode()
    {
        StartCoroutine(ActuallyExplode());
        NodeVisibility.sReveal = true;
        m_camMovement.focused = false;
        m_targetPos = explodingLocators[NodeVisibility.sCurrentPlayer].transform.position;
        m_explodeBtn.Disable();
        m_passBtn.Disable();

        store.GetComponent<NodeStore>().Clear();
    }

    public void Pass(bool preserveWick = false)
    {
        m_wick.SetTurnRemainder(1.0f);

        if (m_wick.Burnt())
        {
            Explode();
            return;
        }

        NodeVisibility.sCurrentPlayer = 1 - NodeVisibility.sCurrentPlayer;
        m_camMovement.focused = false;
        m_targetPos = passingLocator.transform.position;
        m_explodeBtn.Disable();
        m_passBtn.Disable();

        if (!preserveWick) m_wick.Burn();

        StartCoroutine(MakeAutoTurn());

        store.GetComponent<NodeStore>().Clear();
    }

    private IEnumerator MakeAutoTurn()
    {
        yield return new WaitForSeconds(1.0f);

        // if (Random.Range(0, 100) < 10)
        // {
        //     Explode();
        //     yield break;
        // }

        int pick = Random.Range(1, 4);
        m_evilGuy.StartTweakingAnimation(pick);
        for (int id = 0; id < pick; ++id)
        {
            yield return new WaitForSeconds(0.3f);

            int slotId = 0;
            int reattempt = 0;
            do
            {
                slotId = Random.Range(1, m_slots.Count);
                ++reattempt;
            } while ((m_slots[slotId].m_node is not null) && reattempt < 5);

            GameObject nodePrefab = null;
            while (nodePrefab is null) nodePrefab = NodePool.pickOne(m_level);
            GameObject node = Instantiate(nodePrefab, transform);
            NodeSlot slot = m_slots[slotId];
            node.GetComponent<NodeMovement>().BindToSlot(slot.gameObject);
        }

        yield return new WaitForSeconds(1.0f);

        ReturnToThePlayer();
    }

    private void ReturnToThePlayer()
    {
        if (m_wick.Burnt())
        {
            Explode();
            return;
        }

        m_remainingTime = timePerTurn;

        NodeVisibility.sCurrentPlayer = 1 - NodeVisibility.sCurrentPlayer;
        m_camMovement.focused = true;

        m_targetPos = m_ogPos;
        m_targetRot = m_ogRot;

        m_wick.Burn();

        if (!m_wick.Burnt()) m_passBtn.Enable();
        m_explodeBtn.Enable();

        store.GetComponent<NodeStore>().Restock(m_level, 3);
    }

    public void SwitchExplodePosition()
    {
        NodeVisibility.sCurrentPlayer = 1 - NodeVisibility.sCurrentPlayer;
        m_targetPos = explodingLocators[NodeVisibility.sCurrentPlayer].transform.position;
    }

    public void Modify()
    {
        if (!m_wick.Burnt()) m_explodeBtn.Disable();
    }

    public void DealDamage(int amount)
    {
        if (NodeVisibility.sCurrentPlayer == 1)
        {
            m_evilGuy.Hit(amount);
            if (m_evilGuy.health <= 0)
            {
                AdvanceStage();
                m_execState.SetNext(null);
            }
        }
        else
        {
            playerCamera.GetComponent<CameraMovement>().Hit(amount);
            m_playerHealth -= amount;
            playerHealthDisplay.GetComponent<TextMeshPro>().text = m_playerHealth.ToString();
            if (m_playerHealth <= 0)
            {
                ResetStage();
                m_execState.SetNext(null);
            }
        }
    }

    void AdvanceStage()
    {
        ++m_level;
        m_evilGuy.m_startingHealth += 2;
        timePerTurn = minTimePerTurn + (timePerTurn - minTimePerTurn) * timeDecreaseRate;
        ++m_wick.startingLength;
        m_evilGuy.ResetHealth();
        UpdateLevelDisplay();
    }

    void ResetStage()
    {
        m_level = 1;
        m_wick.startingLength = roundLength;
        m_playerHealth = startingPlayerHealth;
        timePerTurn = m_defaultTimePerTurn;
        m_evilGuy.ResetHealth();
        UpdateLevelDisplay();
        playerHealthDisplay.GetComponent<TextMeshPro>().text = m_playerHealth.ToString();
        Reset();
    }

    public void AfterExplosion()
    {
        StartCoroutine(ResetAfterPause());
    }

    private IEnumerator ResetAfterPause()
    {
        yield return new WaitForSeconds(1.0f);
        Reset();
    }

    static bool sQueue = false;

    public void Reset()
    {
        NodeVisibility.sReveal = false;
        m_wick.Reset();
        for (int id = 0; id < m_slots.Count; ++id)
        {
            m_slots[id].Clear();
        }

        NodeVisibility.sCurrentPlayer = 0;
        m_camMovement.focused = true;

        m_targetPos = m_ogPos;
        m_targetRot = m_ogRot;

        m_passBtn.Enable();
        m_explodeBtn.Enable();

        m_remainingTime = timePerTurn;

        sQueue = !sQueue;

        if (sQueue)
        {
            Pass(true);
        }
        else
        {
            store.GetComponent<NodeStore>().Restock(m_level, 3);
        }
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
