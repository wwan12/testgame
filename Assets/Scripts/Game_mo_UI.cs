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
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    

}
