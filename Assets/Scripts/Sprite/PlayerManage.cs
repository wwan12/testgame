using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManage : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;
    public float speed = 1f;
    [Tooltip("与物品的交互范围半径")]
    public float operationRange = 0.5f;
    [HideInInspector]
    public GameObject available;
   
    public float weight=1.5f;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        m_Rigidbody2D.gravityScale = weight;      
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            AppManage.Instance.openUI = GameObject.Find("Bag");
            CanvasGroup group = AppManage.Instance.openUI.GetComponent<CanvasGroup>();
            group.alpha = group.alpha == 1 ? 0 : 1;
            group.interactable = group.interactable ? false : true;
            group.blocksRaycasts = group.blocksRaycasts ? false : true;
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
        //ArticlesAttachment articles = collider.gameObject.GetComponent<ArticlesAttachment>();
        //if (articles == null)
        //{
        //    return;
        //}
        //else
        //{
        //    foreach (var pre in articles.prefix)
        //    {
        //        if (pre== ArticlesAttachment.InteractiveType.PLAYER)
        //        {
        //            available = collider.gameObject;
        //        }
        //    }

        //}
    }

    // 接触结束
    void OnTriggerExit(Collider collider)
    {
        //if (collider.gameObject.GetInstanceID()==available.GetInstanceID())
        //{
        //    available = null;
        //}
    }

    // 接触持续中
    void OnTriggerStay(Collider collider)
    {
     
    }

    private void MoveOnAndroid()
    {

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
