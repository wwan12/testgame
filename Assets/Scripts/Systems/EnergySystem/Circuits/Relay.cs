using System.Collections;
using UnityEngine;

namespace Circuits
{/// <summary>
/// 继电器
/// </summary>
	public class Relay : SingleDependancyNode
	{
		[SerializeField]
		private float delay = 0f;

        [Tooltip("额定功率")]
        public int ratedPower;
        [Tooltip("消耗/得到的功率")]
        public int nowPower;
        [Tooltip("运行速度")]
        public float speed;
        public bool HasNode()
		{
			return node != null;
		}
        /// <summary>
        /// 评估状态
        /// </summary>
        /// <returns></returns>
		protected override bool EvaluateState()
		{
          //  Debug.LogWarning("EvaluateState");
			if (node != null && node.IsPowered() != IsPowered())
			{
				if (delay <= 0f)
				{
					return inverted ? !node.IsPowered() : node.IsPowered();
				}
				else
				{
					StartCoroutine(DelayedPowerChange(node.IsPowered()));
				}
			}
			return IsPowered();
		}

        protected override int InputPower(int w)
        {
            if (IsPowered())
            {
                w -= ratedPower;
            }
            return w;

        }

        private IEnumerator DelayedPowerChange(bool value)
		{
           // Debug.LogWarning("DelayedPowerChange");
            yield return new WaitForSeconds(delay);
			SetPowered(value);
		}
	}
}
