﻿using System.Collections;
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
#if UNITY_EDITOR||UNITY_STANDALONE_WIN
        MoveOnWindows();
        Use();
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

    private void Use() {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject.FindObjectOfType<BagManage>().gameObject.SetActive(true);
        }
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
        ArticlesAttachment articles = collider.gameObject.GetComponent<ArticlesAttachment>();
        if (articles == null)
        {
            return;
        }
        else
        {

        }
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
