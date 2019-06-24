using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyComponent : MonoBehaviour
{
    [Header("基础属性")]
    [Tooltip("是否是发电机")]
    public bool isAlternator;
    [Tooltip("输出功率")]
    public int outputPower;
    [Tooltip("额定功率")]
    public int ratedPower;
    [Tooltip("连接范围")]
    public float connectionScope = 1f;
    /// <summary>
    /// 现在消耗的功率
    /// </summary>
    private float powerConsumption;
    // Start is called before the first frame update
    void Start()
    {
        if (isAlternator)
        {
           TransferModel transfer= gameObject.AddComponent<TransferModel>();
           transfer.connectionScope = connectionScope;
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }


    public enum Model
    {

    }
}
