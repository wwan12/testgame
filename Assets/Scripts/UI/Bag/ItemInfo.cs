using System;
using UnityEngine;

public class ItemInfo: EventArgs
{
    public int id;
    public string name="";
    public string type;
    public string note="";
    public int num=0;
    public float weight=0;
    public bool isUse=true;
    public Sprite sprite;
}