namespace DialogueManager.Controllers
{
    using System.Linq;
    using DialogueManager.Models;

    /// <summary>
    /// 会话组件的控制器
    /// </summary>
    public class ConversationController
    {
        /// <summary>对话的模型</summary>
        private Conversation model;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationController"/> class.
        /// </summary>
        /// <param name="conversation">Model of the Conversation</param>
        public ConversationController(Conversation conversation)
        {
            conversation.ActiveStatus = conversation.Status[conversation.ActiveStatusIndex];
            this.model = conversation;
        }

        /// <summary>
        /// 触发对话，检查是否存在未锁定的对话状态并触发正确的状态
        /// </summary>
        /// <param name="dialogueManager">显示对话的对话管理器</param>
        public void Trigger(DialogueManager dialogueManager)
        {
            var conversations = this.model.GameConversations.PendingConversations;
            if (conversations.ContainsKey( this.model.Name ) && conversations[this.model.Name].Count > 0)
            {
                var statusList = conversations[this.model.Name];
                string statusName = statusList[0].StatusName;
                statusList.RemoveAt( 0 );

                this.model.ActiveStatus = this
                    .model
                    .Status
                    .Where( status => status.Name.Equals( statusName ) )
                    .First();

                this.model.ActiveStatusIndex = this
                    .model
                    .Status
                    .IndexOf( this.model.ActiveStatus );
            }

            if (this.model.ActiveStatus != null)
            {
                this.TriggerStatus(dialogueManager);
            }
        }

        /// <summary>
        ///触发ActiveStatus并将其更改为NextStatus
        /// </summary>
        /// <param name="dialogueManager">显示对话的对话管理器</param>
        private void TriggerStatus(DialogueManager dialogueManager)
        {
            ConversationStatus status = this.model.ActiveStatus;
            this.model.GameConversations.ConversationsToAdd.AddRange( status.NewConversations );
            dialogueManager.DialogueToShow = status.Dialogue;
            this.model.ActiveStatus = status.NextStatus;
            this.model.ActiveStatusIndex = status.NextStatusIndex;
        }
    }
}
