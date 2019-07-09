using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactionsMenu : MonoBehaviour
{
    private ScrollRect scroll;
    // Start is called before the first frame update
    void Start()
    {
        scroll = gameObject.transform.GetChild(0).gameObject.GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddFactions(string fName,int rel)
    {
        GameObject item= GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("prefabs/UI/FactionsItem"));      
        item.transform.SetParent(scroll.content,false);
        item.GetComponentInChildren<Text>().text = fName;
        item.GetComponent<Slider>().value = rel;

    }
}
