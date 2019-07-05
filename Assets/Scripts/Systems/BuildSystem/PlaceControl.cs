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
    private void OnTriggerExit2D(Collider2D collision)
    {
        isCheck = true;
        sprite.color = Color.green;
        Debug.LogWarning(collision.gameObject.name);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.gameObject.name.Equals("Boundary"))
        {
            isCheck = false;
            sprite.color = Color.red;
            Debug.LogWarning(collision.gameObject.name);
        }
      
    }
}
