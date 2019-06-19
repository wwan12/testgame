namespace DialogueManager.InspectorEditors
{
    using UnityEngine;
    using System.Collections;
    using UnityEditor;
    using DialogueManager.GameComponents;

    [CustomEditor( typeof( DialogueManagerComponent ) )]
    public class DialogueManagerEditor : Editor
    {
       // DialogueManagerComponent m_Target;
        private SerializedProperty gameConversationsProperty;
        private SerializedProperty canvasObjectsProperty;
        private SerializedProperty waitTimeProperty;
        private SerializedProperty voiceVolumeProperty;
        private SerializedProperty doubleTapProperty;
        private SerializedProperty nextKeyProperty;
        private SerializedProperty fontProperty;
        private SerializedProperty materialProperty;
        private SerializedProperty auto;
        private SerializedProperty canvas;
        void OnEnable()
        {
            gameConversationsProperty = serializedObject.FindProperty("Model.GameConversationsPrefab");
            canvasObjectsProperty = serializedObject.FindProperty( "Model.CanvasObjectsPrefab" );
            waitTimeProperty = serializedObject.FindProperty( "Model.WaitTime" );
            voiceVolumeProperty = serializedObject.FindProperty( "Model.VoiceVolume" );
            doubleTapProperty = serializedObject.FindProperty( "Model.DoubleTap" );
            nextKeyProperty = serializedObject.FindProperty( "Model.NextKey" );
            fontProperty = serializedObject.FindProperty( "Model.Font" );
            materialProperty = serializedObject.FindProperty( "Model.Material" );
            auto = serializedObject.FindProperty("Model.IsAuto");
            canvas = serializedObject.FindProperty("Model.Canvas");
        }

        public override void OnInspectorGUI()
        {
          //  m_Target = target as DialogueManagerComponent;
          //  m_Target.Model.Canvas = EditorGUILayout.ObjectField("canvas", m_Target.Model.Canvas, typeof(RectTransform), true) as RectTransform;
          //  m_Target.Model.IsAuto = EditorGUILayout.Toggle("auto", m_Target.Model.IsAuto);
            serializedObject.Update();
            EditorGUILayout.PropertyField(canvas, false);
            EditorGUILayout.PropertyField(gameConversationsProperty, false);
            EditorGUILayout.PropertyField( canvasObjectsProperty, false );
            EditorGUILayout.PropertyField( waitTimeProperty, true );
            EditorGUILayout.PropertyField( voiceVolumeProperty, true );
            EditorGUILayout.PropertyField( doubleTapProperty, true );
            EditorGUILayout.PropertyField( nextKeyProperty, true );
            EditorGUILayout.PropertyField( fontProperty, false );
            EditorGUILayout.PropertyField( materialProperty, false );
          //  EditorGUILayout.PropertyField(auto, false);
            serializedObject.ApplyModifiedProperties();
        }     
    }
}