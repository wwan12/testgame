using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArticlesAttachment : MonoBehaviour
{
    public int id;
    [Tooltip("是否可交互")]
    public ArticlesType type;
    public string note;
    [Tooltip("支持交互的类型")]
    public InteractiveType[] prefix;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AutoCreateId() {

    }

    public enum ArticlesType
    {
        NOT_INTERACTIVE=0,
        INTERACTIVE =1,

    }
    public enum InteractiveType
    {
        PLAYER = 0,
        MAP = 1,
        BUILD=2,

    }
}
