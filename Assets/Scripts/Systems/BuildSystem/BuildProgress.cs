using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildProgress : MonoBehaviour
{
    public GameObject lockBuild;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vector = lockBuild.transform.position;
        vector.y += 0.2f;
        gameObject.transform.position = Camera.main.WorldToScreenPoint(vector);
    }
   
    
}
