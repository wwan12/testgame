using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_mo_Android : MonoBehaviour
{

    AsyncOperation asyncOperation;
    string imei;
    public GameObject Loaddlg;
    long time=0;
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
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ToGameScene() {
        Loaddlg.SetActive(true);
        loadingSlider = Loaddlg.transform.Find("Loading/Slider").GetComponent<Slider>();
        loadingText = Loaddlg.transform.Find("Loading/Slider/Text").GetComponent<Text>();
        asyncOperation = SceneManager.LoadSceneAsync("Game_mo_Android");
        StartCoroutine(LoadingScene());
    }

    public void About() {

    }

    public void LoadProgress(int progress) {
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
        AppManage.Instance.LogWrap(imei);
    }

    private IEnumerator LoadingScene()
    {
        asyncOperation.allowSceneActivation = false;  //如果加载完成，也不进入场景

        int toProgress = 0;
        int showProgress = 0;

        //测试了一下，进度最大就是0.9
        while (asyncOperation.progress < 0.9f)
        {
            toProgress = (int)(asyncOperation.progress * 100);

            while (showProgress < toProgress)
            {
                showProgress++;
                LoadProgress(showProgress);
            }
            yield return new WaitForEndOfFrame(); //等待一帧
        }
        //计算0.9---1   其实0.9就是加载好了，我估计真正进入到场景是1  
        toProgress = 100;

        while (showProgress < toProgress)
        {
            showProgress++;
            LoadProgress(showProgress);
            yield return new WaitForEndOfFrame(); //等待一帧
        }
        asyncOperation.allowSceneActivation = true;  //如果加载完成，可以进入场景
    }
}
