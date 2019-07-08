using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class Ai : MonoBehaviour
    {
        private NavMeshAgent agent;
        private AiTaskControl AiTask;
        // Start is called before the first frame update
        /// <summary>
        /// 视野角度
        /// </summary>
        public int angle;
        /// <summary>
        /// 游走半径，移动状态下，如果超出游走半径会返回出生位置
        /// </summary>
        public float wanderRadius;
        /// <summary>
        /// 警戒半径，玩家进入后怪物会发出警告，并一直面朝玩家
        /// </summary>
        public float alertRadius;
        /// <summary>
        /// 自卫半径，玩家进入后怪物会追击玩家，当距离<攻击距离则会发动攻击（或者触发战斗）
        /// </summary>
        public float defendRadius;
        /// <summary>
        /// 追击半径，当怪物超出追击半径后会放弃追击，返回追击起始位置
        /// </summary>
        public float chaseRadius;            //
        /// <summary>
        /// 攻击距离 或者最小距离
        /// </summary>
        public float attackRange;
        /// <summary>
        /// 攻击速度
        /// </summary>
        public float attackSpeed; //
        /// <summary>
        /// 攻击力
        /// </summary>
        public float attack;
        /// <summary>
        /// 移动速度
        /// </summary>
        public float walkSpeed;          //
        /// <summary>
        /// 跑动速度
        /// </summary>
        public float runSpeed;          //
        /// <summary>
        /// 转身速度，建议0.1
        /// </summary>
        public float turnSpeed=0.1f;

        void Start()
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
            AiTask = gameObject.AddComponent<AiTaskControl>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool InVisualField(Vector3 opv, float distance)
        {
            if ((opv - transform.position).sqrMagnitude < distance * distance)
            {
                return true;
            }
            return false;
        }

        public void RandomEvent()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="opv"></param>
        public void GoToPosition(Vector3 opv)
        {
            agent.Move(opv);
        }

        private bool Find(GameObject player, float r)
        {
            Vector3 otherPos = player.gameObject.transform.position;
            Vector3 v = transform.position - otherPos;
            v.y = 0.5f; //处理一下y轴，因为判断建立在二维上
            Vector3 w = player.transform.position + transform.rotation * Vector3.forward * r - otherPos;
            w.y = 0.5f;
            if (Vector3.Angle(v, w) < angle / 2)
            {
                return true;
            }
            return false;
        }

        void BehaviorTree()
        {
            if (true)
            {

            }
        }


        public enum State
        {
            ready,
            fight,
            calm,
        }
    }
}

