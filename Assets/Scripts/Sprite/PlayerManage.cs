using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManage : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        AppManage.Instance.LogWrap(">>" + Application.platform.ToString());
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        MoveOnWindows();
#endif

#if UNITY_ANDROID
     MoveOnAndroid();
#endif

#if UNITY_IOS
      Debug.Log("Iphone");
#endif

#if UNITY_STANDALONE_WIN
        MoveOnWindows();
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
        //if (Input.GetKey(KeyCode.W))
        //{
        //    this.gameObject.transform.Translate(Vector3.forward*speed * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    this.gameObject.transform.Translate(Vector3.down * speed * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    this.gameObject.transform.Translate(Vector3.left * speed * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    this.gameObject.transform.Translate(Vector3.right * speed * Time.deltaTime);
        //}        
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
        open,
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
            case PlayerAnimState.open:
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
