using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TaskLabel : MonoBehaviour,IPointerClickHandler
{
    public delegate void ClickCallBack(Task task);

    public ClickCallBack clickCallBack;

    public Task task;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TaskLabel SetTask(Task task) {
        this.task = task;
        gameObject.name = task.name;
        gameObject.GetComponent<Text>().text = task.name;
        return this;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button==PointerEventData.InputButton.Left)
        {
            clickCallBack(task);
        }
    }
}
