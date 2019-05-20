using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ai : MonoBehaviour
{
    private NavMeshAgent agent;
    // Start is called before the first frame update
    [Tooltip("视野角度")]
    public int angle;


    void Start()
    {
        agent = gameObject.AddComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool InVisualField(Vector3 opv,float distance)
    {
        if ((opv - transform.position).sqrMagnitude < distance * distance)
        {
            return true;
        }
        return false;
    }

    public void RandomEvent() {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="opv"></param>
    public void GoToPosition(Vector3 opv) {
        agent.Move(opv);
    }

    private bool Find(GameObject player,float r)
    {
        Vector3 otherPos = player.gameObject.transform.position;
        Vector3 v = transform.position - otherPos;
        v.y = 0.5f; //处理一下y轴，因为判断建立在二维上
        Vector3 w = player.transform.position + transform.rotation * Vector3.forward * r - otherPos;
        w.y = 0.5f;
        if (Vector3.Angle(v, w) < angle/2)
        {
            return true;
        }
        return false;
    }


}
