using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using EasyAnimation;

public class DialogBox
{
    private static GameObject cacheUI;

    public static void ShowDialogBox(RectTransform canvas, DialogData dialogData)
    {
        GameObject ui = Resources.Load<GameObject>("prefabs/UI/DialogBox");
        Text[] texts= ui.GetComponentsInChildren<Text>();
        foreach (var t in texts)
        {
            switch (t.gameObject.name)
            {
                case "Title":
                    t.text = dialogData.title;
                    break;
                case "Note":
                    t.text = dialogData.note;
                    break;
                case "Yes":
                    t.text = dialogData.yes;
                    break;
                case "No":
                    t.text = dialogData.no;
                    break;
            }
        }
        Button[] buttons= ui.GetComponentsInChildren<Button>();
        foreach (var b in buttons)
        {
            if (b.name.Equals("Yes"))
            {
                b.onClick.AddListener(dialogData.yesCallback);             
            }
            else
            {
                b.onClick.AddListener(dialogData.noCallback);
            }
            b.onClick.AddListener(ClickOver);
        }
        ui= GameObject.Instantiate(ui);
        ui.transform.SetParent(canvas, false);
        cacheUI = ui;          
    }

    static void ClickOver()
    {
        if (cacheUI!=null)
        {
            cacheUI.GetComponent<EasyAnimation_Enlarge>().addListener(DeleteCache,PlayActionType.On_End);
            cacheUI.GetComponent<EasyAnimation_Enlarge>().RePlay();
        }       
    }

    static void DeleteCache()
    {
        GameObject.Destroy(cacheUI);
    }

    public class DialogData
    {
        public string title;
        public string note;
        public string yes;
        public string no;
        public UnityAction yesCallback;
        public UnityAction noCallback;
    }
}
