using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    [CreateAssetMenu(fileName = "自定义防具", menuName = "自定义生成系统/装备/防具")]
    public class Armor : Equip
    {
        [Range(0, 1)]
        public float defense;
        [Range(0,1)]
        public float coverage;
       // public float toughness;
        public float minDefense;
    }
}

