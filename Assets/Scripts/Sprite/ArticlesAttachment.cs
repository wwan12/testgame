using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 提供物品相关信息
/// </summary>
[Obsolete("物品信息改为外部读取")]
public class ArticlesAttachment : MonoBehaviour
{
    public string id;
    [Tooltip("物品的类型")]
    public Warehouse.ArticlesType type;
    public string note;
    [Tooltip("支持交互的类型")]
    public InteractiveType[] prefix;
    [Tooltip("显示的精灵")]
    public Sprite sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        switch (type)
        {
            case Warehouse.ArticlesType.ARTICLES:
                break;
            case Warehouse.ArticlesType.BUILDS:
                gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;           
                gameObject.AddComponent<PolygonCollider2D>().isTrigger=true;
                break;
            case Warehouse.ArticlesType.ENEMYS:
                gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.AddComponent<PolygonCollider2D>().isTrigger = true;
                break;
            case Warehouse.ArticlesType.NEUTRALS:
                gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.AddComponent<PolygonCollider2D>().isTrigger = true;
                break;
            case Warehouse.ArticlesType.NPCS:
                gameObject.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                gameObject.AddComponent<CapsuleCollider2D>().isTrigger = true;
                break;
            default:
                break;
        }
        gameObject.AddComponent<SpriteRenderer>().sprite=sprite;
        gameObject.name = id.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AutoCreateId() {

    }


    private void OnMouseEnter()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerManage>().available = gameObject;
    }



    public enum InteractiveType
    {
       
        PLAYER = 1,
        MAP = 2,
        BUILD=3,
       

    }
}
