using DialogueManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 简单的对话系统使用预设
/// </summary>
public class DialoguePrefab : MonoBehaviour
{
    [Tooltip("触发方式")]
    public TriggerType triggerType;
    [Tooltip("触发方式为接触时被碰撞的对象")]
    public GameObject Tracked;
    private bool wasTriggered = false;
    ConversationComponent conversation;
    private Transform tPosition;
    // Start is called before the first frame update
    void Start()
    {
        conversation =gameObject.GetComponent<ConversationComponent>();
        name = conversation.Model.Name;
        if (triggerType==TriggerType.aside)
        {
            Messenger.AddListener(name, Play);
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        if (triggerType==TriggerType.tracked)
        {
            Track();
        }
    }
    /// <summary>
    /// 开始播放对话
    /// </summary>
    /// <param name="i"></param>
    public void Play() {
        conversation.Trigger();
    }

    private void OnMouseOver()
    {
        if (triggerType == TriggerType.mouse_left)
        {
            if (Input.GetMouseButton(1))
            {
                conversation.Trigger();

            }
        }
        if (triggerType == TriggerType.mouse_right)
        {
            if (Input.GetMouseButton(2))
            {

                conversation.Trigger();
            }
        }

    }

    void Track()
    {
        if (tPosition.position.x < this.transform.position.x &&
                tPosition.position.y > this.transform.position.y)
        {

            if (!wasTriggered)
            {
                wasTriggered = true;
                if (conversation != null)
                {
                    conversation.Trigger();
                }
            }
        }
        else if (wasTriggered)
        {
            wasTriggered = false;
        }
    }

    public enum TriggerType
    {
        mouse_left,
        mouse_right,
        tracked,
        aside,
    }
}
