using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Typewriter : MonoBehaviour
{
    [Tooltip("显示文字的间隔")]
    public float pause=0.1f;
    [Tooltip("缓存数据条数")]
    public int cacheNum=20;
    [Tooltip("自适应")]
    public bool isAdaption;
    public float height;
    private ScrollRect scrollRect;
    private Queue<string> queue;
    private Text textUI;
    private string printText;//打印的字
    private bool isRun;
    private int lines;
    // Start is called before the first frame update
    void Start()
    {
        scrollRect= gameObject.GetComponentInParent<ScrollRect>();

        textUI = gameObject.GetComponent<Text>();
        queue = new Queue<string>(cacheNum);
        height = textUI.canvas.pixelRect.size.y;

        if (isAdaption)
        {
            gameObject.AddComponent<ContentSizeFitter>().verticalFit= ContentSizeFitter.FitMode.PreferredSize;
        }
        else
        {
             lines = Convert.ToInt32(height / (textUI.fontSize + textUI.lineSpacing*2));
            
        }

        InitBaseText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitBaseText() {
        AddQueue("据上次启动已经过" + Time.deltaTime + "次时钟频率");
        AddQueue("正在接管系统日志...");
        AddQueue("");
        AddQueue("系统已上线_");
    }

    public void AddQueue(string text)
    {        
        queue.Enqueue(text);
        if (!isRun)
        {
            isRun = true;
            StartCoroutine(TypeText());
        }         
    }

    public void Clear()
    {
        textUI.text = "";
        AddQueue("系统清除日志");
    }

    public void Hide() {

    }
    public void Show()
    {

    }

    /**输出文本功能*/
    IEnumerator TypeText()
    {
        while (queue.Count>0)
        {
            string mText = "";
            if (!isAdaption)//非自适应自动删除第一条
            {
                string[] c = textUI.text.Split('\n');
                if (c.Length >= lines)
                {
                    c[0] = "";
                    for (int i = 1; i < c.Length-1; i++)
                    {
                        mText += (c[i] + "\n");
                    }
                }
                else
                {
                    mText = textUI.text;
                }
            }
          
            string cache = isAdaption?textUI.text.ToString():mText;
            string word = queue.Dequeue();
            foreach (char letter in word.ToCharArray())
            {
                printText += letter;//把这些字赋值给Text
                yield return new WaitForSeconds(pause);
                textUI.text = cache+printText;
            }
            textUI.text = textUI.text + "\n";
            printText = "";
            if (isAdaption)
            {
                scrollRect.verticalNormalizedPosition = 0;
            }    
            yield return new WaitForSeconds(3*pause);
           
        }
        isRun = false;
        //显示完成换行
    }
}
