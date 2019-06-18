using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 提供给其他特殊建筑继承
/// </summary>
public class BuildControl : MonoBehaviour
{
    public string objectName = "Building Name";
    public float durable;
    public float dTime;
    public float buildTime;
    public GameObject buildProgress;
    public GameObject dismantleProgress;
    private int progress = 0;
    private bool ready;


    void Start()
    {
        name = objectName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseOver()
    {
        if (ready&&Input.GetKeyDown(KeyCode.Mouse0))
        {
            Open();
        }
        if (ready&&Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (dTime!=0)
            {
                StartCoroutine(RemoveProgress());
            }
            else
            {
                Destroy(gameObject);
            }                       
        }
    }
    /// <summary>
    /// 当呗左键点中
    /// </summary>
    public virtual void Open() {

    }

    IEnumerator RemoveProgress()
    {
        GameObject dp= GameObject.Instantiate(dismantleProgress);
        dp.transform.SetParent(GameObject.Find("GameUI").transform);
        dp.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        while (progress < 100)
        {
            yield return new WaitForSeconds(dTime / 100);
            progress++;
            dp.GetComponentInChildren<Image>().fillAmount = progress / 100;
        }
        progress = 0;
        Destroy(gameObject);
    }

    public void Build() {
        if (buildTime!=0)
        {
            StartCoroutine(BuildProgress());
        }
        else
        {
            ready = true;
        }
       
    }


    IEnumerator BuildProgress()
    {
        GameObject bp= GameObject.Instantiate(buildProgress);
        bp.transform.SetParent(GameObject.Find("GameUI").transform);
        bp.transform.position= Camera.main.WorldToScreenPoint(gameObject.transform.position);
        Color alpha= gameObject.GetComponent<SpriteRenderer>().color;
        while (progress < 100)
        {
            yield return new WaitForSeconds(buildTime / 100);
            progress++;
            alpha.a = progress*(255/100);
            gameObject.GetComponent<SpriteRenderer>().color = alpha;
            bp.GetComponentInChildren<Image>().fillAmount = progress / 100;

        }
        progress = 0;
        ready = true;
        Destroy(bp);
    }
}
