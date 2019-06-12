using UnityEngine;
using System.Collections;
using Battle;

public class TestFOW : MonoBehaviour
{
    GameObject debugUI = null;

	// Use this for initialization
	void Start () {
        // fow系统启动
        FOWLogic.instance.Startup();
        FOWLogic.instance.AddCharactor(0);
         // 通知角色出生
        //Messenger.Broadcast(MessageName.MN_CHARACTOR_BORN, 1);
        // debug
        //FOWSystemLbl.text = FOWSystem.instance.enableSystem ?
        //   "关闭 FOW 系统" : "开启 FOW 系统";
        //FOWRenderLbl.text = FOWSystem.instance.enableRender ?
        //    "关闭 FOW 渲染" : "开启 FOW 渲染";
        //FOWFogLbl.text = FOWSystem.instance.enableFog ?
        //    "关闭 FOW 迷雾" : "开启 FOW 迷雾";
        //UpdatFreqInput.value = FOWSystem.instance.updateFrequency.ToString();
        //BlendTimeInput.value = FOWSystem.instance.textureBlendTime.ToString();
        //BlurTimeInput.value = FOWSystem.instance.blurIterations.ToString();
        //RadiusOffsetInput.value = FOWSystem.instance.radiusOffset.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        int deltaMS = (int)(Time.deltaTime * 1000f);
        FOWLogic.instance.Update(deltaMS);
	}


    private void OnDestroy()
    {
        FOWLogic.instance.Dispose();
    }
}
