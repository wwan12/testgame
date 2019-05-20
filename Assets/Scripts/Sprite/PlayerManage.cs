using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManage : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;
    private string eventLog;
    public GameObject player;
    public GameObject playerStateLabel;
    public float speed = 1f;
    [Tooltip("与物品的交互范围半径")]
    public float operationRange = 0.5f;
    [HideInInspector]
    public GameObject available;
    [HideInInspector]
    public PlayerRole role;
    public float weight=55f;
    public int Hp;
    public int power;
    public float collectSpeed;
    public enum PlayerRole
    {
        BUSINESSMAN,      //
        ENGINEER,       //
        SPY,       //
        INVESTIGATOR,       //侦察,点亮48*48区块
    }
    //Technology tree
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        
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
        Vector2 playerMove = new Vector2(H * speed, V * speed);
        m_Rigidbody2D.AddForce(playerMove);
    }

    private void Operate() {
        if (Input.GetKeyDown(KeyCode.E))//物品栏
        {
            AppManage.Instance.SetOpenUI(GameObject.Find("Bag"));    
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (AppManage.Instance.openUI == null)
            {
                AppManage.Instance.SetOpenUI(GameObject.Find("SelectInStar"));
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            AppManage.Instance.SetOpenUI(GameObject.Find("TechnologyTree"));
        }

    }
    /// <summary>
    /// 接受外界赋予的状态
    /// </summary>
    public void Suffer(State state,float value,string note) {
        switch (state)
        {
            case State.hp:
                break;
            case State.power:
                break;
            case State.collectSpeed:
                break;
            case State.moveSpeed:
                break;
            case State.increaseOrDecrease:
                break;
            default:
                break;
        }
        eventLog += note;
        note = "<" + note + ">";
        playerStateLabel.GetComponent<Text>().text += note;

    }

    public void CreatePlayerInMap(Vector3 vector)
    {
        GameObject.Instantiate(player).transform.position= vector;
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

    public enum State
    {
        other,
        hp,
        power,
        collectSpeed,
        moveSpeed,
        increaseOrDecrease,
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
    public PlayerAnimState playerState;
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
