namespace DialogueManager.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DialogueManager.GameComponents;
    using DialogueManager.Models;

    /// <summary>
    /// Controller for the GameConversations Component
    /// </summary>
    public class GameConversationsController
    {
        /// <summary>
        /// Model of the GameConversation
        /// </summary>
        private GameConversations model;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameConversationsController"/> class.
        /// </summary>
        /// <param name="gameConversations">Model of the GameConversations</param>
        public GameConversationsController( GameConversations gameConversations )
        {
            gameConversations.PendingConversations = new Dictionary<string, List<PendingStatus>>();
            gameConversations.ConversationsToAdd = new List<PendingStatus>();
            this.model = gameConversations;
        }

        /// <summary>
        /// 在挂起的会话上创建一个键，该键的名称为会话的名称（如果尚未存在）
        ///添加conversations中的第一个元素，使用正确的键添加到挂起的值conversations中，并对列表进行排序。
        /// </summary>
        public void AddConversation()
        {
            PendingStatus unlockedStatus = this.model.ConversationsToAdd[0];
            this.model.ConversationsToAdd.RemoveAt( 0 );
            Dictionary<string, List<PendingStatus>> conversations = this.model.PendingConversations;
            if (!conversations.ContainsKey( unlockedStatus.ConversationName ))
            {
                conversations[unlockedStatus.ConversationName] = new List<PendingStatus>();
            }

            List<PendingStatus> pending = conversations[unlockedStatus.ConversationName];
            PendingStatus match = pending.Where( status => status.ConversationName == unlockedStatus.StatusName ).FirstOrDefault();
            if (match == null)
            {
                pending.Add( unlockedStatus );
                pending.OrderBy( status => status.Importance );
            }
        }
    }
}