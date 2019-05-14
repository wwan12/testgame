using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Warehouse
{
    private static readonly Warehouse _instance = new Warehouse();
    private GameObject[] articles;
    private GameObject[] builds;
    private GameObject[] enemys;
    private GameObject[] neutrals;
    private GameObject[] npcs;
    public event EventHandler<ArticlesAttachment> DestroyCallBack;
    public static Warehouse Instance
    {
        get
        {
            return _instance;
        }
    }

    private Warehouse() { Init(); }

    private void Init()
    {
        
    }

    public void Initialize() {
        articles= Resources.LoadAll<GameObject>("Warehouse/Articles");
        builds = Resources.LoadAll<GameObject>("Warehouse/builds");
        enemys = Resources.LoadAll<GameObject>("Warehouse/enemys");
        neutrals = Resources.LoadAll<GameObject>("Warehouse/neutrals");
        npcs = Resources.LoadAll<GameObject>("Warehouse/npcs");

    }
    /// <summary>
    /// 在指定位置创建一个仓库中的物品
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="id"></param>
    /// <param name="z"></param>
    /// <param name="type">物品的类型</param>
    /// <returns>null为在指定类型中没找到</returns>
    public GameObject Create(float x,float y, int id, float z =0,ArticlesType type=ArticlesType.ARTICLES)
    {
        GameObject[] cache=null;
        switch (type)
        {
            case ArticlesType.ARTICLES:cache = articles;
                break;
            case ArticlesType.BUILDS:cache = builds;
                break;
            case ArticlesType.ENEMYS:cache = enemys;
                break;
            case ArticlesType.NEUTRALS: cache = neutrals;
                break;
            case ArticlesType.NPCS:cache = npcs;
                break;
        }
        foreach (var item in cache)
        {
            if (item.GetComponent<ArticlesAttachment>().id==id)
            {
                GameObject art =GameObject.Instantiate<GameObject>(item, new Vector3(x, y, z), Quaternion.identity);
                return art;
            }
        }
        return null;
    }

    /// <summary>
    /// 通过id获取仓库里物品的详细信息
    /// </summary>
    /// <param name="id"></param>
    public ArticlesAttachment GetAtriclesInfo(int id, ArticlesType type = ArticlesType.ARTICLES) {
        //用unity的内存指针去找，效率待测
        //string p= System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Enum.GetName(typeof(ArticlesType), type));
        //return Resources.Load<GameObject>("Warehouse/" + p + "/" + id.ToString()).GetComponent<ArticlesAttachment>();

        GameObject[] cache = null;
        switch (type)
        {
            case ArticlesType.ARTICLES:
                cache = articles;
                break;
            case ArticlesType.BUILDS:
                cache = builds;
                break;
            case ArticlesType.ENEMYS:
                cache = enemys;
                break;
            case ArticlesType.NEUTRALS:
                cache = neutrals;
                break;
            case ArticlesType.NPCS:
                cache = npcs;
                break;
        }
        foreach (var item in cache)
        {
            if (item.GetComponent<ArticlesAttachment>().id == id)
            {
                return item.GetComponent<ArticlesAttachment>();
            }
        }
        return null;
    }
    /// <summary>
    /// 回收一个任意物品
    /// </summary>
    /// <param name="gameObject"></param>
    public void Destroy(GameObject gameObject, [DefaultValue("1.0F")] float t)
    {
        DestroyCallBack(this, gameObject.GetComponent<ArticlesAttachment>());
        GameObject.Destroy(gameObject,t);      
    }

    public enum ArticlesType
    {
        ARTICLES = 0,
        BUILDS = 1,
        ENEMYS=2,
        NEUTRALS=3,
        NPCS=4,
    }
}
