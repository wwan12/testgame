using System;
using UnityEngine;
namespace BattleSystem
{
    public class HitBox : MonoBehaviour
    {
        public float hp;

        public float defense=0;

        public float coverage=1;

        public float minDefense=0;

        private UnityEngine.Random r;
        public event EventHandler<Hit> DeadCallBack;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Recipient(Hit hit)
        {
            Hit(hit);
        }

        private void Hit(Hit hit)
        {
            if (hit.hit < minDefense)
            {
                return;
            }
            //    y =\left(x - m\right)\left(1 - z\right)\left(1 - c\right)\left\{ x > 0\right\}
            float chp = (hit.hit - minDefense) * UnityEngine.Random.Range(0, 1) < coverage ? (1 - defense) : 1;           
            hp -= chp;
            if (hp <= 0)
            {
                if (DeadCallBack == null)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DeadCallBack(this, hit);
                    
                }
                Messenger.Broadcast(EventCode.ENEMY_DEAD,gameObject.name);
            }
        }
    }

}
