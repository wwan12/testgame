using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanel : MonoBehaviour
{
    public string help;
    private void Start()
    {

    }

    public void SetInfoPanel(ItemInfo info)
    {
		var childs = GetComponentsInChildren<Text>(true);
		if (childs.Length > 0)
		{
            childs[0].text = string.Format("<color=cyan><b>{0}</b></color>", info.name);
            childs[1].text = string.Format("物品类型：{0}", info.type);
            childs[2].text = string.Format("{0}", info.note);
            childs[3].text = string.Format("<color=gray>{0}</color>", help);
        }
        else
        {
            AddText("ItemName", string.Format("<color=cyan><b>{0}</b></color>", info.name));
            AddText("ItemType", string.Format("物品类型：{0}", info.type));
            AddText("ItemNote", string.Format("{0}", info.note));
            AddText("ItemNote", string.Format("<color=gray>{0}</color>", help));
        }
       
    }

    private void AddText(string objectName, string text)
    {
        var obj = new GameObject(objectName);
        obj.transform.SetParent(transform);
        var typeText = obj.AddComponent<Text>();
        //typeText.font = font;
        typeText.text = text;
    }


}
