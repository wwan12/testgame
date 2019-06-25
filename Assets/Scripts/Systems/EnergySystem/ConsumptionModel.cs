using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumptionModel : MonoBehaviour
{
    public bool isOpen;
    public bool isConnected;
    [Tooltip("额定功率")]
    public int ratedPower;
    [Tooltip("消耗/得到的功率")]
    public int nowPower;
    [Tooltip("运行速度")]
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open()
    {
        isOpen = true;
        EnergyManage manage = GameObject.FindObjectOfType<EnergyManage>();
        if (isConnected)
        {
            nowPower = manage.PercentagePower(gameObject.name, ratedPower);
            if (nowPower == ratedPower)
            {
                speed = 1;
            }
            else
            {
                speed = nowPower / ratedPower;
            }
        }
       
    }

    public void Close()
    {
        isOpen = false;
        nowPower =0;
        speed = 0;
        GameObject.FindObjectOfType<EnergyManage>().RemoveOutput(gameObject.name, ratedPower);
    }

    /// <summary>
    /// 开始连接
    /// </summary>
    public void StartConnected()
    {
        TransferModel transfer = gameObject.GetComponent<TransferModel>();
        EnergyManage manage = GameObject.FindObjectOfType<EnergyManage>();
        transfer.isUse = true;
        if (transfer.energys.Count > 0)
        {
            string gridName = transfer.GridConnected(transfer, transfer.GetUUUID());
            if (gridName != null)
            {
                gameObject.name = gridName;
                isConnected = true;

                if (isOpen&&nowPower==0)
                {
                    nowPower= manage.Output(gameObject.name, ratedPower);
                }

            }           
        }
       
    }

    public void UpDataPower() {
        EnergyManage manage = GameObject.FindObjectOfType<EnergyManage>();
        if (isConnected)
        {
            if (isOpen)
            {
                Open();
            }
        }
        else
        {
            StartConnected();
        }
       
        
    }


}
