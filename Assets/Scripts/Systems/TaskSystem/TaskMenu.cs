using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskMenu : MonoBehaviour
{
    public GameObject chainLabel;

    public GameObject taskLabel;
 

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindObjectOfType<TaskManager>().AddWorkOrder(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTasks(Task task)
    {
        GameObject t = Resources.Load<GameObject>("");
        t = GameObject.Instantiate<GameObject>(t);
        t.name = task.name;
        t.GetComponent<Text>().text=task.name;
        t.transform.SetParent(taskLabel.transform, false);

    }

    public void SetTasks(TaskChain chain)
    {
        GameObject t = Resources.Load<GameObject>("");
        t = GameObject.Instantiate<GameObject>(t);
        t.name = chain.name;
        t.GetComponent<Text>().text = chain.name;
        t.transform.SetParent(chainLabel.transform, false);
    }

    public void ClickLabel(string taskName)
    {

    }

    public void SetAvailable(string taskName) {
        
    }
}
