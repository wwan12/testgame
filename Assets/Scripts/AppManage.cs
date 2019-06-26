using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class AppManage
{
    private static readonly AppManage _instance = new AppManage();
    public Dictionary<string,object> datas =new Dictionary<string, object>();
    public const string ANDROIDOS = "UNITY_ANDROID";
    public const string IOSOS = "UNITY_IOS";
    public const string WINDOWSOS = "UNITY_WINDOWS";
    private const string SAVEFILENAME ="/byBin.dat";
    public event EventHandler StartCallBack;
    public event EventHandler ExitCallBack;
    public event EventHandler SaveSuccessCallBack;
    public event EventHandler<SingleSave> LoadSuccessCallBack;
    public event EventHandler ToSaveCallBack;
    public event EventHandler<int> LoadSceneCallBack;
    private Save allSave;
    public static AppManage Instance
    {
        get
        {
            return _instance;
        }
    }

    private AppManage() { Init(); }

    /**
     * 共享属性
     * */
    public string RunOS = WINDOWSOS;
    public string systemInfo;
    public SingleSave saveData;
    /// <summary>
    /// 上层显示的ui可以快速关闭
    /// </summary>
    public GameObject openUI;
    public bool isNew;
    /// <summary>
    /// 是否已经开始
    /// </summary>
    public bool isInGame;


    /// <summary>
    /// 初始化获取系统信息等
    /// </summary>
    void Init()
    {
#if UNITY_IOS
       RunOS = IOSOS;
        
#endif

#if UNITY_ANDROID
     RunOS = ANDROIDOS;
     
#endif

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        RunOS = WINDOWSOS;      

#endif
        GetDeviceInfo();

    }

    void GetDeviceInfo() {
        systemInfo = "\tTitle:当前系统基础信息：\n设备模型：" + SystemInfo.deviceModel + "\n设备名称：" + SystemInfo.deviceName + "\n设备类型：" + SystemInfo.deviceType +
            "\n设备唯一标识符：" + SystemInfo.deviceUniqueIdentifier + "\n显卡标识符：" + SystemInfo.graphicsDeviceID +
            "\n显卡设备名称：" + SystemInfo.graphicsDeviceName + "\n显卡厂商：" + SystemInfo.graphicsDeviceVendor +
            "\n显卡厂商ID:" + SystemInfo.graphicsDeviceVendorID + "\n显卡支持版本:" + SystemInfo.graphicsDeviceVersion +
            "\n显存（M）：" + SystemInfo.graphicsMemorySize +
            "\n显卡支持Shader层级：" + SystemInfo.graphicsShaderLevel + "\n支持最大图片尺寸：" + SystemInfo.maxTextureSize +
            "\nnpotSupport：" + SystemInfo.npotSupport + "\n操作系统：" + SystemInfo.operatingSystem +
            "\nCPU处理核数：" + SystemInfo.processorCount + "\nCPU类型：" + SystemInfo.processorType +
           "\nsupportedRenderTargetCount：" + SystemInfo.supportedRenderTargetCount + "\nsupports3DTextures：" + SystemInfo.supports3DTextures +
            "\nsupportsAccelerometer：" + SystemInfo.supportsAccelerometer + "\nsupportsComputeShaders：" + SystemInfo.supportsComputeShaders +
            "\nsupportsGyroscope：" + SystemInfo.supportsGyroscope + "\nsupportsImageEffects：" +
            "\nsupportsInstancing：" + SystemInfo.supportsInstancing + "\nsupportsLocationService：" + SystemInfo.supportsLocationService +            
            "\nsupportsShadows：" + SystemInfo.supportsShadows + "\nsupportsSparseTextures：" + SystemInfo.supportsSparseTextures +         
           "\nsupportsVibration：" + SystemInfo.supportsVibration + "\n内存大小：" + SystemInfo.systemMemorySize;
    }

    public void SetOpenUI(string path)
    {
        GameObject ui= Resources.Load<GameObject>(path);
        CanvasGroup group = ui.GetComponent<CanvasGroup>() ?? openUI.AddComponent<CanvasGroup>();
    }

    /// <summary>
    /// 显示一个ui控件，若已显示则隐藏 todo转为消息系统处理
    /// </summary>
    /// <param name="ui"></param>
    /// <returns></returns>
    public void SetOpenUI(GameObject ui) { 
         CanvasGroup group;
        if (openUI!=null&&openUI.GetInstanceID()!=ui.GetInstanceID())
        {
            group = openUI.GetComponent<CanvasGroup>();
            group.alpha = 0;
            group.interactable =  false;
            group.blocksRaycasts = false;
        }
        openUI = ui;
        group=openUI.GetComponent<CanvasGroup>()??openUI.AddComponent<CanvasGroup>();
        group.alpha = group.alpha == 1 ? 0 : 1;
        group.interactable = group.interactable ? false : true;
        group.blocksRaycasts = group.blocksRaycasts ? false : true;      
    }

    private GameObject[] openUICaches;

    private void OpenUIGC( ) {
        if (openUICaches == null)
        {
            openUICaches = new GameObject[20];
        }


    }

    /// <summary>
    /// 储存全局数据
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public void SaveParameter(string key,object data) {
        datas.Add(key, data);
    }

    public void RemoveParameter(string key) {
        datas.Remove(key);
    }

    public E GetParameter<E>(string key) {
       return (E)datas[key];
    }
    /// <summary>
    /// C#调用Android 的Toast
    /// </summary>
    public void ToastAndroid(string text)
    {
        AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
        activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            toast.CallStatic<AndroidJavaObject>("makeText", context, text, toast.GetStatic<int>("LENGTH_LONG")).Call("show");
        }));

    }
    /// <summary>
        /// 二进制方法：存档
        /// </summary>
    private void SaveByBin()
    {
        Save save = LoadByBin();
        //序列化过程（将Save对象转换为字节流）
        //创建Save对象并保存当前游戏状态
        if (save == null)
        {
            save = CreateSaveGO();
        }
        else {
            save.singleSaves[saveData.listIndex] = saveData;
        }       
        //创建一个二进制格式化程序
        BinaryFormatter bf = new BinaryFormatter();
        //创建一个文件流
        FileStream fileStream = System.IO.File.Create(Application.dataPath + SAVEFILENAME);
        //用二进制格式化程序的序列化方法来序列化Save对象,参数：创建的文件流和需要序列化的对象
        bf.Serialize(fileStream, save);
        //关闭流
        fileStream.Close();
        //如果文件存在，则显示保存成功
        if (System.IO.File.Exists(Application.dataPath + SAVEFILENAME))
        {
            SaveSuccessCallBack(this, EventArgs.Empty);
        }
    }

    /// <summary>
    ///二进制方法： 读档
    /// </summary>
    private Save LoadByBin()
    {
        if (System.IO.File.Exists(Application.dataPath + SAVEFILENAME))
        {
            //反序列化过程
            //创建一个二进制格式化程序
            BinaryFormatter bf = new BinaryFormatter();
            //打开一个文件流
            FileStream fileStream = System.IO.File.Open(Application.dataPath + SAVEFILENAME, FileMode.Open);
            //调用格式化程序的反序列化方法，将文件流转换为一个Save对象
            Save save = (Save)bf.Deserialize(fileStream);
            //关闭文件流
            fileStream.Close();
           // SetGame(save);
            return save;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 传递开始新游戏事件
    /// </summary>
    public void StartNewGame(MonoBehaviour mono) {
        //StartCallBack(this, saveData);
        GameObject.FindObjectOfType<MapManage>().CreateMap();    
        saveData.mapData = GameObject.FindObjectOfType<MapManage>().SaveMap();//获取地图数据
        saveData.bagData = GameObject.FindObjectOfType<BagManage>().SaveBagData();//获取背包数据
        saveData.playerLocation[0] = GameObject.FindGameObjectWithTag("Player").transform.position.x;
        saveData.playerLocation[1] = GameObject.FindGameObjectWithTag("Player").transform.position.y;
        saveData.playerLocation[2] = GameObject.FindGameObjectWithTag("Player").transform.position.z;
        SaveByBin();
        Messenger.Broadcast<SingleSave>(EventCode.APP_START_GAME, saveData);
        isInGame = true;
        mono.StartCoroutine(AutoSave());//启动自动存档  
        
    }
    /// <summary>
    /// 继续游戏
    /// </summary>
    public void ContinueGame(MonoBehaviour mono) {        
        GameObject.FindObjectOfType<MapManage>().ReadMap(saveData.mapData);//恢复地图数据
        GameObject.FindObjectOfType<BagManage>().ReadBagData(saveData.bagData);//恢复背包数据
        GameObject.FindGameObjectWithTag("Player").transform.position=new Vector3(saveData.playerLocation[0],saveData.playerLocation[1],saveData.playerLocation[2]);//恢复人物位置
        Messenger.Broadcast<SingleSave>(EventCode.APP_CONTINUE_GAME, saveData);
        isInGame = true;
        mono.StartCoroutine(AutoSave());//启动自动存档      
    }
    /// <summary>
    /// 传递退出游戏事件
    /// </summary>
    public void ExitGame()
    {
        ExitCallBack(this, saveData);
        SaveByBin();
        saveData = null;

    }
    /// <summary>
    /// 读取存档游戏事件
    /// </summary>
    /// <param name="save"></param>
    public Save LoadAllGame()
    {
        allSave = LoadByBin();
        if (allSave==null)
        {
            allSave = CreateSaveGO();
        }     
        return allSave;
    }
    /// <summary>
    /// 读取指定序号存档游戏事件
    /// </summary>
    /// <param name="save"></param>
    public SingleSave LoadGame(int saveIndex)
    {
        saveData = allSave==null? LoadByBin().singleSaves[saveIndex]:allSave.singleSaves[saveIndex];
       
        Debug.Log(saveData.roleId.ToString());
        if (LoadSuccessCallBack!=null)
        {
            LoadSuccessCallBack(this, saveData);
        }     
        return saveData;
    }

    public SingleSave CreateSingleSave(int saveIndex)
    {
        if (saveData == null)
        {
            allSave.singleSaves[saveIndex] = new SingleSave();
            saveData = allSave.singleSaves[saveIndex];
        }

        return saveData;

    }
    /// <summary>
    /// 触发一次手动存档
    /// </summary>
    /// <param name="save"></param>
    public void SaveGame()
    {
        SaveByBin();
        
    }
    /// <summary>
    /// 创建Save对象
    /// </summary>
    /// <returns>存档对象</returns>
    private Save CreateSaveGO()
    {
        //新建Save对象
        Save save = new Save();
        //返回该Save对象
        return save;
    }

    [System.Serializable]
    public class Save: EventArgs
    {
        public int listLe=0;//3个存档位

        public SingleSave[] singleSaves = new SingleSave[3];

    }
    [System.Serializable]
    public class SingleSave : EventArgs
    {
        public int roleId = 0;
        public int listIndex = 0;
        public string portraitName = "";
        public int money = 0;
        public int hp = 0;
        public int mp = 0;
        public float[] playerLocation = new float[3];
        public string mapData = "";
        public string bagData = "";
        public string buildLocation = "";
        public string otherData = "";
    }
    AsyncOperation asyncOperation;

    private IEnumerator AutoSave() {
        while (isInGame) {
            yield return new WaitForSeconds(60 * 2);
            SaveByBin();
        }
       
    }

    public void StartLoadScene(MonoBehaviour mono, AsyncOperation async) {
        asyncOperation = async;
        mono.StartCoroutine(LoadingScene());
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
                LoadSceneCallBack(this, showProgress);
            }
            yield return new WaitForEndOfFrame(); //等待一帧
        }
        //计算0.9---1   其实0.9就是加载好了，我估计真正进入到场景是1  
        toProgress = 100;

        while (showProgress < toProgress)
        {
            showProgress++;
            LoadSceneCallBack(this, showProgress);
            yield return new WaitForEndOfFrame(); //等待一帧
        }
        asyncOperation.allowSceneActivation = true;  //如果加载完成，可以进入场景
        if (isNew)
        {
            StartNewGame(GameObject.FindObjectOfType<Game_mo_UI>());
        }
        else
        {
            ContinueGame(GameObject.FindObjectOfType<Game_mo_UI>());
        }
    }
}
