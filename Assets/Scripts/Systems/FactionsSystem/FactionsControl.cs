using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactionsControl : MonoBehaviour
{
    public Factions factions;

    public bool atke;

    public bool surrender;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void OneRaid();

    public abstract void OneVisitor();

    public abstract void OneGift();

    public abstract void OneTribute();

    public  virtual void Surrender()
    {

    }
}
