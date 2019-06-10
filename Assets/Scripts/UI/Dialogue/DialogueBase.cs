using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 读取剧本，分发剧本，提供流程方法
/// </summary>
public class DialogueBase : MonoBehaviour
{
    [Tooltip("输出字的时间间隔")]
    public float letterPause = 0.1f;//时间间隔
    [Tooltip("普通对话显示窗体")]
    public RectTransform generalTextLabel;
    [Tooltip("选择对话显示窗体")]
    public RectTransform choiceTextLabel;
    [Tooltip("旁白显示窗体")]
    public RectTransform asideTextLabel;
    [Tooltip("读取的剧本名称")]
    public string textName;
    [Tooltip("在那个canvas上绘制")]
    public RectTransform gameUI;
    public bool isAuto;
    private Type lastType;
    string readInAsset;
    bool isStart;
    float pause;
    Text showText;
    RectTransform showTextLabel;
    private string word;//存储输出文本
    private string printText;//打印的字
    private int i, j = 0;//设置第几句话，以及判断是否出现新的语句
    public event EventHandler<int> ChoiceLimbsCallBack;
    public event EventHandler<string> WriterOverCallBack;
    //  private Queue<string> queue;
    private GameObject[] choiceItems; 
    public enum Type
    {
        general,
        choice,
        aside,

    }


    /// <summary>
    /// 0>事件id,1>类型,2>主体,3>附加
    /// 类型：0保留，1普通对话，2选择支，3跳转
    /// 附加：^0(无)，^1(无),^2^选项，3^要跳转的行号
    /// </summary>
    /// 改为json
      private BranchInfo[] Text;

    // Use this for initialization
    void Start()
    {
      //  queue = new Queue<string>();
    }

    public void ReadText(string name) {
        readInAsset = Resources.Load<TextAsset>("Dialogue/"+ name).text;
     //   Text= readInAsset.Split('|');
    }

    public void Ext() {
        foreach (var t in Text)
        {
            switch (t.type)
            {
                case Type.general:
                    AddNextText(t.text);
                    break;
                case Type.choice:
                    AddChoiceText(t.question, t.answer);
                    break;
                case Type.aside:
                    AddAside(t.text);
                    break;
                default:
                    break;
            }


        }
    }

     void ShowDialogue(string text)
    {
        GameObject.Instantiate(showTextLabel.gameObject);      
        word = text;
        showText = showTextLabel.gameObject.GetComponent<Text>();
        pause = letterPause;
        isStart = true;
        TextChange();
        AppManage.Instance.SetOpenUI(showTextLabel.gameObject);
    }

     void ShowChoice(string question, string[] answerItems)
    {
        GameObject.Instantiate(showTextLabel.gameObject).transform.Find("question").GetComponent<Text>().text=question;//设置问题
        choiceItems = new GameObject[answerItems.Length];
        for (int i = 0; i < answerItems.Length; i++)
        {
            GameObject obj = new GameObject(answerItems[i]);
            obj.name = i.ToString();
            obj.transform.SetParent(choiceTextLabel.gameObject.transform.Find("answer"));
            EventTrigger eventTrigger = obj.AddComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerClick,
                callback = new EventTrigger.TriggerEvent()
            };
            entry.callback.AddListener(Callback);
            eventTrigger.triggers.Add(entry);
            Text typeText = obj.AddComponent<Text>();            
            typeText.text = answerItems[i];
            choiceItems[i] = obj;
        }
        AppManage.Instance.SetOpenUI(showTextLabel.gameObject);
    }

    void Callback(BaseEventData eventData)
    {
        if (((PointerEventData)eventData).button==PointerEventData.InputButton.Left)
        {
            ChoiceLimbsCallBack(this,int.Parse(eventData.selectedObject.name) );
        }
        foreach (var item in choiceItems)
        {
            Destroy(item);
        }
    }

    //返回整个文本字符串
    string[] ReadAllFile(string FileName)
    {
        string[] strs;
        return strs = File.ReadAllLines(FileName);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0)&& isStart)
        //{
        //    //检测对话显示完没有 i = j 就是还没有显示完
        //    if (i == j)
        //    {
        //        pause = 0.0f;
        //    }
        //    else
        //    {
        //        if (i < Text.Length - 1)
        //        {
        //            pause = letterPause;
        //            i++;
        //            TextChange();
        //        }
        //        else
        //        {//所有剧本展示完，关闭对话框

        //            CloseText();
        //        }
        //    }
        //}
    }
    public void AddNextText(string text) {

        //CloseText();
        switch (lastType)
        {         
            case Type.choice:
                showTextLabel = generalTextLabel;
                ShowDialogue(text);
                break;       

            default:
                word = text;
                TextChange();
                break;
        }
        lastType = Type.general;
       // queue.Enqueue(text);
    }
    [Obsolete]
    public void AddNextTexts(string[] texts)
    {
        foreach (var item in texts)
        {
            AddNextText(item);
           
        }
    }

    public void AddChoiceText(string question, string[] answerItems)
    {
        switch (lastType)
        {
            case Type.general:
                break;
            case Type.choice:
                break;
            case Type.aside:
                break;
            default:
                break;
        }
        showTextLabel = choiceTextLabel;
        ShowChoice(question, answerItems);
    }

    public void AddAside(string text)
    {
        GameObject.Find("CmdWindow").GetComponent<Typewriter>().AddQueue(text);
    }

    public void CloseText()
    {
        isStart = false;
        Destroy(showTextLabel.gameObject);
    }

    /**切换语句功能*/
    void TextChange(){
        if (letterPause==0)
        {
            printText = word;
            showText.text = printText;
            j = i;
        }
        else
        {
           // word = "";
           // word = line[2];
            printText = "";
            StartCoroutine(TypeText());
        }
    }
    /**输出文本功能*/
    IEnumerator TypeText()
    {       
        foreach (char letter in word.ToCharArray())
        {
            printText += letter;//把这些字赋值给Text
            yield return new WaitForSeconds(pause);
            showText.text = printText;
        }
        printText += "  ▼"; //提示
        j = i;
        WriterOverCallBack(this, printText);
    }

    public class BranchInfo
    {
        public Type type;
        public int id;
        public string text;
        public string question;
        public string[] answer;
    }
}
