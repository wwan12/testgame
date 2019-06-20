using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 倒计时销毁物体
/// </summary>
public class Lifetimer : MonoBehaviour
{
    public static Lifetimer AddTimer(GameObject onThis, float timeInSeconds, bool callDespawn)
    {
        Lifetimer omg = onThis.AddComponent<Lifetimer>();
        omg.Lifetime = timeInSeconds;
        omg.CallDespawn = callDespawn;

        return omg;
    }

    public float Lifetime;
    public bool CallDespawn;
    // TODO 'persist after use' option

    void Start()
    {
        StartCoroutine(StartLifetimer(Lifetime));
    }
    public IEnumerator StartLifetimer(float time)
    {
        yield return new WaitForSeconds(time);

        if (CallDespawn)
        {
            //gameObject.SendMessage("DeSpawn", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            Destroy(gameObject);
        }
        Destroy(this); // remove this script - important!
    }
}
