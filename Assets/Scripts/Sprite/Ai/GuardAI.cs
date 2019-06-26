﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    /// <summary>
    /// 这是一个守卫型的AI
    /// </summary>
    public class GuardAI : MonoBehaviour
    {
        public float visualField = 10f;

        private GameObject playerUnit;          //获取玩家单位
        private Animator thisAnimator;          //自身动画组件
        private Vector3 initialPosition;            //初始位置
        [Tooltip("游走半径，移动状态下，如果超出游走半径会返回出生位置")]
        public float wanderRadius;
        [Tooltip("警戒半径，玩家进入后怪物会发出警告，并一直面朝玩家")]//
        public float alertRadius;
        [Tooltip("自卫半径，玩家进入后怪物会追击玩家，当距离<攻击距离则会发动攻击（或者触发战斗）")]//
        public float defendRadius;
        [Tooltip("追击半径，当怪物超出追击半径后会放弃追击，返回追击起始位置")]////
        public float chaseRadius;            //
        [Tooltip("攻击距离")]
        public float attackRange;
        [Tooltip("攻击速度")]
        public float attackSpeed; //
        [Tooltip("攻击力")]
        public float attack;
        [Tooltip("移动速度")]
        public float walkSpeed;          //
        [Tooltip("跑动速度")]
        public float runSpeed;          //
        [Tooltip("转身速度，建议0.1")]
        public float turnSpeed;         //

        private enum MonsterState
        {
            STAND,      //原地呼吸
            CHECK,       //原地观察
            WALK,       //移动
            WARN,       //盯着玩家
            CHASE,      //追击玩家
            RETURN      //超出追击范围后返回
        }
        private MonsterState currentState = MonsterState.STAND;          //默认状态为原地呼吸
        [Tooltip("设置待机时各种动作的权重，顺序依次为呼吸、观察、移动")]
        public float[] actionWeight = { 3000, 3000, 4000 };         //
        [Tooltip("更换待机指令的间隔时间")]
        public float actRestTme;            //
        private float lastActTime;          //最近一次指令时间
        private float diatanceToPlayer;         //怪物与玩家的距离
        private float diatanceToInitial;         //怪物与初始位置的距离
        private Quaternion targetRotation;         //怪物的目标朝向

        private bool is_Warned = false;
        private bool is_Running = false;

        void Start()
        {
            playerUnit = GameObject.FindGameObjectWithTag("Player");
            thisAnimator = GetComponent<Animator>();

            //保存初始位置信息
            initialPosition = gameObject.GetComponent<Transform>().position;

            //检查并修正怪物设置
            //1. 自卫半径不大于警戒半径，否则就无法触发警戒状态，直接开始追击了
            defendRadius = Mathf.Min(alertRadius, defendRadius);
            //2. 攻击距离不大于自卫半径，否则就无法触发追击状态，直接开始战斗了
            attackRange = Mathf.Min(defendRadius, attackRange);
            //3. 游走半径不大于追击半径，否则怪物可能刚刚开始追击就返回出生点
            wanderRadius = Mathf.Min(chaseRadius, wanderRadius);

            //随机一个待机动作
            RandomAction();
        }

        /// <summary>
        /// 根据权重随机待机指令
        /// </summary>
        void RandomAction()
        {
            //更新行动时间
            lastActTime = Time.time;
            //根据权重随机
            float number = Random.Range(0, actionWeight[0] + actionWeight[1] + actionWeight[2]);
            if (number <= actionWeight[0])
            {
                currentState = MonsterState.STAND;
                thisAnimator.SetTrigger("Stand");
            }
            else if (actionWeight[0] < number && number <= actionWeight[0] + actionWeight[1])
            {
                currentState = MonsterState.CHECK;
                thisAnimator.SetTrigger("Check");
            }
            if (actionWeight[0] + actionWeight[1] < number && number <= actionWeight[0] + actionWeight[1] + actionWeight[2])
            {
                currentState = MonsterState.WALK;
                //随机一个朝向
                targetRotation = Quaternion.Euler(0, Random.Range(1, 5) * 90, 0);
                thisAnimator.SetTrigger("Walk");
            }
        }

        void Update()
        {
            switch (currentState)
            {
                //待机状态，等待actRestTme后重新随机指令
                case MonsterState.STAND:
                    if (Time.time - lastActTime > actRestTme)
                    {
                        RandomAction();         //随机切换指令
                    }
                    //该状态下的检测指令
                    EnemyDistanceCheck();
                    break;

                //待机状态，由于观察动画时间较长，并希望动画完整播放，故等待时间是根据一个完整动画的播放长度，而不是指令间隔时间
                case MonsterState.CHECK:
                    if (Time.time - lastActTime > thisAnimator.GetCurrentAnimatorStateInfo(0).length)
                    {
                        RandomAction();         //随机切换指令
                    }
                    //该状态下的检测指令
                    EnemyDistanceCheck();
                    break;

                //游走，根据状态随机时生成的目标位置修改朝向，并向前移动
                case MonsterState.WALK:
                    transform.Translate(Vector3.forward * Time.deltaTime * walkSpeed);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);

                    if (Time.time - lastActTime > actRestTme)
                    {
                        RandomAction();         //随机切换指令
                    }
                    //该状态下的检测指令
                    WanderRadiusCheck();
                    break;

                //警戒状态，播放一次警告动画和声音，并持续朝向玩家位置
                case MonsterState.WARN:
                    if (!is_Warned)
                    {
                        thisAnimator.SetTrigger("Warn");
                        gameObject.GetComponent<AudioSource>().Play();
                        is_Warned = true;
                    }
                    //持续朝向玩家位置
                    targetRotation = Quaternion.LookRotation(playerUnit.transform.position - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
                    //该状态下的检测指令
                    WarningCheck();
                    break;

                //追击状态，朝着玩家跑去
                case MonsterState.CHASE:
                    if (!is_Running)
                    {
                        thisAnimator.SetTrigger("Run");
                        is_Running = true;
                    }
                    transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
                    //朝向玩家位置
                    targetRotation = Quaternion.LookRotation(playerUnit.transform.position - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
                    //该状态下的检测指令
                    ChaseRadiusCheck();
                    break;

                //返回状态，超出追击范围后返回出生位置
                case MonsterState.RETURN:
                    //朝向初始位置移动
                    targetRotation = Quaternion.LookRotation(initialPosition - transform.position, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed);
                    transform.Translate(Vector3.forward * Time.deltaTime * runSpeed);
                    //该状态下的检测指令
                    ReturnCheck();
                    break;
            }
        }

        /// <summary>
        /// 原地呼吸、观察状态的检测
        /// </summary>
        void EnemyDistanceCheck()
        {
            diatanceToPlayer = Vector3.Distance(playerUnit.transform.position, transform.position);
            if (diatanceToPlayer < attackRange)
            {
              //  playerUnit.GetComponent<PlayerManage>().Suffer("");
                //进入战斗状态
            }
            else if (diatanceToPlayer < defendRadius)
            {
                currentState = MonsterState.CHASE;
            }
            else if (diatanceToPlayer < alertRadius)
            {
                currentState = MonsterState.WARN;
            }
        }

        /// <summary>
        /// 警告状态下的检测，用于启动追击及取消警戒状态
        /// </summary>
        void WarningCheck()
        {
            diatanceToPlayer = Vector3.Distance(playerUnit.transform.position, transform.position);
            if (diatanceToPlayer < defendRadius)
            {
                is_Warned = false;
                currentState = MonsterState.CHASE;
            }

            if (diatanceToPlayer > alertRadius)
            {
                is_Warned = false;
                RandomAction();
            }
        }

        /// <summary>
        /// 游走状态检测，检测敌人距离及游走是否越界
        /// </summary>
        void WanderRadiusCheck()
        {
            diatanceToPlayer = Vector3.Distance(playerUnit.transform.position, transform.position);
            diatanceToInitial = Vector3.Distance(transform.position, initialPosition);

            if (diatanceToPlayer < attackRange)
            {
                //进入战斗状态
            }
            else if (diatanceToPlayer < defendRadius)
            {
                currentState = MonsterState.CHASE;
            }
            else if (diatanceToPlayer < alertRadius)
            {
                currentState = MonsterState.WARN;
            }

            if (diatanceToInitial > wanderRadius)
            {
                //朝向调整为初始方向
                targetRotation = Quaternion.LookRotation(initialPosition - transform.position, Vector3.up);
            }
        }

        /// <summary>
        /// 追击状态检测，检测敌人是否进入攻击范围以及是否离开警戒范围
        /// </summary>
        void ChaseRadiusCheck()
        {
            diatanceToPlayer = Vector3.Distance(playerUnit.transform.position, transform.position);
            diatanceToInitial = Vector3.Distance(transform.position, initialPosition);

            if (diatanceToPlayer < attackRange)
            {
                //进入战斗状态
            }
            //如果超出追击范围或者敌人的距离超出警戒距离就返回
            if (diatanceToInitial > chaseRadius || diatanceToPlayer > alertRadius)
            {
                currentState = MonsterState.RETURN;
            }
        }

        /// <summary>
        /// 超出追击半径，返回状态的检测，不再检测敌人距离
        /// </summary>
        void ReturnCheck()
        {
            diatanceToInitial = Vector3.Distance(transform.position, initialPosition);
            //如果已经接近初始位置，则随机一个待机状态
            if (diatanceToInitial < 0.5f)
            {
                is_Running = false;
                RandomAction();
            }
        }
    }
}

