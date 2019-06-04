using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class AiTaskControl : MonoBehaviour
    {
        Queue<Task> tasks = new Queue<Task>();
        bool isRun;
        IEnumerator taskRun;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddTask(Task task) {
            if (isRun)
            {

            }
            else
            {
                taskRun = TaskRunner();
                StartCoroutine(taskRun);
            }
        }

        public void Stop()
        {
            StopCoroutine(taskRun);
        }

        IEnumerator TaskRunner (){
            while (tasks.Count>0)
            {
                yield return new WaitForEndOfFrame();
            }
           
        }
    }
}

