using External;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_mo_UI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //Physics.gravity = new Vector3(0, -1.0F, 0);
        ExternalRead read = new ExternalRead();
        read.ReadItems(this);
        StartCoroutine(test());
        
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(2);
        GameObject.FindGameObjectWithTag("Bag").GetComponent<BagManage>().BagAddItem(new ItemInfo()
        {
            name = "aaa" + 0,
            sprite = Resources.Load<Sprite>("Texture/Items/zdnp"),
            num = 12,
        });
        GameObject.FindGameObjectWithTag("Bag").GetComponent<BagManage>().BagAddItem( new ItemInfo()
        {
            name = "aaa" + 1,
            sprite = Resources.Load<Sprite>("Texture/Items/zdnp"),
            num = 1,

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    

}
