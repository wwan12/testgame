using UnityEngine;

namespace Circuits
{/// <summary>
/// 加转接头
/// </summary>
	public class AndGate : MultiDependancyNode
	{
		protected override bool EvaluateState()
		{
			if (nodes == null)
				return inverted ? true : false;

			for(int i = 0; i < nodes.Count; i++)
			{
				if(nodes[i] != null && !nodes[i].IsPowered())
				{
					return inverted ? true : false;
				}
			}
			return inverted ? false : true;
		}

       
    }
}
