using UnityEngine;

namespace Circuits
{
    /// <summary>
    /// 异或门
    /// </summary>
	public class XorGate : MultiDependancyNode
	{
        /// <summary>
        /// 范围指示
        /// </summary>
        private SpriteRenderer rangeDis;
        /// <summary>
        /// 可连接范围
        /// </summary>
        public float connectionScope = 7f;
        /// <summary>
        /// 有效范围
        /// </summary>
        public float effectiveScope=3f;
        private void Start()
        {
           // Debug.LogWarning(gameObject.layer);

            rangeDis = Resources.Load<GameObject>("prefabs/Sprite/RangeDisplay").GetComponent<SpriteRenderer>();
            rangeDis.size = new Vector2(connectionScope, connectionScope);
            rangeDis.transform.position = gameObject.transform.position;
            rangeDis = GameObject.Instantiate(rangeDis);
            ScopeControl s= rangeDis.gameObject.AddComponent<ScopeControl>();
            s.gate = this;
            s.connectionScope = connectionScope;
            rangeDis.transform.SetParent(gameObject.transform);
            rangeDis.enabled = false;

          
            CircleCollider2D effectiveCollider = gameObject.AddComponent<CircleCollider2D>();
            //effectiveCollider.name = EnergyManage.TRANSFER_NAME;
            effectiveCollider.radius = effectiveScope;
            effectiveCollider.isTrigger = true;
        }

        protected override bool EvaluateState()
		{
			if (nodes == null)
				return inverted ? false : true;

			bool found = false;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i] != null && nodes[i].IsPowered())
				{
					if(found)
					{
						return inverted ? true : false;
					}
					found = true;
				}
			}
			return inverted ? !found : found;
		}

        protected override int InputPower()
        {
            return w;
        }

        private void OnTriggerEnter2D(Collider2D collision)//将范围内发用点加入node
        {
            if (collision.gameObject.layer==AppManage.Instance.BuildLayer)//图层为build
            {
                //if (collision.gameObject.GetComponent<Relay>()!=null)
                //{
                //    nodes.Add(collision.gameObject.GetComponent<Relay>());
                //}
                if (collision.gameObject.GetComponent<Source>() != null)
                {
                    nodes.Add(collision.gameObject.GetComponent<Source>());
                }
                Relay relay = collision.gameObject.GetComponent<Relay>();
                if (relay != null)
                {
                    relay.node=this ;
                }
                //  collision.gameObject.GetComponent<BuildControl>().TryConnect();
            }
            
                                      
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == AppManage.Instance.BuildLayer)//图层为build
            {
                //if (collision.gameObject.GetComponent<Relay>() != null)
                //{
                //    nodes.Remove(collision.gameObject.GetComponent<Relay>());
                //}
                if (collision.gameObject.GetComponent<Source>() != null)
                {
                    nodes.Remove(collision.gameObject.GetComponent<Source>());
                }
                Relay relay = collision.gameObject.GetComponent<Relay>();
                if (relay != null)
                {
                    relay.node = null;
                }
            }


        }

     
    }
}
