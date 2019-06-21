using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    public class Attacker : MonoBehaviour
    {
        public Equip equip;
        public AttackModel attackModel;
        public Vector3 equipAttackPosition;

        private const string rangName = "rang";
        private const string minRangName = "minRang";
        private const string closeCombatRangeName = "closeCombatRange";

        private HitBox target;
        private bool minRangShare;
        private bool isLock;
        private bool perFrameDetection;
        private bool ready;
        private IEnumerator attackLoop;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// 唤醒，启动所有监听,在下一次睡眠前只能唤醒一次
        /// </summary>
        public void Awaken(Equip equip_alpha)
        {
            equip = equip_alpha;
            if (equip.range!=0f)
            {
                CircleCollider2D rangeCollider2D = gameObject.AddComponent<CircleCollider2D>();
                rangeCollider2D.name = rangName;
                rangeCollider2D.isTrigger = true;
                rangeCollider2D.radius = equip.range;
            }         
            if (equip.minRange != 0f)
            {
                CircleCollider2D minRangeCollider2D = gameObject.AddComponent<CircleCollider2D>();
                minRangeCollider2D.name = minRangName;
                minRangeCollider2D.isTrigger = true;
                minRangeCollider2D.radius = equip.minRange;
            }
            if (equip.isCloseCombat)
            {
                if (equip.closeCombatRange != equip.minRange)
                {
                    CircleCollider2D closeCombatRangeCollider2D = gameObject.AddComponent<CircleCollider2D>();
                    closeCombatRangeCollider2D.name = closeCombatRangeName;
                    closeCombatRangeCollider2D.isTrigger = true;
                    closeCombatRangeCollider2D.radius = equip.closeCombatRange;
                }
                else
                {
                    minRangShare = true;
                }
            }//添加距离监听
            perFrameDetection = true;//第一次启动在下一帧刷新下模式
            if (equip.isAutoAttack)
            {
                attackLoop = AttackLoop();
                StartCoroutine(attackLoop);
            }       
        }
        /// <summary>
        /// 休眠，移除所有范围检测
        /// </summary>
        public void Sheep()
        {
            minRangShare = false;
            perFrameDetection = false;
            StopAllCoroutines();
            CircleCollider2D[] circles = gameObject.GetComponents<CircleCollider2D>();
            foreach (var c in circles)
            {
                if (c.name.Equals(rangName)|| c.name.Equals(minRangName) || c.name.Equals(closeCombatRangeName))
                {
                    Destroy(c);
                }
            }         
        }

        public void Attack() {
            if (ready)
            {
                Hit hit=new Hit();
                switch (attackModel)
                {
                    case AttackModel.longRange:
                        hit.sender = gameObject.name;
                        hit.hit = equip.attack;
                        hit.hitFrequency = equip.numberAttacks;
                        hit.hitType = equip.hitType;                      
                        break;
                    case AttackModel.closeCombat:
                        hit.sender = gameObject.name;
                        hit.hit = equip.closeCombatAttack;
                        hit.hitType = equip.hitType;                      
                        break;
                    case AttackModel.none:
                        return;                        
                }
                if (target!=null)
                {
                    switch (equip.shootType)
                    {
                        case ShootType.gun:
                            if (equip.muzzleFlame!=null)
                            {
                                GameObject b = Instantiate<GameObject>(equip.muzzleFlame, equipAttackPosition, Quaternion.identity);
                                Lifetimer.AddTimer(b,1.5f,true);
                            }
                            target.Recipient(hit);//即时的hit
                            break;
                        case ShootType.ballistic:
                            //todo 弹道发射hit
                            break;
                        case ShootType.closeCombat:
                            target.Recipient(hit);//即时的hit
                            break;                       
                    }
                   
                    ready = false;
                }               
               
            }
            
        }

        IEnumerator AttackLoop()
        {
            while (true)
            {              
                ready = true;
                if (equip.isAutoAttack)
                {
                    Attack();
                }
                if (attackModel == AttackModel.closeCombat)
                {
                    yield return new WaitForSeconds(equip.closeCombatAttackInterval);
                }
                else
                {
                    yield return new WaitForSeconds(equip.attackInterval);
                }               
            }
        }

      
        /// <summary>
        /// 锁定某个目标
        /// </summary>
        /// <param name="hitBox"></param>
        public void LockModel(HitBox hitBox)
        {
            target = hitBox;
            isLock = true;
        }

        public void UnlockModel()
        {
            isLock = false;
            perFrameDetection = true;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!isLock && perFrameDetection)
            {
                if (IsEnemy(collision.gameObject.tag))
                {
                    switch (collision.name)
                    {
                        case rangName:
                            attackModel = AttackModel.longRange;
                            FindTarget(collision.gameObject);
                            break;
                        case minRangName:
                            if (minRangShare)
                            {
                                attackModel = AttackModel.closeCombat;
                                FindTarget(collision.gameObject);
                            }
                            else
                            {
                                attackModel = AttackModel.none;
                            }
                            break;
                        case closeCombatRangeName:
                            attackModel = AttackModel.closeCombat;
                            FindTarget(collision.gameObject);
                            break;
                    }
                    perFrameDetection = false;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //if (isLock)
            //{
            //    return;
            //}
            if (IsEnemy(collision.gameObject.tag))
            {
                switch (collision.name)
                {
                    case rangName:
                        attackModel = AttackModel.longRange;
                        FindTarget(collision.gameObject);
                        break;
                    case minRangName:
                        if (minRangShare)
                        {
                            attackModel = AttackModel.closeCombat;
                            FindTarget(collision.gameObject);
                        }
                        else
                        {
                            attackModel = AttackModel.none;
                        }
                        break;
                    case closeCombatRangeName:
                        attackModel = AttackModel.closeCombat;
                        FindTarget(collision.gameObject);
                        break;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            //if (isLock)
            //{
            //    return;
            //}
            if (IsEnemy(collision.gameObject.tag))
            {
                switch (collision.name)
                {
                    case rangName:                    
                        attackModel = AttackModel.none;
                        FindNewTarget();
                        break;
                    case minRangName:
                        attackModel = AttackModel.longRange;
                        break;
                    case closeCombatRangeName:
                        if (minRangShare)
                        {
                            attackModel = AttackModel.longRange;
                        }
                        else
                        {
                            attackModel = AttackModel.none;
                            FindNewTarget();
                        }
                        break;
                }
            }
          
        }

        private bool IsEnemy(string colTag)
        {      
            switch (gameObject.tag)
            {
                case "Player":
                    if (colTag.Equals("Enemy"))
                    {
                        return true;
                    }
                    break;
                case "Enemy":
                    if (colTag.Equals("Player")||colTag.Equals("Build"))
                    {
                        return true;
                    }
                    break;
                case "NPC":
                    if (colTag.Equals("Enemy"))
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }
        /// <summary>
        /// 有新目标进入攻击范围，选取一个最近的攻击
        /// </summary>
        /// <param name="hitBox"></param>
        private void FindTarget(GameObject hitBox)
        {
            if (isLock)
            {
                return;
            }
            if (target==null)
            {
                target = hitBox.GetComponent<HitBox>();
            }
            else
            {
                float qt = Vector3.Distance(gameObject.transform.position, target.transform.position);
                float ht = Vector3.Distance(gameObject.transform.position, hitBox.transform.position);              
                if (ht<qt)
                {
                    target = hitBox.GetComponent<HitBox>();
                }
                
            }
            
        }
        /// <summary>
        /// 目标离开攻击范围,寻找一个新的
        /// </summary>
        /// <param name="hitBox"></param>
        private void FindNewTarget()
        {
            target = null;
            UnlockModel();

        }
        public enum AttackModel
        {
            longRange,
            closeCombat,
            none,

        }

      
    }

}
