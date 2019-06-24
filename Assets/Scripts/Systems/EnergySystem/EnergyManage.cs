using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManage : MonoBehaviour
{
    private Dictionary<string, EnergyGrid> energyGrids;
    /// <summary>
    /// 单位为kw
    /// </summary>
    private float totalEnergy;
    private float consumeEnergy;
    public float refreshTime;
    public event EventHandler<string> powerChange;
    // Start is called before the first frame update
    void Start()
    {
        energyGrids = new Dictionary<string, EnergyGrid>();
        energyGrids.Add("主电网", new EnergyGrid());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string[] GetGridNames()
    {
        string[] keys=new string[energyGrids.Keys.Count];
        energyGrids.Keys.CopyTo(keys, 0);    
        return keys;
    }

    public void Input(string name,int power)
    {
        if (energyGrids.ContainsKey(name))
        {
            energyGrids[name].totalEnergy += power / 1000;
        }
        else
        {
            energyGrids.Add(name,new EnergyGrid {totalEnergy=power/1000 });
        }
        powerChange?.Invoke(this, name);
    }

    public int Output(string name, int power)
    {
        if (energyGrids.ContainsKey(name))
        {
            float te = energyGrids[name].totalEnergy;
            float ce = energyGrids[name].consumeEnergy;
            int outPwoer = 0;
            if (te >= ce + power)
            {
                outPwoer = power;
            }
            else if (te > ce)
            {
                outPwoer = (int)((te - ce) * 1000);
            }
           
            ce += power / 1000;
            powerChange?.Invoke(this, name);
            return outPwoer;
        }
        return 0;
    }

    public void RemoveInput(string name, int power)
    {
        if (energyGrids.ContainsKey(name))
        {
            energyGrids[name].totalEnergy -= power / 1000;
        }
        powerChange?.Invoke(this, name);
    }

    public void RemoveOutput(string name, int power)
    {
        if (energyGrids.ContainsKey(name))
        {
            energyGrids[name].consumeEnergy -= power / 1000;
        }
        powerChange?.Invoke(this, name);
    }

    public EnergyGrid GetEnergyGrids(string name)
    {
        return energyGrids[name];
    }

    public class EnergyGrid
    {
        public string name;
        public float totalEnergy;
        public float consumeEnergy;
    }
}
