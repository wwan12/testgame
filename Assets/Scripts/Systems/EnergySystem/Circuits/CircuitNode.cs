using UnityEngine;
using UnityEngine.Events;

namespace Circuits
{
	[ExecuteInEditMode, DisallowMultipleComponent]
	public abstract class CircuitNode : MonoBehaviour
	{
		[SerializeField, HideInInspector]
		private bool powered = true;

		[SerializeField]
		protected bool inverted = false;

		private bool nextActivatedValue;

		public virtual bool IsPowered()
		{
			return powered;
		}

		public virtual void SetPowered(bool value)
		{
			nextActivatedValue = value;
		}

		protected abstract bool EvaluateState();

        public virtual void NeedPower(Relay relay) {

        }

        private void FixedUpdate()
		{
            
			nextActivatedValue = EvaluateState();
		}

		private void LateUpdate()
		{
			powered = nextActivatedValue;
		}

#if UNITY_EDITOR
		protected virtual void OnDrawGizmos()
		{
			Gizmos.color = IsPowered() ? Color.green : Color.gray;
			Gizmos.DrawWireSphere(transform.position, 0.9f);
		}
#endif
	}
}
