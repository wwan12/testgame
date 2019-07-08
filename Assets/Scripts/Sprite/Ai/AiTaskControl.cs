using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    
    public class AiTaskControl : MonoBehaviour
    {
        Queue<Task> tasks = new Queue<Task>();
        IEnumerator taskRun;
        Animator ani;
        Task task;
        float intervalTime=0.2f;
        int intervalNums = 0;
        public AiState state;

        // Start is called before the first frame update
        void Start()
        {
          
            ani =  gameObject.GetComponent<Animator>();
            state = AiState.Ready;
            StartCoroutine(TaskRunner());
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
            Stop();
            tasks.Clear();
            tasks.Enqueue(task);
        }
        /// <summary>
        /// 执行一个任务
        /// </summary>
        /// <param name="task"></param>
        private void Implement(Task task)
        {
            
            taskRun = task.task;
            StartCoroutine(taskRun);
          
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
            state = AiState.Ready;
        }

        IEnumerator TaskRunner (){
            yield return new WaitForSeconds(intervalTime*10);
            while (true)
            {
                if (state==AiState.Ready)
                {
                    if (tasks.Count > 0)
                    {
                        state = AiState.Execut;
                        task = tasks.Dequeue();
                        Implement(task);
                    }
                }
                else
                {
                    if (task.taskComplete.Invoke())
                    {
                        task.taskResult.Invoke(true);
                    }
                    else
                    {
                        if (task.waitTime<intervalTime*intervalNums)
                        {
                            task.taskResult.Invoke(false);
                            Stop();
                        }
                        intervalNums++;
                    }
                    
                }     
                yield return new WaitForSeconds(intervalTime);
            }
           
        }
        public enum AiState
        {
            Ready,
            Execut,
        }

        

       
    }
}

