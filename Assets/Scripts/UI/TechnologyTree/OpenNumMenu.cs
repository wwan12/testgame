using EasyAnimation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// 点击显示gameobject的name+Menu的gameobject
/// </summary>
public class OpenNumMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    EasyAnimation_Move move;

    public Canvas canvas;
    [HideInInspector]
    public GameObject cacheGameObject;
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Open();
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        move.Play();
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        move.RePlay();
    }

    void Open()
    {
        if (cacheGameObject==null)
        {
            cacheGameObject = Resources.Load<GameObject>(gameObject.name + "Menu");
            cacheGameObject = GameObject.Instantiate<GameObject>(cacheGameObject);
            cacheGameObject.transform.SetParent(canvas.transform, false);
        }
     
        AppManage.Instance.SetOpenUI(cacheGameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        move = gameObject.GetComponent<EasyAnimation_Move>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
