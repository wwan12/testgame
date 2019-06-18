using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildNode : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler
{
    [Tooltip("该节点对应的建筑")]
    public BuildingSO building;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button==PointerEventData.InputButton.Left)
        {
            GameObject.FindObjectOfType<PlacingManage>().SetPlaceable(building);
        }
   
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Text>().text = building.objectName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
