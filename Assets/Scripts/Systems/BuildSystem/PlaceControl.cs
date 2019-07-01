using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceControl : MonoBehaviour
{


    public bool isCheck = true;
    private SpriteRenderer sprite;
    
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    private void OnCollisionStay2D(Collision2D collision)
    {
        isCheck = false;
        sprite.color = Color.red;
       // Debug.Log("stayc:" + isCheck);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isCheck = true;
        sprite.color = Color.green;
       // Debug.Log("exitc:" + isCheck);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCheck = false;
        sprite.color = Color.red;
       // Debug.Log("stayt:" + isCheck);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isCheck = true;
        sprite.color = Color.green;
       // Debug.Log("Exitt:" + isCheck);
    }
}
