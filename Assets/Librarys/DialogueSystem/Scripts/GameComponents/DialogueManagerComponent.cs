namespace DialogueManager
{
    using System.Collections;
    using System.Collections.Generic;
    using DialogueManager.Controllers;
    using DialogueManager.Models;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// 这个类管理对话中的文本、句子之间的转换、动画等等。 
    /// </summary>
    public class DialogueManagerComponent : MonoBehaviour
    {
        /// <summary> 对话管理器模型</summary>
        public DialogueManager Model;

        /// <summary> 对话管理器的控制器 </summary>
        private DialogueManagerController controller;
        private  GameObject dialogueBox;


        /// <summary>
        /// 在对象被实例化时被摘录
        /// </summary>
        private void Awake()
        {
            GameObject gameConversations = Instantiate( this.Model.GameConversationsPrefab );
            gameConversations.name = "GameConversations";

           
            Transform canvasObject = Model.Canvas ?? GameObject.Find("DialogueCanvas").GetComponent<Transform>();
            dialogueBox = Instantiate( this.Model.CanvasObjectsPrefab );
            //dialogueBox.transform.position = new Vector3( -250, 0, 0 );
            RectTransform boxRect = dialogueBox.GetComponent<RectTransform>();
            dialogueBox.name = "DialogueBox";
            boxRect.SetParent( canvasObject.transform,false);                    
            boxRect.localPosition = new Vector3( 0, -500, 0 );


            this.Model.DialogueStartPoint = GameObject.Find( "/"+canvasObject.name+"/DialogueBox/DialogueStartPoint" ).GetComponent<Transform>();
            this.Model.ImageText = GameObject.Find("/" + canvasObject.name + "/DialogueBox/CharacterImage").GetComponent<Image>();
            this.Model.Animator = GameObject.Find("/" + canvasObject.name + "/DialogueBox").GetComponent<Animator>();
            this.Model.Source = this.GetComponent<AudioSource>();

            this.controller = new DialogueManagerController( this.Model );
        }

        /// <summary>
        /// 检查模型中是否有要显示的内容以及是否有输入
        /// </summary>
        private void Update()
        {
            if ( this.Model.DialogueToShow != null )
            {
                this.StartDialogue();
            }

            if ( Input.GetKeyDown( this.Model.NextKey ) && this.Model.Finished && this.Model.DoubleTap )
            {
                this.DisplayNextSentence();
                this.Model.Finished = false;
            }

            if ( Input.GetKeyDown( this.Model.NextKey ) && this.Model.DoubleTap == false )
            {
                this.Model.Finished = true;
                this.DisplayNextSentence();
            }
            if (Model.IsAuto && this.Model.DoubleTap == false)
            {
                this.Model.Finished = true;
                this.DisplayNextSentence();
              
            }
        }

        /// <summary>
        ///启动新对话，并重置以前对话中的所有数据
        /// </summary>
        private void StartDialogue()
        {
            CanvasGroup group= dialogueBox.GetComponent<CanvasGroup>();
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;
            this.controller.StartDialogue();
            this.DisplayNextSentence();
        }

        /// <summary>
        /// 在对话中显示下一句话
        /// </summary>
        private void DisplayNextSentence()
        {
            this.StopAllCoroutines();
            if (this.controller.DisplayNextSentence())
            {
                this.StartCoroutine(this.controller.TypeSentence());
            }
            else {
                CanvasGroup group = dialogueBox.GetComponent<CanvasGroup>();
                group.alpha = 0;
                group.interactable = false;
                group.blocksRaycasts = false;
            }
        }
    }
}