using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TechnologyNode : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler
{
    [Tooltip("该节点的层级")]
    public int hierarchy;
    [Tooltip("该节点对应的科技")]
    public Technology tec;
   
   
 


    public void SetAvailable()
    {
        tec.isResearch = true;
        gameObject.GetComponentInChildren<Text>().color = Color.white;
        gameObject.GetComponentInChildren<Image>().color = Color.white;
    }

    public void SetComplete()
    {
        tec.isComplete = true;
        gameObject.GetComponentInChildren<Text>().color = Color.blue;
        gameObject.GetComponentInChildren<Image>().color = Color.blue;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            TechnologyManager mana = GameObject.FindObjectOfType<TechnologyManager>();
            if (tec.isResearch&&!tec.isComplete)
            {
                mana.StartTec(tec);
            }
            else
            {
                Messenger.Broadcast(EventCode.AUDIO_EFFECT_PLAY,AudioCode.SYSTEM_ERROR);
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        hierarchy = tec.hierarchy;
        //isResearch = tec.isResearch;
        if (!tec.isResearch)
        {
            gameObject.GetComponentInChildren<Text>().color = Color.gray;
            gameObject.GetComponentInChildren<Image>().color = Color.grey;
        }
        //tec.objectName = building.name;
        gameObject.name = tec.name;
        gameObject.GetComponentInChildren<Text>().text = tec.name; ;
        gameObject.GetComponentInChildren<Image>().sprite = tec.lowSource;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
