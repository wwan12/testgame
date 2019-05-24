using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManage : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;
    private List<string> eventLog;
    [Tooltip("角色信息栏")]
    public GameObject playerStateLabel;
    [Tooltip("多用途菜单栏（文件夹样式）")]
    public GameObject menu;
    [Tooltip("与物品的交互范围半径")]
    public float operationRange = 0.5f;
    [HideInInspector]
    public GameObject available;
    [HideInInspector]
    public PlayerRole role;
    public float weight=55f;
    public float Hp;
    public float power;
    public float collectSpeed;
    public float moveSpeed = 1f;
    public enum PlayerRole
    {
        BUSINESSMAN,      //
        ENGINEER,       //
        SPY,       //
        INVESTIGATOR,       //侦察,点亮48*48区块
    }
    public State state;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        eventLog = new List<string>();
        state = new State() {
            hp = Hp,
            power = power,
            collectSpeed = collectSpeed,
            moveSpeed = moveSpeed,
            weight = weight,
        };

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR||UNITY_STANDALONE_WIN
        MoveOnWindows();
        Operate();
#endif

#if UNITY_ANDROID
     MoveOnAndroid();
#endif

#if UNITY_IOS
      Debug.Log("Iphone");
#endif
    }

    /// <summary>
    /// 
    /// </summary>
    private void MoveOnWindows()
    {
        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");
        Vector2 playerMove = new Vector2(H * moveSpeed, V * moveSpeed);
        m_Rigidbody2D.AddForce(playerMove);
    }

    private void Operate() {
        if (Input.GetKeyDown(KeyCode.E))//物品栏
        {
            AppManage.Instance.SetOpenUI(GameObject.FindGameObjectWithTag("Bag"));    
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (AppManage.Instance.openUI == null)
            {
                AppManage.Instance.SetOpenUI(GameObject.Find("EscMenu"));
            }
            else
            {
                CanvasGroup group = AppManage.Instance.openUI.GetComponent<CanvasGroup>();
                group.alpha = 0;
                group.interactable = false;
                group.blocksRaycasts = false;
                AppManage.Instance.openUI = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {

        }
        if (Input.GetKeyDown(KeyCode.R))
        {

        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AppManage.Instance.SetOpenUI(GameObject.Find("TechnologyTree"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AppManage.Instance.SetOpenUI(GameObject.Find("TechnologyTree"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AppManage.Instance.SetOpenUI(GameObject.Find("TechnologyTree"));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AppManage.Instance.SetOpenUI(GameObject.Find("TechnologyTree"));
        }

    }
    /// <summary>
    /// 接受外界赋予的状态
    /// </summary>
    public void Suffer(string log) {
        if (eventLog.Count>10)
        {
            eventLog.RemoveAt(0);
        }    
        eventLog.Add(log);
        if (state.hp<=0)
        {
            //todo gameover
        }
        
    }
    /// <summary>
    /// 延时状态
    /// </summary>
    /// <param name="func"></param>
    /// <param name="log"></param>
    /// <param name="delay"></param>
    public void Suffer(System.Action func, string log, float delay)
    {
        Suffer(log);
        StartCoroutine(DelayFunc(func,delay));
    }

    IEnumerator DelayFunc(System.Action func,float delay) {
        yield return new WaitForSeconds(delay);
        func.Invoke();
    }


    public void CreatePlayerInMap(Vector3 vector)
    {
        GameObject.Instantiate(gameObject).transform.position= vector;
    }

    public bool InOperationRange(Vector3 opv) {
        if ((opv - transform.position).sqrMagnitude < operationRange * operationRange)
        {
            return true;
        }
        return false;
    }
   
    // 碰撞开始
    void OnCollisionEnter(Collision collision)
    {
       
    }

    // 碰撞结束
    void OnCollisionExit(Collision collision)
    {

    }

    // 碰撞持续中
    void OnCollisionStay(Collision collision)
    {

    }

    // 开始接触
    void OnTriggerEnter(Collider collider)
    {
   
    }

    // 接触结束
    void OnTriggerExit(Collider collider)
    {

    }

    // 接触持续中
    void OnTriggerStay(Collider collider)
    {
     
    }

    private void MoveOnAndroid()
    {

    }

    public class State
    {
       public string other;
        public float hp;
        public float power;
        public float collectSpeed;
        public float moveSpeed;
        public float increaseOrDecrease;
    public float weight;

}

    public enum PlayerAnimState
    {
        /// <summary>
        /// 默认状态
        /// </summary>
        idle,
        /// <summary>
        /// 玩耍状态
        /// </summary>
        run,
        /// <summary>
        /// 张嘴状态
        /// </summary>
        use,
        /// <summary>
        /// 吞食状态
        /// </summary>
        eat,
        /// <summary>
        /// 失败状态
        /// </summary>
        fail,
    }


    /// <summary>
    /// 动画状态
    /// </summary>
    public PlayerAnimState animState;
    /// <summary>
    /// 动画控制器
    /// </summary>
    public Animator playerAnimator;

    /// <summary>
    /// 改变玩家状态方法
    /// </summary>
    /// <param name="state">玩家状态</param>
    public void ChangeState(PlayerAnimState state)
    {
        switch (state)
        {
            case PlayerAnimState.run:
                playerAnimator.SetBool("IsPlay", true);
                playerAnimator.SetBool("IsIdle", false);
                break;
            case PlayerAnimState.idle:
                playerAnimator.SetBool("IsIdle", true);
                playerAnimator.SetBool("IsOpen", false);
                playerAnimator.SetBool("IsPlay", false);
                break;
            case PlayerAnimState.use:
                playerAnimator.SetBool("IsOpen", true);
                playerAnimator.SetBool("IsIdle", false);
                break;
            case PlayerAnimState.eat:
                playerAnimator.SetBool("IsEat", true);
                playerAnimator.SetBool("IsOpen", false);
                break;
            case PlayerAnimState.fail:
                playerAnimator.SetBool("IsUnhappy", true);
                playerAnimator.SetBool("IsOpen", false);
                break;
            default:
                break;
        }
    }
}
