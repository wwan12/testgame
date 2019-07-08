using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class Task
    {
        //任务名
        public string taskName;
        /// <summary>
        /// 等待超时
        /// </summary>
        public float waitTime=3f;
        /// <summary>
        /// 主体
        /// </summary>
        public IEnumerator task;
        /// <summary>
        /// 任务完成条件
        /// </summary>
        public TaskComplete taskComplete;
        /// <summary>
        /// 任务完成结果
        /// </summary>
        public TaskResult taskResult;

        public Task(IEnumerator task,TaskComplete taskComplete,TaskResult taskResult, string taskName = "defaultTaskName")
        {
            this.task = task;
            this.taskResult = taskResult;
            this.taskComplete = taskComplete;
            this.taskName = taskName;
        }

        public delegate void TaskResult(bool result);
        public delegate bool TaskComplete();
    }



}

