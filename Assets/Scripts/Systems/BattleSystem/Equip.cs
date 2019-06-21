
using UnityEngine;

namespace BattleSystem
{
    public class Equip : ScriptableObject
    {
        /// <summary>
        /// 射程
        /// </summary>
        public float range;
        /// <summary>
        /// 最小射程
        /// </summary>
        public float minRange = 0f;
        /// <summary>
        /// 伤害
        /// </summary>
        public float attack;
        /// <summary>
        /// 单次攻击的伤害次数
        /// </summary>
        public int numberAttacks=1;
        /// <summary>
        /// 攻击间隔
        /// </summary>
        public float attackInterval;
        /// <summary>
        /// 爆炸范围
        /// </summary>
        public float explosiveRange=0f;
        /// <summary>
        /// 枪口焰
        /// </summary>
        public GameObject muzzleFlame;
        /// <summary>
        /// 弹道类射击飞行物
        /// </summary>
        public GameObject ammo;
        /// <summary>
        /// 弹道类的飞行速度
        /// </summary>
        public float flightSpeed;
        /// <summary>
        /// 是否允许近战攻击
        /// </summary>
        public bool isCloseCombat;
        /// <summary>
        /// 近战射程
        /// </summary>
        public float closeCombatRange;
        /// <summary>
        /// 近战伤害
        /// </summary>
        public float closeCombatAttack;
        /// <summary>
        /// 近战攻击间隔
        /// </summary>
        public float closeCombatAttackInterval;
       /// <summary>
       /// 射击类型
       /// </summary>
        public ShootType shootType;
        /// <summary>
        /// 伤害类型
        /// </summary>
        public HitType hitType;
        /// <summary>
        /// 是否自动攻击
        /// </summary>
        public bool isAutoAttack;
    }


    public enum ShootType
    {
        gun,
        ballistic,
        closeCombat,
        notAllow,
    }

    public enum HitType
    {
        none,
    }
}


