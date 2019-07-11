using UnityEngine;

namespace Circuits
{
	public class OrGate : MultiDependancyNode
	{
		protected override bool EvaluateState()
		{
			if (nodes == null)
				return inverted ? false : true;

			for(int i = 0; i < nodes.Count; i++)
			{
				if(nodes[i] != null && nodes[i].IsPowered())
				{
					return inverted ? false : true;
				}
			}
			return inverted ? true : false;
		}

        protected override int InputPower(int w)
        {
            throw new System.NotImplementedException();
        }
    }
}
