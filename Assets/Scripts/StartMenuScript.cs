using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



    public class StartMenuScript : MonoBehaviour
    {

    AsyncOperation asyncOperation;

        // Use this for initialization
        void Start()
        {
        // PlayerPrefs 系统设置缓存
        PlayerPrefs.SetString("aa","aa");
        PlayerPrefs.Save();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ToSetScene()
        {

            SceneManager.LoadScene("SetScene", LoadSceneMode.Additive);
        }

    public void ToStartScene()
    {
        GameObject lm= GameObject.Find("LoadMenu");
        lm.SetActive(true);
        asyncOperation= SceneManager.LoadSceneAsync("GameScene");
        StartCoroutine(LoadingScene());
    }
    public void OpenLoadWindow()
    {
       // Player.GetComponent < 脚本名字 >（）
    }
    public void Quit()
    {

        Application.Quit(0);
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

            }
            yield return new WaitForEndOfFrame(); //等待一帧
        }
        //计算0.9---1   其实0.9就是加载好了，我估计真正进入到场景是1  
        toProgress = 100;

        while (showProgress < toProgress)
        {
            showProgress++;
            yield return new WaitForEndOfFrame(); //等待一帧
        }

        asyncOperation.allowSceneActivation = true;  //如果加载完成，可以进入场景
    }
}



