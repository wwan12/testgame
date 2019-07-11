using UnityEngine;

namespace Circuits
{
   /// <summary>
   /// 电源
   /// </summary>
	public class Source : CircuitNode
	{
        [Tooltip("输出功率")]
        public int outputPower;

        protected override bool EvaluateState()
		{
			return !inverted;
		}

        protected override int InputPower(int w)
        {
            w += outputPower;
            return w;
        }

        

    }
}
