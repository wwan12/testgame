using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// todo 考虑改用message实现消耗通知，目前是迭代
/// </summary>
public class EnergyComponent : MonoBehaviour
{
    [Header("基础属性")]
    [Tooltip("什么类型的节点")]
    public Model whatModel;
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
    [HideInInspector]
    public TransferModel transfer;
    [HideInInspector]
    public SupplyModel supply;
    [HideInInspector]
    public ConsumptionModel Consumption;
    // Start is called before the first frame update
    void Start()
    {

        transfer = gameObject.AddComponent<TransferModel>();
        transfer.connectionScope = connectionScope;

        switch (whatModel)
        {
            case Model.supply:
                supply = gameObject.AddComponent<SupplyModel>();
                transfer.isSupply = true;
                break;
            case Model.consumption:
                Consumption = gameObject.AddComponent<ConsumptionModel>();
                transfer.isUse = true;
                break;
        }
    }

    public void Build()
    {
        switch (whatModel)
        {
            case Model.supply:                            
                supply.StartSupply();
                break;
            case Model.consumption:
                transfer.CutOff(transfer, 100);
                break;
        }

       // Messenger.Broadcast<string>(EventCode.ADD_POWER_NODE,gameObject.name);
       //
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public enum Model
    {
        none,//只连接
        supply,
        consumption,
    }



}
