using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BagManage))] //指定要编辑的脚本对象
public class BagAutoEditor : Editor
{
    private BagManage m_Target;

    //重写OnInspectorGUI方法，当激活此面板区域时调用
    public override void OnInspectorGUI()
     {
         //加入此句，不影响原在Inspector绘制的元素
        // base.OnInspectorGUI();
 
         //获取指定脚本对象
         m_Target = target as BagManage;

        m_Target.itemInfoPanel = EditorGUILayout.ObjectField("物品信息显示框", m_Target.itemInfoPanel, typeof(GameObject), true) as GameObject;
        m_Target.itemInBag = EditorGUILayout.ObjectField("物体预制体", m_Target.itemInBag, typeof(GameObject), true) as GameObject;
        m_Target.equip = EditorGUILayout.ObjectField("右侧UI", m_Target.equip, typeof(GameObject), true) as GameObject;
        m_Target.isDrag = EditorGUILayout.Toggle("是否可拖动", m_Target.isDrag);
        m_Target.isAuto = EditorGUILayout.Toggle("自动排版", m_Target.isAuto);

        if (m_Target.isAuto)
         {
             m_Target.Lattice = EditorGUILayout.ObjectField("格子预制体", m_Target.Lattice, typeof(GameObject), true) as GameObject;
            m_Target.allCapacity = EditorGUILayout.IntField("容量", m_Target.allCapacity);
            m_Target.top = EditorGUILayout.FloatField("整体距上", m_Target.top);
            m_Target.left = EditorGUILayout.FloatField("整体距左", m_Target.left);
            m_Target.autoTop = EditorGUILayout.FloatField("行间距", m_Target.autoTop);
            m_Target.autoLeft = EditorGUILayout.FloatField("列间距", m_Target.autoLeft);
            m_Target.autoSize = EditorGUILayout.FloatField("格子大小", m_Target.autoSize);
            m_Target.lineNum = EditorGUILayout.IntField("每行有几个格子", m_Target.lineNum);
        }
     }


}
