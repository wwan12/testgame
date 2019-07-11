using System.Collections.Generic;
using UnityEngine;

namespace Circuits
{
	public abstract class MultiDependancyNode : CircuitNode
	{
        public List<CircuitNode> nodes = new List<CircuitNode>();
       // public  CircuitNode[] initNode = new CircuitNode[1];

        private void Start()
        {
          
        }
#if UNITY_EDITOR
        protected override void OnDrawGizmos()
		{
			base.OnDrawGizmos();
			if (nodes != null)
			{
				Gizmos.color = Color.white;
				for (int i = 0; i < nodes.Count; i++)
				{
					if (nodes[i] != null)
					{
						Vector3 dir = transform.position - nodes[i].transform.position;
						Gizmos.DrawLine(transform.position, dir.normalized + nodes[i].transform.position);
					}
				}
			}
		}
#endif
	}
}
