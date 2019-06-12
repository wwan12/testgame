namespace DialogueManager.GameComponents
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using DialogueManager.Controllers;
    using DialogueManager.Models;
    using UnityEngine;

    /// <summary>
    ///会话组件   必须为每个单独的NPC或有会话的情况添加会话组件 
    /// </summary>
    public class ConversationComponent : MonoBehaviour
    {
        /// <summary> Model of the Conversation </summary>
        public Conversation Model;

        /// <summary> Controller of the Conversation </summary>
        private ConversationController controller;

        /// <summary>
        /// 触发对话，显示活动状态。 
        /// </summary>
        public void Trigger()
        {
            this.Model.GameConversations = GameObject
                .Find( "GameConversations" )
                .GetComponent<GameConversationsComponent>()
                .Model;
            DialogueManager dialogueManager = GameObject
                .Find( "DialogueManager" )
                .GetComponent<DialogueManagerComponent>()
                .Model;
            this.controller.Trigger( dialogueManager );
        }

        /// <summary> Creation of the Controller </summary>
        private void Awake()
        {
            this.controller = new ConversationController( this.Model );
        }
    }
}