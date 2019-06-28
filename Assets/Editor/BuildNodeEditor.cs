using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

//[CustomEditor(typeof(BuildNode))]
public class BuildNodeEditor : Editor
{
    private BuildNode m_Target;



    public override void OnInspectorGUI() {
        m_Target = target as BuildNode;

        m_Target.building = EditorGUILayout.ObjectField("建筑资源", m_Target.building, typeof(BuildingSO), true) as BuildingSO;
        if (m_Target.building!=null)
        {
            //if (m_Target.gameObject.GetComponentInChildren<Image>().sprite==null)
            //{
            //    m_Target.gameObject.GetComponentInChildren<Image>().sprite = m_Target.building.lowSource;
            //}
            //m_Target.gameObject.GetComponentInChildren<Text>().text = m_Target.building.objectName;
          
        }
       
    }
}
