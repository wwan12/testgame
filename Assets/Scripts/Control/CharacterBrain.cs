using Node.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBrain : MonoBehaviour
{
    /* Character */
    public BaseCharacter character { get; protected set; }

    /* Root node */
    public BrainNode brainNode { get; protected set; }

    /* Task runner */
    public TaskRunner taskRunner { get; protected set; }

    /* Data about the current task */
    public TaskData currentTaskData { get; protected set; }

    public CharacterBrain(BaseCharacter character, BrainNode brainNode)
    {
        this.character = character;
        this.brainNode = brainNode;
        this.brainNode.SetCharacter(character);
        this.taskRunner = new TaskRunner();
        this.currentTaskData = null;

        this.taskRunner.onEndTask = delegate {
            if (this.taskRunner.task.taskStatus == TaskStatus.Success)
            {
                ///Debug.Log("Clearing task (success)");
                this.currentTaskData = null;
            }
            else if (this.taskRunner.task.taskStatus == TaskStatus.Failed)
            {
                //Debug.Log("Clearing task (failed)");
                this.currentTaskData = null;
            }
        };
    }

    /// 检查我们是否有当前任务（至少是数据），如果没有，在树中获取下一个任务。
    public void Update()
    {
        if (this.currentTaskData == null)
        {
            this.GetNextTaskData();
        }
        else
        {
            if (this.taskRunner.running == false)
            {
                //Debug.Log("Starting new task: "+this.currentTaskData.def.uid);
                this.taskRunner.StartTask(this.currentTaskData);
            }
            else
            {
                if (this.taskRunner.task.taskStatus == TaskStatus.Running)
                {
                    this.taskRunner.task.Update();
                }
            }
        }
    }

    /// Get the next task data in tree.
    public void GetNextTaskData()
    {
        TaskData nextTaskData = this.brainNode.GetTaskData();
        if (nextTaskData != null)
        {
            this.currentTaskData = nextTaskData;
        }
    }
}
