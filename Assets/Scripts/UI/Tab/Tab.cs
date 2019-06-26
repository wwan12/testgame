using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tab : MonoBehaviour
{
   // public int pageNumber;
    
    public struct TagPrefab
    {
        public Panel panel;
        public string pageName;
    }
    [Tooltip("配置页")]
    public TagPrefab[] tags;

    public int defTag = 0;

    public GameObject tagStyle;

    public Canvas canvas;

    public GameObject tagsLayout;

    public GameObject panelLayout;



    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach (var tag in tags)
        {
            if (tagStyle==null)
            {
                GameObject gameObject = new GameObject();
                gameObject.AddComponent<Text>().text = tag.pageName;
                gameObject.transform.SetParent(tagsLayout.transform,false);
            }
            else
            {
                Text text = tagStyle.GetComponentInChildren<Text>();
                text.text = tag.pageName;
                GameObject go= GameObject.Instantiate<GameObject>(tagStyle);
                go.AddComponent<CanvasGroup>();
                go.transform.SetParent(tagsLayout.transform,false);
            }

            GameObject p = GameObject.Instantiate<GameObject>(tag.panel.gameObject);
            CanvasGroup group= p.AddComponent<CanvasGroup>();
            p.transform.SetParent(panelLayout.transform,false);
            if (i!=defTag)
            {
                group.alpha = 0;
                group.interactable = false;
                group.blocksRaycasts = false;
            }
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
