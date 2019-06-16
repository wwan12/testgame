using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceControl : MonoBehaviour
{

    public float time;
    private int progress=0;
    public bool isCheck = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Camera.main.ScreenToWorldPoint(gameObject.transform.position);
           // gameObject.GetComponent<BuildOS>();
            Destroy(gameObject);
        }
    }

    IEnumerator RemoveProgress() {
        while (progress<10)
        {
            yield return new WaitForSeconds(time / 10);
            progress++;
        }
       
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isCheck = false;
        sprite.color = Color.red;
        Debug.Log("stayc:" + isCheck);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isCheck = true;
        sprite.color = Color.green;
        Debug.Log("exitc:" + isCheck);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isCheck = false;
        sprite.color = Color.red;
        Debug.Log("stayt:" + isCheck);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isCheck = true;
        sprite.color = Color.green;
        Debug.Log("Exitt:" + isCheck);
    }
}
