using BattleSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Arms))]
public class EquipEditor : Editor
{
    private Arms m_Target;

    //重写OnInspectorGUI方法，当激活此面板区域时调用
    public override void OnInspectorGUI()
    {
        //加入此句，不影响原在Inspector绘制的元素
        // base.OnInspectorGUI();

        //获取指定脚本对象
        m_Target = target as Arms;

        m_Target.range = EditorGUILayout.FloatField("射程", m_Target.range);
        m_Target.minRange = EditorGUILayout.FloatField("最小射程", m_Target.minRange);
        m_Target.attack = EditorGUILayout.FloatField("伤害", m_Target.attack);
        m_Target.numberAttacks = EditorGUILayout.IntField("单次攻击的伤害次数", m_Target.numberAttacks);
        m_Target.attackInterval = EditorGUILayout.FloatField("攻击间隔", m_Target.attackInterval);
        m_Target.explosiveRange = EditorGUILayout.FloatField("爆炸范围", m_Target.explosiveRange);
        m_Target.muzzleFlame = EditorGUILayout.ObjectField("枪口焰", m_Target.muzzleFlame, typeof(GameObject), true) as GameObject;
        m_Target.isCloseCombat = EditorGUILayout.Toggle("是否允许近战攻击", m_Target.isCloseCombat);
        if (m_Target.isCloseCombat)
        {
            m_Target.closeCombatRange = EditorGUILayout.FloatField("近战射程", m_Target.closeCombatRange);
            m_Target.closeCombatAttack = EditorGUILayout.FloatField("近战伤害", m_Target.closeCombatAttack);
            m_Target.closeCombatAttackInterval = EditorGUILayout.FloatField("近战攻击间隔", m_Target.closeCombatAttackInterval);
        }

        m_Target.shootType = (ShootType)EditorGUILayout.EnumFlagsField("射击类型", m_Target.shootType);
        switch (m_Target.shootType)
        {
            case ShootType.gun:
                break;
            case ShootType.ballistic:
                m_Target.ammo = EditorGUILayout.ObjectField("弹道类射击飞行物", m_Target.ammo, typeof(GameObject), true) as GameObject;
                m_Target.flightSpeed = EditorGUILayout.FloatField("弹道类的飞行速度", m_Target.flightSpeed);
                break;
            case ShootType.closeCombat:
                break;
            case ShootType.notAllow:
                break;
          
        }
        m_Target.hitType = (HitType)EditorGUILayout.EnumFlagsField("伤害类型", m_Target.hitType);
        m_Target.isAutoAttack = EditorGUILayout.Toggle("是否自动攻击", m_Target.isAutoAttack);

       
    }
}
