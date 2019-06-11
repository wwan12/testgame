using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControl : MonoBehaviour
{
    /// <summary>
    /// 0>事件id,1>类型,2>主体,3>附加
    /// 类型：0保留，1普通对话，2选择支，3跳转
    /// 附加：^0(无)，^1(无),^2^选项，3^要跳转的行号
    /// </summary>
    /// 改为json
    private DialogueHelper.BranchInfo[] Text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ext()
    {
        DialogueHelper dialogue = new DialogueHelper();
        foreach (var t in Text)
        {
            switch (t.type)
            {
                case DialogeType.general:
                    dialogue.AddNextText(t.text,this);
                    break;
                case DialogeType.choice:
                    dialogue.AddChoiceText(t.question, t.answer);
                    break;
                case DialogeType.aside:
                    dialogue.AddAside(t.text);
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator TextStream() {

        yield return null;
    }
}
