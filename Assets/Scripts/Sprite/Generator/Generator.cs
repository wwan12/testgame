using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private GameObject testCheck;
    [Tooltip("允许最大差值,超过此差值会创建失败")]
    public float allowDifference;
    // Start is called before the first frame update
    void Start()
    {
        testCheck = new GameObject();
        testCheck.AddComponent<BoxCollider2D>();
        testCheck.AddComponent<TestCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Generate(GameObject obj,Vector3 pos) {
        
        return false;
    }

    public class TestCheck : MonoBehaviour
    {
        private bool isCheck;
        private void LateUpdate()
        {
            
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            
        }
    }
}
