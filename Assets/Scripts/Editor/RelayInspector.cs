using UnityEngine;
using UnityEditor;

namespace Circuits.Editor
{
	[CustomEditor(typeof(Relay))]
	public class RelayInspector : UnityEditor.Editor
	{
		private Relay relay;

		private void OnEnable()
		{
			relay = (Relay)target;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (!relay.HasNode())
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("powered"));
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}