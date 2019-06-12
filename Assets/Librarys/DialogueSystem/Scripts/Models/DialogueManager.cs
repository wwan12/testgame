namespace DialogueManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Model of the main Dialogue Manager
    /// </summary>
    [Serializable]
    public class DialogueManager
    {
        /// <summary> 对话框、文本和图像的前言 . </summary>
        public GameObject CanvasObjectsPrefab;

        /// <summary> 游戏对话的序言。 </summary>
        public GameObject GameConversationsPrefab;

        /// <summary> 每个字母之间的时间。  </summary>
        public float WaitTime = .01f;

        /// <summary> 字符声音的音量。  </summary>
        public float VoiceVolume = 1f;

        /// <summary> 是双击 . </summary>
        public bool DoubleTap = true;

        /// <summary> 必须按下键才能继续下一句话 . </summary>
        public string NextKey = "z";

        /// <summary> 字体 </summary>
        public Font Font;

        /// <summary> FontMaterial </summary>
        public Material Material;

        /// <summary> Gets or sets the Text that is being displayed on the Scene. </summary>
        public Transform DialogueStartPoint { get; set; }

        /// <summary> Gets or sets the Image that is being displayed on the Scene. </summary>
        public Image ImageText { get; set; }

        /// <summary> Gets or sets the Animation that causes the Dialogue box to go up or down. </summary>
        public Animator Animator { get; set; }

        /// <summary> Gets or sets the Audio that the current dialogue is showing. </summary>
        public AudioSource Source { get; set; }

        /// <summary> Gets or sets a value indicating whether the Dialogue has finished or not. </summary>
        public bool Finished { get; set; }

        /// <summary> Gets or sets the <see cref="Dialogue"/> that will be displayed. </summary>
        public Dialogue DialogueToShow { get; set; }
    }
}
