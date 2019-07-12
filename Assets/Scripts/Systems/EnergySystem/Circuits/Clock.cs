using UnityEngine;

namespace Circuits
{
    /// <summary>
    /// 脉冲器
    /// </summary>
	public class Clock : CircuitNode
	{/// <summary>
     /// 脉冲间隔
     /// </summary>
        [SerializeField]
		private float pulseInterval = 0.9f;
        /// <summary>
        /// 脉冲长度
        /// </summary>
		[SerializeField]
		private float pulseLength = 0.1f;

		private float currentDuration;

		private bool pulsing;
    
        private void Awake()
		{
			pulsing = !inverted;
			currentDuration = pulsing ? pulseLength : pulseInterval;
		}

      

        protected override bool EvaluateState()
		{
			currentDuration -= Time.inFixedTimeStep ? Time.fixedDeltaTime : Time.deltaTime;
			if(currentDuration <= 0f)
			{
				pulsing = !pulsing;
				currentDuration = pulsing ? pulseLength : pulseInterval;
			}
			return inverted ? !pulsing : pulsing;
		}

       
    }
}
