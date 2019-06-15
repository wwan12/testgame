using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private GameObject testCheck;
    private TestCheck check;
    [Tooltip("允许最大差值,超过此差值会创建失败")]
    public float allowDifference=0f;
    // Start is called before the first frame update
    void Start()
    {
        testCheck = new GameObject();
        testCheck.AddComponent<BoxCollider2D>();
        check= testCheck.AddComponent<TestCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Generate(GameObject obj,Vector3 pos) {
        testCheck.transform.position = pos;
        if (check.isCheck)
        {
            GameObject.Instantiate(obj, pos, Quaternion.identity);
            return true;
        }
       
       
        return false;
    }

    public class TestCheck : MonoBehaviour
    {
        public bool isCheck;
        private void LateUpdate()
        {
            
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            isCheck = false;
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            isCheck = true;
        }
    }
}
