using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactionsNpc : MonoBehaviour
{
    [Tooltip("NPC名称")]
    public string NpcName;
    [Tooltip("属于的派系")]
    public Factions factions;

    public Sprite head;
    [Tooltip("个人关系")]
    [Range(-1000,1000)]
    public int relations;//
    [Tooltip("倾向（负数混沌，正数秩序）")]
    [Range(-10,10)]
    public float inclination=1;
    private void Start()
    {
        gameObject.name = NpcName;
        relations+= FindObjectOfType<FactionsManager>().GetRelation(factions.factName);
    }

    public abstract void ChangeRelations(int rel);

    public abstract void Dead();

    public virtual void SeeBad(FactionsManager.BadThings things) {

        // FindObjectOfType<FactionsManager>().ChangeRelation(factions.factName,(int)inclination* FindObjectOfType<FactionsManager>().CalculationRel(relations,things));
        relations += CalculationRel(relations,things);
    }

    public int CalculationRel(int relations, FactionsManager.BadThings things)
    {
        int value = -10;
        switch (things)
        {
            case FactionsManager.BadThings.small:
                break;
            case FactionsManager.BadThings.medium:
                value = -50;
                break;
            case FactionsManager.BadThings.major:
                value = -300;
                break;
            case FactionsManager.BadThings.noMercy:
                value = -800;
                break;
        }


        if (relations > 600)
        {
            value += relations / 100;
        }
        else if (relations > 200)
        {
            value += relations / 200;
        }
        if (relations < -200)
        {
            value -= relations / 200;
        }
        else if (relations < -600)
        {
            value -= relations / 100;
        }
        return value;
    }

}
