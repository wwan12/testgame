using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tag : MonoBehaviour, IPointerClickHandler
{
    public delegate void ClickCallBack(int i);
    public event ClickCallBack clickCallBack;
    public int index;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button==PointerEventData.InputButton.Left)
        {
            clickCallBack(index);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
