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

        m_Target.itemInfoPanel = EditorGUILayout.ObjectField("itemInfoPanel", m_Target.itemInfoPanel, typeof(GameObject), true) as GameObject;
        m_Target.isAuto = EditorGUILayout.Toggle("isAuto", m_Target.isAuto);
        if (m_Target.isAuto)
         {
             m_Target.Lattice = EditorGUILayout.ObjectField("Lattice", m_Target.Lattice, typeof(GameObject), true) as GameObject;
            m_Target.allCapacity = EditorGUILayout.IntField("allCapacity", m_Target.allCapacity);
            m_Target.devier = EditorGUILayout.IntField("devier", m_Target.devier);
            m_Target.autoTop = EditorGUILayout.IntField("autoTop", m_Target.autoTop);
            m_Target.autoLeft = EditorGUILayout.IntField("autoLeft", m_Target.autoLeft);
            m_Target.lineNum = EditorGUILayout.IntField("lineNum", m_Target.lineNum);
        }
     }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
