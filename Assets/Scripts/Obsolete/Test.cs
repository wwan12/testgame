using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Messenger.AddListener(EventCode.APP_SAVE_GAME, Render);
        Messenger.AddListener(EventCode.APP_SAVE_GAME, Render);
        Messenger.Broadcast(EventCode.APP_SAVE_GAME);
    }

    void Render()
    {
        Debug.LogWarning("aaaa");
    }
    void Render1()
    {
        Debug.LogWarning("bbbb");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
