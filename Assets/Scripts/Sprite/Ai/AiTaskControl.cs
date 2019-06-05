using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{

    public class AiTaskControl : MonoBehaviour
    {
        Queue<Task> tasks = new Queue<Task>();
        [HideInInspector]
        public bool isRun;
        IEnumerator taskRun;
        Animator ani;
        public AiState state;
        public event EventHandler<Task> IdleCallBack;
        // Start is called before the first frame update
        void Start()
        {
            ani =  gameObject.GetComponent<Animator>();
            state = AiState.Idle;
        }

        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(Task task) {
            tasks.Enqueue(task);
        }
        /// <summary>
        /// 添加一个任务,丢弃之前的任务并立即执行
        /// </summary>
        /// <param name="task"></param>
        public void AddTaskNow(Task task)
        {
            tasks.Clear();
            tasks.Enqueue(task);
        }
        /// <summary>
        /// 执行一个任务
        /// </summary>
        /// <param name="task"></param>
        private void Implement(Task task)
        {
            if (!isRun)
            {
                taskRun = TaskRunner();
                StartCoroutine(taskRun);
            }
            if (task.taskName.Equals("Idle"))
            {
                state = AiState.Idle;
                IdleCallBack(this, task);//在这里将task发送回去,供替换待机方法
            }
            else
            {
                state = AiState.beBusy;
            }
            task.task.Invoke();
        }
        /// <summary>
        /// 清除任务
        /// </summary>
        public void ClearTask()
        {
            tasks.Clear();
        }

        public void Stop()
        {
            StopCoroutine(taskRun);
        }

        private void TaskComplete()
        {
          //  state = AiState.Idle;
        }

        IEnumerator TaskRunner (){
            isRun = true;
            while (isRun)
            {
                if (tasks.Count > 0)
                {
                    Implement(tasks.Dequeue());
                }
                else {
                    Task task = new Task(TaskComplete,"Idle");
                    tasks.Enqueue(task);
                }
                yield return new WaitForEndOfFrame();
            }
           
        }
        public enum AiState
        {
            Idle,
            beBusy,
        }
    }
}

