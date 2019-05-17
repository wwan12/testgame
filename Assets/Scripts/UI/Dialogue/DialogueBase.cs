using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 读取剧本，分发剧本，提供流程方法
/// </summary>
public class DialogueBase : MonoBehaviour
{
    [Tooltip("输出字的时间间隔")]
    public float letterPause = 0.1f;//时间间隔
    [Tooltip("显示窗体")]
    public GameObject textLabel;
    [Tooltip("读取的剧本名称")]
    public string textName;
    string readInAsset;
    bool isStart;
    float pause;
    Text showText;
    private string word;//存储输出文本
    private string printText;//打印的字
    private int i, j = 0;//设置第几句话，以及判断是否出现新的语句
    public event EventHandler<BranchInfo> ChoiceLimbsCallBack;
    public event EventHandler<BranchInfo> GoToCallBack;



    /// <summary>
    /// 0>事件id,1>类型,2>主体,3>附加
    /// 类型：0保留，1普通对话，2选择支，3跳转
    /// 附加：^0(无)，^1(无),^2^选项，3^要跳转的行号
    /// </summary>
    private string[] Text;

    // Use this for initialization
    void Start()
    {
        ReadText(textName);
    }

    public void ReadText(string name) {
        readInAsset = Resources.Load<TextAsset>("Dialogue/"+ name).text;
        Text= readInAsset.Split('|');
    }

    public void StartDialogue() {
        if (textLabel==null)
        {
            textLabel= Resources.Load<GameObject>("Dialogue/DefaultLabel.perfab");
        }
        AppManage.Instance.SetOpenUI(textLabel);
        //GameObject.Instantiate(textLabel);
        showText = textLabel.GetComponent<Text>();
        pause = letterPause;
        isStart = true;
        TextChange();
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
        if (Input.GetMouseButtonDown(0)&& isStart)
        {
            //检测对话显示完没有 i = j 就是还没有显示完
            if (i == j)
            {
                pause = 0.0f; //加快显的速度，让对话速度显示完
            }
            else
            {
                //检测对话语句是否超出了最大限制，超出了就DO STH.
                if (i < Text.Length - 1)
                {
                    pause = letterPause;
                    i++;
                    TextChange();
                }
                else
                {//所有剧本展示完，关闭对话框
                    isStart = false;
                    CanvasGroup group = textLabel.GetComponent<CanvasGroup>();
                    group.alpha = 0;
                    group.interactable = false;
                    group.blocksRaycasts = false;
                    //DO STH.

                }
            }
        }
    }
    /**切换语句功能*/
    void TextChange(){
        string[] line = Text[i].Split(',');
        switch (line[1])
        {
            case "2":
                ChoiceLimbsCallBack(this, new BranchInfo() {pointerId=int.Parse(line[0]),option=line[3].Split('^')  });
                break;
            case "3":
                GoToCallBack(this, new BranchInfo() { pointerId = int.Parse(line[0]), gotoId = int.Parse(line[3])  });
                break;
            default:
                break;
        }
        if (letterPause==0)
        {
            printText = line[2];
            showText.text = printText;
            j = i;
        }
        else
        {
            word = "";
            word = line[2];
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
        printText += "  ▼"; //标记可按键的提示
        j = i;                //避免出现下一句不显示的情况将对话记录+1
    }

    public class BranchInfo
    {
        public int pointerId;
        public int gotoId;
        public string[] option;
    }
}
