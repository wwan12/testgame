using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ListItem : MonoBehaviour
{
    public GameObject itemEvent;
  //  [SerializeField]
  //  public ItemEventTrigger m_ItemEventTrigger = new ItemEventTrigger();

    // Start is called before the first frame update
    void Start()
    {
     
        EventTrigger trigger = this.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        UnityEngine.Events.UnityAction<BaseEventData> click = new UnityEngine.Events.UnityAction<BaseEventData>(OnItemClick);
        entry.callback.AddListener(click);

        trigger.triggers.Clear();
        trigger.triggers.Add(entry);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnItemClick(BaseEventData ed)
    {
        //  m_ItemEventTrigger.itemEvent.Invoke();
       // itemEvent.GetComponent
    }
  

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new NotImplementedException();
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


