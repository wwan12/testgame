using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyModel : MonoBehaviour
{
    [Tooltip("输出功率")]
    public int outputPower;
    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSupply() {
        TransferModel transfer = gameObject.GetComponent<TransferModel>();
        EnergyManage manage = GameObject.FindObjectOfType<EnergyManage>();
        transfer.isSupply = true;
        if (transfer.energys.Count == 0)
        {
            gameObject.name = manage.GetGridName();           
        }
        else
        {
            string gridName= transfer.GridConnected(transfer, transfer.GetUUUID());
            if (gridName==null)
            {
                gameObject.name = manage.GetGridName();
            }
            else
            {
                gameObject.name = gridName;
              
            } 
        }
        manage.Input(gameObject.name, outputPower);
    }

    public void RemoveSupply()
    {
        TransferModel transfer = gameObject.GetComponent<TransferModel>();
        EnergyManage manage = GameObject.FindObjectOfType<EnergyManage>();
        transfer.isSupply = false;
        transfer.CutOff(transfer,transfer.GetUUUID());
        manage.RemoveInput(gameObject.name, outputPower);
        
    }
}
