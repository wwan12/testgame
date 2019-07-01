using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildNode : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler
{
    [Tooltip("该节点的层级")]
    public int hierarchy;
    [Tooltip("该节点对应的建筑")]
    public BuildingSO building;
    /// <summary>
    /// 是否允许建造
    /// </summary>
    public bool isBuild { get; private set; }

    public BuildNode(BuildingSO building) {
        this.building = building;
    }

    public void SetAvailable() {
        isBuild = true;
        gameObject.GetComponentInChildren<Text>().color = Color.white;
        gameObject.GetComponentInChildren<Image>().color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button==PointerEventData.InputButton.Left)
        {
            //GameObject.FindObjectOfType<PlacingManage>().SetPlaceable(building);
            Messenger.Broadcast(EventCode.BUILD_THIS, building);
        }
   
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        hierarchy = building.hierarchy;
        isBuild = building.isBuild;
        if (!isBuild)
        {
            gameObject.GetComponentInChildren<Text>().color = Color.gray;
            gameObject.GetComponentInChildren<Image>().color = Color.grey;
        }
        building.objectName = building.name;
        gameObject.GetComponentInChildren<Text>().text = building.objectName;
        gameObject.GetComponentInChildren<Image>().sprite = building.lowSource;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
