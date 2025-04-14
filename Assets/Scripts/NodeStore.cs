using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class NodeStore : MonoBehaviour
{
    public GameObject start;
    private List<GameObject> offers = new List<GameObject>();

    void Start()
    {
        Restock(1, 3);
    }

    private IEnumerator DelayedRestock(int level, int count)
    {
        yield return new WaitForSeconds(1.0f);

        for (int id = 0; id < count; ++id)
        {
            GameObject node = Instantiate(NodePool.pickOne(level));
            offers.Add(node);
            node.transform.position = start.transform.position - start.transform.forward * id * 0.02f;
            node.transform.rotation = start.transform.rotation;
        }
    }

    public void Restock(int level, int count = 3)
    {
        Clear();

        StartCoroutine(DelayedRestock(level, count));
    }

    public void Clear()
    {
        for (int id = 0; id < offers.Count; ++id)
        {
            if (!offers[id].IsDestroyed() && offers[id].GetComponent<NodeBehavior>().hostingSlot is null)
            {
                Destroy(offers[id]);
            }
        }

        offers.Clear();
    }
}
