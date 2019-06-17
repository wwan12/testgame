using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_mo_UI : MonoBehaviour
{

    AsyncOperation asyncOperation;
    string imei;
    [Tooltip("进入加载界面")]
    public GameObject loaddlg;
    [Tooltip("存档界面")]
    public GameObject saveMenu;
    long time=0;
    AppManage.SingleSave[] saves;
    /// <summary>
    /// 加载进度条
    /// </summary>
    private Slider loadingSlider;
    /// <summary>
    /// 文本
    /// </summary>
    private Text loadingText;
    // Start is called before the first frame update
    void Start()
    {
        // GetIMEI();

        PlayerPrefs.SetString("aa", "aa");
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void InitLoadMenu() {
        saves= AppManage.Instance.LoadAllGame().singleSaves;
        Image[] images= saveMenu.GetComponentsInChildren<Image>();
        for (int i = 0; i < saves.Length; i++)
        {
            if (saves[i] != null)
            {
               images[i].sprite= Resources.Load<Sprite>("Save/"+saves[i].portraitName);
            }
            else
            {
                images[i].sprite = Resources.Load<Sprite>("Save/Null");
            }          
        }
        AppManage.Instance.SetOpenUI(saveMenu);
    }

    public void SaveSelect(int i) {
        if (saves[i] != null)
        {
            AppManage.Instance.LoadGame(i).listIndex=i;
            ToGameScene();
        }
        else
        {
            AppManage.Instance.CreateSingleSave(i).listIndex=i;
            GameObject role= GameObject.Find("RoleSelectMenu");
            AppManage.Instance.SetOpenUI(role);
        }
    }

    public void RoleSelect(int i) {
        AppManage.Instance.saveData.roleId = i;
        ToGameScene();
    }

    private void ToGameScene() {
        loaddlg.SetActive(true);
        loadingSlider = loaddlg.transform.Find("Loading/Slider").GetComponent<Slider>();
        loadingText = loaddlg.transform.Find("Loading/Slider/Text").GetComponent<Text>();
        asyncOperation = SceneManager.LoadSceneAsync("Game_mo_Android");
        AppManage.Instance.LoadSceneCallBack += LoadProgress;
        AppManage.Instance.StartLoadScene(this, asyncOperation);
        //StartCoroutine(LoadingScene());
    }

    public void About() {

    }

    public void LoadProgress(object obj,int progress) {
         if (loadingSlider.value != 1.0f) {
                loadingText.text = "加载中。。。" + (loadingSlider.value * 100).ToString() + "%";
            }
    }

    public void Quit()
    {
        Application.Quit();

    }

    private void QuitTip() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (time == 0 || Time.time - time < 3000)
            {
                AppManage.Instance.ToastAndroid("再次按下返回键退出");
            }
            else
            {
                Application.Quit();

            }
            time = (long)Time.time;
        }
    }

    private void GetIMEI() {
        imei= SystemInfo.deviceUniqueIdentifier;
        var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var context = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        var telephoneyManager = context.Call<AndroidJavaObject>("getSystemService", "phone");
        imei = telephoneyManager.Call<string>("getImei", 0);//如果手机双卡 双待  就会有两个MIEI号
        if (imei == null || imei.Equals("")) {
            imei = telephoneyManager.Call<string>("getMeid");//电信的手机 是MEID
        }
        //imei1 = telephoneyManager.Call<string>("getImei", 1);
    }
}
