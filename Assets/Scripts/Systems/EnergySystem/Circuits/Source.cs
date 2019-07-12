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

        public int residual;

        private void Start()
        {
            residual = outputPower;
        }

        protected override bool EvaluateState()
		{
			return !inverted;
		}

        public override void NeedPower(Relay relay)
        {
            if (residual>(relay.ratedPower-relay.nowPower))
            {
                residual -=(relay.ratedPower - relay.nowPower);
                relay.nowPower += (relay.ratedPower - relay.nowPower);
                relay.StopCoroutine(relay.scan);
            }
            else
            {
                relay.nowPower += residual;
                residual = 0;
            }
        }

        

    }
}
