using System;
using UnityEngine;

public class ItemInfo: EventArgs
{
    public int id;
    public string name;
    public string type;
    public string note;
    public int num;
    public bool isUse=true;
    public int bagNum;
    public Sprite sprite;
}