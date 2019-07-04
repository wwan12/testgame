using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenu : MonoBehaviour
{
    private GameObject set;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSetMenu(Canvas canvas)
    {
        if (set==null)
        {
            set = Resources.Load<GameObject>("prefabs/UI/SetMenu");
            set = GameObject.Instantiate<GameObject>(set);
            set.transform.SetParent(canvas.gameObject.transform, false);
        }
        AppManage.Instance.SetOpenUI(set);
    }

    public void BackMainMenu()
    {
        AppManage.Instance.SaveGame();
        SceneManager.LoadScene("Menu_mo");
    }

    public void OpenGuide()
    {

    }

    public void Continue() {
        Destroy(set);
        Destroy(gameObject);
    }

}
