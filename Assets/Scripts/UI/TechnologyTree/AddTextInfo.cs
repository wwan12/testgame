using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddTextInfo : MonoBehaviour
{
    public int defSize=14;
    public Transform canvas;
    private float left=4;
    private float top;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void SetInfo(int index, string text)
    {
        var childs = GetComponentsInChildren<Text>(true);
        if (childs.Length > 0)
        {
            childs[index].text = text;
        }
        else
        {
            AddText("def" + text.GetHashCode(), text, defSize, Color.white);
        }

    }

    public void AddInfo(string text)
    {
        AddText("def" + text.GetHashCode(), text, defSize, Color.white);
    }

    public void AddInfo(string text,int size)
    {
        AddText("def" + text.GetHashCode(), text, size, Color.white);
    }

    public void AddInfo(Color color , string text)
    {
        AddText("def" + text.GetHashCode(),text, defSize, color);
    }

    public void AddInfo(string objName,string text) {
        AddText(objName, text, defSize, Color.white);
    }


    private void AddText(string objectName, string text,int fontSize ,Color color)
    {
        var obj = new GameObject(objectName);
        obj.transform.SetParent(canvas==null?transform:canvas);
    
        var typeText = obj.AddComponent<Text>();
        typeText.fontSize = fontSize;
        typeText.text = text;
        typeText.color = color;
    }
}
