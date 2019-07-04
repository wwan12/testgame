using System;
using UnityEngine;
namespace BattleSystem
{
    public class HitBox : MonoBehaviour
    {
        public float hp;

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
            hp -= hit.hit;
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
