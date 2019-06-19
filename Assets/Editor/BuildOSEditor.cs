using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildingSO))]
public class BuildOSEditor : Editor
{
    private BuildingSO m_Target;
    [System.Serializable]
    private struct ResourcePrefab
    {
        public ResourceType type;
        public int num;
    }

    private ResourcePrefab[] cost;
    private int costLength=0;
    private bool showPosition;

    //重写OnInspectorGUI方法，当激活此面板区域时调用
    public override void OnInspectorGUI()
    {
        //加入此句，不影响原在Inspector绘制的元素
        // base.OnInspectorGUI();

        //获取指定脚本对象
        m_Target = target as BuildingSO;

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

            m_Target.res = EditorGUILayout.ObjectField("采集的资源", m_Target.res, typeof(ResourceType), true) as ResourceType;
            m_Target.collectNum = EditorGUILayout.IntField("每个循环采集数量", m_Target.collectNum);
            m_Target.collectInterval = EditorGUILayout.FloatField("采集间隔", m_Target.collectInterval);           
        }
       
        showPosition = EditorGUILayout.BeginFoldoutHeaderGroup(showPosition, "建造费用");
        if (showPosition)
        {
            costLength = EditorGUILayout.IntField("需资源种类数量", costLength);
            if (costLength>0)
            {
                cache(costLength);
                for (int i = 0; i < costLength; i++)
                {

                    cost[i].type = EditorGUILayout.ObjectField("资源种类", cost[i].type, typeof(ResourceType), true) as ResourceType;
                    cost[i].num = EditorGUILayout.IntField("资源数量", cost[i].num);
                }
                m_Target.cost = new Dictionary<string, int>();
                foreach (var c in cost)
                {
                    m_Target.cost.Add(c.type.resName,c.num);
                }
              
            }
           // showPosition = !showPosition;
        }
      //  EditorGUILayout.EndFoldoutHeaderGroup();
       
    }

    void cache(int Length) {
        if (cost == null)
        {
            cost = new ResourcePrefab[Length];
        }
        else {
            ResourcePrefab[] c = new ResourcePrefab[Length];
            cost.CopyTo(c,cost.Length);
            cost = c;
        }
    }
   
}
