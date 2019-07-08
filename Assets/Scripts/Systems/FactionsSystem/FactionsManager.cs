using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionsManager : MonoBehaviour
{
    private Dictionary<string,int> factionalRelations;


   // private Factions[] allFactions;
   // Start is called before the first frame update
    void Start()
    {
        factionalRelations = new Dictionary<string, int>();
      //  allFactions = Resources.LoadAll<Factions>("Assets/FactionsAssets");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeRelation(string fName,int rel)
    {
        if (factionalRelations.ContainsKey(fName))
        {
            factionalRelations[fName] += rel;
        }
        else
        {
           Factions factions= Resources.Load<Factions>("Assets/FactionsAssets" + fName);//todo
            factionalRelations.Add(factions.factName,factions.initRelations);
            //foreach (var f in allFactions)
            //{
            //    if (f.factName.Equals(fName))
            //    {
            //        factionalRelations.Add(f.factName, rel);
            //    }
            //}
        }
    }

    public int GetRelation(string fName) {
        if (factionalRelations.ContainsKey(fName))
        {
            return factionalRelations[fName];
        }
        return 0;
    }

    public Dictionary<string, int> SaveFactions()
    {
        return factionalRelations;
    }

    public void ReadFactions(Dictionary<string, int> f)
    {
        factionalRelations = f;
    }

    public enum BadThings
    {
        small,
        medium,
        major,
        noMercy,
    }
}

