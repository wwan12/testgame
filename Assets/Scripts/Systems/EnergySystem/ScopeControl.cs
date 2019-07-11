using Circuits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeControl : MonoBehaviour
{
    public float connectionScope;

    public MultiDependancyNode gate;
    // Start is called before the first frame update
    void Start()
    {
        CircleCollider2D connectionCollider = gameObject.AddComponent<CircleCollider2D>();
        //  connectionCollider.name = EnergyManage.TRANSFER_NAME;
        connectionCollider.radius = connectionScope;
        connectionCollider.isTrigger = true;
        gameObject.layer = 12;//能源 检测器图层
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeRadius(float scope)
    {
        gameObject.GetComponent<CircleCollider2D>().radius = scope;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       // CircuitNode[] newNodes = new CircuitNode[nodes.nodes.Length + 1];
       // nodes.CopyTo(newNodes, 0);
       // newNodes[newNodes.Length - 1] = collision.gameObject.GetComponent<CircuitNode>();
       // nodes.nodes.Add(collision.gameObject.GetComponent<CircuitNode>());
        //nodes = newNodes;
        XorGate xgate= collision.gameObject.GetComponent<XorGate>();
        if (xgate != null)
        {
            xgate.nodes.Add(gate);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        XorGate xgate = collision.gameObject.GetComponent<XorGate>();
        if (xgate != null)
        {
            xgate.nodes.Remove(gate);
        }
        //CircuitNode[] newNodes = new CircuitNode[nodes.Length - 1];
        //nodes.CopyTo(newNodes, 0);
        //newNodes[newNodes.Length - 1] = collision.gameObject.GetComponent<CircuitNode>();
        //nodes = newNodes;
        // nodes.nodes.Remove(collision.gameObject.GetComponent<CircuitNode>());
    }
}
