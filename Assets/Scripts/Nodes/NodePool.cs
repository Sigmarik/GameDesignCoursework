using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePool : MonoBehaviour
{
    public static List<GameObject> nodePrefabs = new List<GameObject>();

    public List<GameObject> nodes = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int id = 0; id < nodes.Count; ++id)
        {
            Debug.Assert(nodes[id].GetComponent<NodeBehavior>() is not null);
            nodePrefabs.Add(nodes[id]);
        }
    }

    public static GameObject pickOne(int level)
    {
        float sum = 0;
        for (int id = 0; id < nodePrefabs.Count; ++id)
        {
            GameObject node = nodePrefabs[id];
            NodeBehavior nodeBeh = node.GetComponent<NodeBehavior>();
            if (nodeBeh.level > level) continue;

            sum += nodeBeh.frequency;
        }

        if (sum == 0.0f) return null;

        float pick = Random.Range(0.0f, sum);
        float currentSum = 0.0f;
        GameObject last = null;
        for (int id = 0; id < nodePrefabs.Count; ++id)
        {
            GameObject node = nodePrefabs[id];
            NodeBehavior nodeBeh = node.GetComponent<NodeBehavior>();
            if (nodeBeh.level > level) continue;

            last = node;

            currentSum += nodeBeh.frequency;
            if (currentSum > pick)
            {
                return node;
            }
        }

        return last;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
