using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ListItem : MonoBehaviour,IPointerClickHandler
{

    //  [SerializeField]
    //  public ItemEventTrigger m_ItemEventTrigger = new ItemEventTrigger();

    // Start is called before the first frame update

    public int sel;

    public UnityAction<int> leftAction;
    public UnityAction<int> rightAction;

    void Start()
    {
     
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button==PointerEventData.InputButton.Left)
        {
            leftAction(sel);
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            rightAction(sel);
        }
    }




    //  [Serializable]
    //  public class ItemEventTrigger
    //  {
    //     public int index = 0;

    //    [Serializable]
    //    public class ItemEvent : UnityEvent
    //   {

    //   }

    // [SerializeField]
    //  public ItemEvent itemEvent = new ItemEvent();
    // }

}


