using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskMenu : MonoBehaviour
{
    private TaskManager taskManager;

    public GameObject taskList;

    public GameObject listContent;

    public GameObject detailsLabel;
    /// <summary>
    /// 现在选中的task
    /// </summary>
    private Task showTask;

    private int index;

    public int chainIndex;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindObjectOfType<TaskManager>().AddWorkOrder(this);
        if (AppManage.Instance.isInGame)
        {
            SetTasks();
            TaskLabel label= taskList.transform.GetChild(index).gameObject.GetComponent<TaskLabel>();
            label.clickCallBack.Invoke(label.task);
           // ReadNodes(AppManage.Instance.saveData.tecStates);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetTasks()
    {
        for (int j = 0; j < taskList.transform.childCount; j++)
        {
            Destroy(taskList.transform.GetChild(j).gameObject);
        }
        float lx=40 ;
        float ly = 40;
        float w = 0;
        float h = 0;
        int i=0;
        //foreach (var chain in taskManager.chainTasks)
        //{
        //    GameObject t = Resources.Load<GameObject>("prefabs/UI/TaskCatalog");
        //    t = GameObject.Instantiate<GameObject>(t);
        //    t.GetComponent<TaskLabel>().SetTask(chain).clickCallBack = ClickLabel;
        //    RectTransform rect = t.GetComponent<RectTransform>();
        //    w=  rect.sizeDelta.x;
        //    h = rect.sizeDelta.y;
        //    rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, lx, w);
        //    rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, ly+(i*h)+ly*(i+1), h);
        //    rect.SetParent(taskList.transform,false);         
        //    i++;
        //}
        
        foreach (var task in taskManager.freeTasks)
        {
            GameObject t = Resources.Load<GameObject>("prefabs/UI/TaskCatalog");
            t = GameObject.Instantiate<GameObject>(t);
            t.GetComponent<TaskLabel>().SetTask(task).clickCallBack = ClickLabel;
            RectTransform rect = t.GetComponent<RectTransform>();
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, lx, w);
            rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, ly + (i * h) + ly * (i + 1), h);
            rect.SetParent(taskList.transform, false);
            i++;
        }
        float totalH = (i + 1) * ly + h * i;
        if (totalH>880)
        {
            listContent.GetComponent<RectTransform>().sizeDelta = new Vector2(780, totalH+100);
        }       
    }

    public void Insert(int index,Task task)
    {
        if (index< chainIndex)
        {

        }
    }

    public void ClickLabel(Task task)
    {
        showTask = task;
        detailsLabel.GetComponent<Text>().text=task.note;
        detailsLabel.GetComponent<Button>().onClick.AddListener(SetAvailable);
    }
    /// <summary>
    /// 设为追踪
    /// </summary>
    public void SetAvailable() {
        GameObject.FindObjectOfType<TaskManager>().activatedTask = showTask;
       // showTask.taskControl.OnInProgress();
       
    }
}
