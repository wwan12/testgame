using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
   // public int pageNumber;
    [Serializable]
    public struct TagPrefab
    {
        public GameObject panel;
        public string pageName;
    }
    [Tooltip("配置页")]
    public TagPrefab[] tags;

    public int defTag = 0;
    [Tooltip("标签的样式")]
    public GameObject tagStyle;

    private GameObject tagsLayout;

    private GameObject panelLayout;

    

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name.Equals("Tags"))
            {
                tagsLayout = child.gameObject;
                float h = tagStyle.GetComponent<RectTransform>().sizeDelta.y;
                RectTransform rect = tagsLayout.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(h / 0.618f * tags.Length,rect.sizeDelta.y) ;


            }
            if (child.gameObject.name.Equals("Panels"))
            {
                panelLayout = child.gameObject;
            }
        }

        for (int i = 0; i < tags.Length; i++)
        {
            if (tagStyle == null)
            {
                GameObject gameObject = new GameObject();
                gameObject.AddComponent<Text>().text = tags[i].pageName;
                //gameObject = GameObject.Instantiate<GameObject>(gameObject);
                gameObject.transform.SetParent(tagsLayout.transform, false);
            }
            else
            {
                Text text = tagStyle.GetComponentInChildren<Text>();
                text.text = tags[i].pageName;
                GameObject go = GameObject.Instantiate<GameObject>(tagStyle);
                go.AddComponent<CanvasGroup>();
                Tag t = go.AddComponent<Tag>();
                t.index = i;
                t.clickCallBack += ClickCallBack;
                go.transform.SetParent(tagsLayout.transform, false);
                go.name = tags[i].pageName;
            }
            tags[i].panel = GameObject.Instantiate<GameObject>(tags[i].panel);
            CanvasGroup group = tags[i].panel.AddComponent<CanvasGroup>();
            tags[i].panel.transform.SetParent(panelLayout.transform, false);
            if (i != defTag)
            {
                group.alpha = 0;
                group.interactable = false;
                group.blocksRaycasts = false;
            }

            tags[i].panel.name = "panel" + i;


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickCallBack(int index) {
        //Debug.LogWarning(defTag+">>"+index + tags[defTag].panel.name);
        if (index!=defTag)
        {
            CanvasGroup group = tags[defTag].panel.GetComponent<CanvasGroup>();
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
            CanvasGroup igroup = tags[index].panel.GetComponent<CanvasGroup>();
            igroup.alpha = 1;
            igroup.interactable = true;
            igroup.blocksRaycasts = true;
            defTag = index;
        }
      
    }
}
