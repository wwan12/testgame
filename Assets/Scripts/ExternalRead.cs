using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

namespace External
{

    public class Item
    {
        public string name;
        public string id;
        public float cost;
        public int superposition;
        public string icon;
        public string note;
        public ItemType type;
    }

    public class ExternalRead
    {
        private TextAsset textasset;
        public readonly string ITEMTABLE_PATH = "ExternalConfigurationTable/ItemTable.xlsx";
        // Use this for initialization
        void Start()
        {

            //readTextToFormJson ();
        }
        // Update is called once per frame
        void Update()
        {
        }

        public void ReadItems(MonoBehaviour mono)
        {
           mono.StartCoroutine(ReadTextToFormJson());
        }
        IEnumerator ReadTextToFormJson()
        {
            textasset = Resources.Load<TextAsset>(ITEMTABLE_PATH);
            JsonData jd;
            List<Item> items = new List<Item>();
            if (textasset != null)
            {
                jd = JsonMapper.ToObject(textasset.text);

                for (int i = 0; i < jd.Count; i++)
                {
                    Item bitem = new Item();
                    bitem.id = jd[i]["id"].ToString();
                    bitem.name = jd[i]["名称"].ToString();
                    bitem.cost = float.Parse(jd[i]["价值"].ToString());
                    bitem.icon = jd[i]["图标"].ToString();
                    bitem.note = jd[i]["说明"].ToString();
                    bitem.superposition = int.Parse(jd[i]["叠加数量"].ToString());
                    bitem.type = (ItemType)Enum.ToObject(typeof(ItemType), int.Parse(jd[i]["物品类型"].ToString()));
                    items.Add(bitem);
                }
            }
            Warehouse.Instance.itemTable = items;
            yield return null;
            //几个特殊路径
            //		Debug.Log ("dataPath ：" + Application.dataPath);
            //		Debug.Log ("persistentDataPath ：" + Application.persistentDataPath);
            //		Debug.Log ("temporaryCachePath ：" + Application.temporaryCachePath);
            //		Debug.Log ("streamingAssetsPath ：" + Application.streamingAssetsPath); 		
            //第二种读取读取本地文件方法
            //		FileStream fs = File.OpenRead (Application.dataPath+ "/Resources/book.txt");
            //		StreamReader sr = new StreamReader (fs);
            //		string wholeText = sr.ReadToEnd ();
            //		sr.Close ();
            //		fs.Close ();
            //		fs.Dispose ();
            //		JsonData jd = JsonMapper.ToObject (wholeText); 		
            //用WWW方法读取网络文件（加入协程）（可以做一个进度条）
            //		WWW www = new WWW("http://9214193.s21d-9.faiusrd.com/68/ABUIABBEGAAg7YXmuAUohKPUtQY.txt");
            //		Debug.Log ("start to load file.....");
            //		yield return www;
            //		Debug.Log ("end of load .");
            //		JsonData jd = JsonMapper.ToObject (www.text);  	
            //		读入json数据，并转化为程序中的数据结构		


        }
        public void ReadItems() {
 }


        
    }
}

