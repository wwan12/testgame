using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in Application.GetBuildTags())
        {
            Debug.LogWarning(item+">>");
        }
        Debug.LogWarning(Application.GetBuildTags().Length + ">>");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
