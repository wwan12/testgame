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
     
        public Action task;
        /// <summary>
        /// 任务是否成功完成
        /// </summary>
        public bool taskResult;

        public Task(Action task, string taskName = "defaultTaskName")
        {
            this.task = task;
            this.taskName = taskName;
        }
    }



}

