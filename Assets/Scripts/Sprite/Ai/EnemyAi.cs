using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    private Transform player;

    public float attackDistance = 2;//这是攻击目标的距离，也是寻路的目标距离

    private Animator animator;

    public float speed;

    private CharacterController cc;

    public float attackTime = 3;   //设置定时器时间 3秒攻击一次

    private float attackCounter = 0; //计时器变量

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;

        cc = this.GetComponent<CharacterController>();

        animator = this.GetComponent<Animator>();//控制动画状态机中的奔跑动作调用

        attackCounter = attackTime;//一开始只要抵达目标立即攻击

    }

    void Update()
    {
        Vector3 targetPos = player.position;

        targetPos.y = transform.position.y;//此处的作用将自身的Y轴值赋值给目标Y值

        transform.LookAt(targetPos);//旋转的时候就保证已自己Y轴为轴心旋转

        float distance = Vector3.Distance(targetPos, transform.position);

        if (distance <= attackDistance)
        {
            attackCounter += Time.deltaTime;
            if (attackCounter > attackTime)//定时器功能实现
            {

                int num = Random.Range(0, 2);//攻击动画有两种，此处就利用随机数（【0】，【1】）切换两种动画

                if (num == 0) animator.SetTrigger("Attack1");

                else animator.SetTrigger("Attack2");



                attackCounter = 0;

            } else{
                animator.SetBool("Walk", false);
            }

        } else
        {
            attackCounter = attackTime;//每次移动到最小攻击距离时就会立即攻击

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("EnimyWalk"))//EnimyWalk是动画状态机中的行走的状态

                cc.SimpleMove(transform.forward * speed);

            animator.SetBool("Walk ", true);//移动的时候播放跑步动画

        }

    }
}
