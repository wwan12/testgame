using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 自动刷新点
/// </summary>
public class AutoGenerateControl : MonoBehaviour
{
    
    GameObject m_Player;
    [Tooltip("要生成的预制体")]
    public Transform prefab;
    [Tooltip("现在生成的数量")]
    public int Count = 0;
    [Tooltip("生成最大数量")]
    public int Max=1;
    [Tooltip("生成的时间间隔")]
    public float refreshTime = 0;
    [Tooltip("离多远开始生成")]
    public float distance = 150f;
    [Tooltip("生成域半径")]
    public float raid = 1f;

    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGenerate()
    {
        StartCoroutine(GenerateEnemy());
    }

    public void StopGenerate()
    {
        StopAllCoroutines();
    }

    IEnumerator GenerateEnemy() {
        while (true)
        {
            //如果生成数量达到最大值 停止生成
            if (Count < Max)
            {
                float m_Distance = Vector3.Distance(gameObject.transform.position, m_Player.transform.position);
                //让玩家与出生点距离小于等于d时开始创建克隆
                if (m_Distance <= distance)
                {
                    Vector2 InsPos = Random.insideUnitCircle * raid;//在一个域内随机生
                    Transform transformEnemy = (Transform)Instantiate(prefab,transform.position+ new Vector3(InsPos.x,InsPos.y,0), Quaternion.identity);

                }
            }
            yield return new WaitForSeconds(refreshTime);
        }
        
        
    }

}
