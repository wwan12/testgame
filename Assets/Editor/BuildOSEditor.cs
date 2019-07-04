using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildingSO))]
public class BuildOSEditor : Editor
{
    private BuildingSO m_Target;

    //重写OnInspectorGUI方法，当激活此面板区域时调用
    public override void OnInspectorGUI()
    {
        //加入此句，不影响原在Inspector绘制的元素
        // base.OnInspectorGUI();

        //获取指定脚本对象
        m_Target = target as BuildingSO;
        m_Target.hierarchy = EditorGUILayout.IntField("菜单中层级", m_Target.hierarchy);
        
        m_Target.objectName = EditorGUILayout.DelayedTextField("建筑名称", m_Target.objectName);
        m_Target.buildingPrefab = EditorGUILayout.ObjectField("建筑预制", m_Target.buildingPrefab, typeof(GameObject), true) as GameObject;
        m_Target.lowSource = EditorGUILayout.ObjectField("预览图", m_Target.lowSource, typeof(Sprite), true) as Sprite;
        m_Target.buildTime = EditorGUILayout.FloatField("建造时间", m_Target.buildTime);
        //EditorGUILayout.PropertyField(cost,true);
        m_Target.dTime = EditorGUILayout.FloatField("拆除时间", m_Target.dTime);
        m_Target.durable = EditorGUILayout.FloatField("耐久", m_Target.durable);
       
        m_Target.type = (BuildingSO.BuildType)EditorGUILayout.EnumFlagsField("特殊类型", m_Target.type);
        if (m_Target.type== BuildingSO.BuildType.collect)
        {
            m_Target.res = (ResourceType.AttributionType)EditorGUILayout.EnumFlagsField("采集的资源", m_Target.res);
          //  m_Target.res = EditorGUILayout.ObjectField("采集的资源", m_Target.res, typeof(ResourceType), true) as ResourceType;
            m_Target.collectNum = EditorGUILayout.IntField("每个循环采集数量", m_Target.collectNum);
            m_Target.collectInterval = EditorGUILayout.FloatField("采集间隔", m_Target.collectInterval);           
        }

        m_Target.showPosition = EditorGUILayout.BeginFoldoutHeaderGroup(m_Target.showPosition, "建造费用");
        if (m_Target.showPosition)
        {
            m_Target.costLength = EditorGUILayout.IntField("需资源种类数量", m_Target.costLength);
            if (m_Target.costLength >0)
            {
                if (m_Target.costc==null)
                {
                    m_Target.costc = new BuildingSO.ResourcePrefab[m_Target.costLength];
                }
                else
                {
                    BuildingSO.ResourcePrefab[] c= new BuildingSO.ResourcePrefab[m_Target.costLength];
                    m_Target.costc.CopyTo(c,0);
                    m_Target.costc = c;
                }
                
                for (int i = 0; i < m_Target.costLength; i++)
                {

                    m_Target.costc[i].type = EditorGUILayout.ObjectField("资源种类", m_Target.costc[i].type, typeof(ResourceType), true) as ResourceType;
                    m_Target.costc[i].num = EditorGUILayout.IntField("资源数量", m_Target.costc[i].num);
                }
                if (m_Target.cost == null)
                {
                    m_Target.cost = new Dictionary<string, int>();
                }
                else
                {
                    m_Target.cost.Clear();                    
                }                
                foreach (var c in m_Target.costc)
                {
                    if (c.type!=null)
                    {
                        m_Target.cost.Add(c.type.resName, c.num);
                    }                  
                }
              
            }
           // showPosition = !showPosition;
        }
      //  EditorGUILayout.EndFoldoutHeaderGroup();
       
    }
   
}
