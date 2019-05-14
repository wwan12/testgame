using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class AppManage
{
    private static readonly AppManage _instance = new AppManage();
    private static readonly List<EventListen> eventQueue=new List<EventListen>();
    public Dictionary<string,object> datas =new Dictionary<string, object>();
    public const string ANDROIDOS = "UNITY_ANDROID";
    public const string IOSOS = "UNITY_IOS";
    public const string WINDOWSOS = "UNITY_WINDOWS";
    private const string SAVEFILENAME ="/byBin.dat";
    //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
    public string PathURL;
    public event EventHandler StartSuccessCallBack;
    public event EventHandler SaveSuccessCallBack;
    public event EventHandler LoadSuccessCallBack;
    public event EventHandler ToSaveCallBack;
    public delegate void OSListen();
    public delegate void EventListen(string l);
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
    public string name = "world";
    public string RunOS = WINDOWSOS;
    public string systemInfo;
    public SingleSave saveData;
    public float width;
    public float height;


    /// <summary>
    /// 初始化获取系统信息等
    /// </summary>
    void Init() {
        saveData = new SingleSave();
#if UNITY_IOS
       RunOS = IOSOS;
         PathURL= Application.dataPath + "/Raw/";
#endif

#if UNITY_ANDROID
     RunOS = ANDROIDOS;
         PathURL="jar:file://" + Application.dataPath + "!/assets/"; 
#endif

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        RunOS = WINDOWSOS;
    PathURL="file://" + Application.dataPath + "/StreamingAssets/";

#endif
        SetBasicValues();
        GetDeviceInfo();
        Warehouse.Instance.Initialize();//初始化仓库
    }

   

    void SetBasicValues()
    {

        float leftBorder;
        float rightBorder;
        float topBorder;
        float downBorder;
        //the up right corner
        Vector3 cornerPos = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Mathf.Abs(Camera.main.transform.position.z)));

        leftBorder = Camera.main.transform.position.x - (cornerPos.x - Camera.main.transform.position.x);
        rightBorder = cornerPos.x;
        topBorder = cornerPos.y;
        downBorder = Camera.main.transform.position.y - (cornerPos.y - Camera.main.transform.position.y);

        width = rightBorder - leftBorder;
        height = topBorder - downBorder;
    }

    void GetDeviceInfo() {
        systemInfo = "\tTitle:当前系统基础信息：\n设备模型：" + SystemInfo.deviceModel + "\n设备名称：" + SystemInfo.deviceName + "\n设备类型：" + SystemInfo.deviceType +
            "\n设备唯一标识符：" + SystemInfo.deviceUniqueIdentifier + "\n显卡标识符：" + SystemInfo.graphicsDeviceID +
            "\n显卡设备名称：" + SystemInfo.graphicsDeviceName + "\n显卡厂商：" + SystemInfo.graphicsDeviceVendor +
            "\n显卡厂商ID:" + SystemInfo.graphicsDeviceVendorID + "\n显卡支持版本:" + SystemInfo.graphicsDeviceVersion +
            "\n显存（M）：" + SystemInfo.graphicsMemorySize + "\n显卡像素填充率(百万像素/秒)，-1未知填充率：" + SystemInfo.graphicsPixelFillrate +
            "\n显卡支持Shader层级：" + SystemInfo.graphicsShaderLevel + "\n支持最大图片尺寸：" + SystemInfo.maxTextureSize +
            "\nnpotSupport：" + SystemInfo.npotSupport + "\n操作系统：" + SystemInfo.operatingSystem +
            "\nCPU处理核数：" + SystemInfo.processorCount + "\nCPU类型：" + SystemInfo.processorType +
           "\nsupportedRenderTargetCount：" + SystemInfo.supportedRenderTargetCount + "\nsupports3DTextures：" + SystemInfo.supports3DTextures +
            "\nsupportsAccelerometer：" + SystemInfo.supportsAccelerometer + "\nsupportsComputeShaders：" + SystemInfo.supportsComputeShaders +
            "\nsupportsGyroscope：" + SystemInfo.supportsGyroscope + "\nsupportsImageEffects：" + SystemInfo.supportsImageEffects +
            "\nsupportsInstancing：" + SystemInfo.supportsInstancing + "\nsupportsLocationService：" + SystemInfo.supportsLocationService +
            "\nsupportsRenderTextures：" + SystemInfo.supportsRenderTextures + "\nsupportsRenderToCubemap：" + SystemInfo.supportsRenderToCubemap +
            "\nsupportsShadows：" + SystemInfo.supportsShadows + "\nsupportsSparseTextures：" + SystemInfo.supportsSparseTextures +
           "\nsupportsStencil：" + SystemInfo.supportsStencil + "\nsupportsVertexPrograms：" + SystemInfo.supportsVertexPrograms +
           "\nsupportsVibration：" + SystemInfo.supportsVibration + "\n内存大小：" + SystemInfo.systemMemorySize;
    }


    public void SaveParameter(string key,object data) {
        datas.Add(key, data);
    }

    public void RemoveParameter(string key) {
        datas.Remove(key);
    }

    public E GetParameter<E>(string key) {
       return (E)datas[key];
    }

    public void SendEvent(string key) {
        foreach (EventListen item in eventQueue)
        {
            item(key);
        }
    }

    public void AndroidListen(ref OSListen osListen)
    {
        if (RunOS.Equals(ANDROIDOS))
        {
            osListen.Invoke();
        }
    }
    public void WindowsListen(ref OSListen osListen)
    {
        if (RunOS.Equals(WINDOWSOS))
        {
            osListen.Invoke();
        }
    }
    public void IosListen(ref OSListen osListen)
    {
        if (RunOS.Equals(IOSOS))
        {
            osListen.Invoke();
        }
    }

    public void AddEventListen(ref EventListen eventListen) {
        eventQueue.Add(eventListen);
    }

    public void RemoveEventListen(ref EventListen eventListen) {
        eventQueue.Remove(eventListen);
    }

    [System.Diagnostics.Conditional("LOG")]
    public void LogWrap(string str)
    {
        Debug.Log(str);
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
    /// 传递直接开始新游戏事件
    /// </summary>
    public void StartGame() {
        saveData = CreateSaveGO().singleSaves[0];
        StartSuccessCallBack(this, saveData);
        SaveByBin();
    }
    /// <summary>
    /// 传递读档信息重置游戏事件
    /// </summary>
    /// <param name="save"></param>
    public void SetGame(int saveIndex)
    {
        saveData = LoadByBin().singleSaves[saveIndex];
        LoadSuccessCallBack(this, saveData);
    }
    /// <summary>
    /// 创建Save对象并存储当前游戏状态信息
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
        public int listLe=0;//4个存档位，0是自动存档

        public SingleSave[] singleSaves = new SingleSave[4];

    }
    [System.Serializable]
    public class SingleSave : EventArgs
    {
        public int listIndex = 0;
        public bool isNew = true;
        public int money = 0;
        public int hp = 0;
        public int mp = 0;
        public string mapData = "";
        public string bagData = "";
    }
}
