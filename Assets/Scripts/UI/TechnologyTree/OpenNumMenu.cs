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
        AppManage.Instance.SetOpenUI(GameObject.Find(gameObject.name + "Menu"));
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
