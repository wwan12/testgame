using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cable : MonoBehaviour
{
    private Power source;
    private GameObject[] output;
    /// <summary>
    /// 总电流
    /// </summary>
    private float totalCurrent;
    /// <summary>
    /// 剩余电流
    /// </summary>
    private float residualCurrent;
    /// <summary>
    /// 连接到电源端
    /// </summary>
    /// <param name="p"></param>
    public void SendConnect(Power p) {
        source = p;
    }
    /// <summary>
    /// 接连到用电器端
    /// </summary>
    public void Receive() {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
