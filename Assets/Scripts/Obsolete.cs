

public class Obsolete
{
    //            若要显示：

    //GetComponent<CanvasGroup>().alpha = 1;
    //            GetComponent<CanvasGroup>().interactable = true;
    //            GetComponent<CanvasGroup>().blocksRaycasts = true;

    //            若要隐藏：

    //GetComponent<CanvasGroup>().alpha = 0;
    //            GetComponent<CanvasGroup>().interactable = false;
    //            GetComponent<CanvasGroup>().blocksRaycasts = false;

    //ArticlesAttachment articles = collider.gameObject.GetComponent<ArticlesAttachment>();
    //if (articles == null)
    //{
    //    return;
    //}
    //else
    //{
    //    foreach (var pre in articles.prefix)
    //    {
    //        if (pre== ArticlesAttachment.InteractiveType.PLAYER)
    //        {
    //            available = collider.gameObject;
    //        }
    //    }

    //}

    //    if (Input.GetMouseButton(0))
    // {
    //　　　  Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    RaycastHit hit;

    //       if(Physics.Raycast(ray, out hit))
    //       {

    //               if(hit.transform.name=="Terrain")
    //               {
    //                           position = hit.point;//得到与地面碰撞点的坐标
    //               } 
    //        }
    //}

    //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //RaycastHit hit;  
    //    if (Physics.Raycast(ray, out hit, 100))  
    //    {  
    //        Debug.DrawLine(ray.origin, hit.point);  
    //        Debug.Log("碰撞位置："+hit.normal);  
    //        target.transform.position = hit.point;  
    //        target.transform.up = hit.normal;  
    //        target.transform.Translate(Vector3.up* 0.5f * target.transform.localScale.y, Space.Self);  
    //    }  
    // normalMoveVector = (positionTo - gameObject.transform.localPosition).normalized* speed;
    // CharacterController character_controller = GetComponent<CharacterController>();
    // character_controller.Move(normalMoveVector* Time.deltaTime);

    //m_CD_Left[0] -= Time.deltaTime;  // m_CD_Left[0] 冷却开始后，计算冷却剩余时间
    //    m_Masks[0].fillAmount = m_CD_Left[0] / m_CD_0; 
    //    // 更新对应mask的Image.FillAmount, 由于FillAmount是[0,1]，要换算成对应范围的小数
    //    m_Texts[0].text = string.Format("{0:F1}", m_CD_Left[0]); 
    //    // 更新技能文本中的数字显示，采用string.Format，详情可参考C#官方文档，“F1”表示一位小数

    //    if (m_CD_Left[0] < 0) {  // 如果剩余冷却时间为0，则停止冷却，并重新初始化相关变量。
    //        CD_Trigger[0] = false;   // 下一个frame开始将不再执行if (CD_Trigger[0]){...}语句块的代码；
    //        m_CD_Left[0] = m_CD_0;   // 剩余冷却时间重新赋值为初始值
    //        m_Masks[0].enabled = false;  // Mask被禁用，不显示在图标上
    //        m_Texts[0].enabled = false;  // Text被禁用，不显示数值
    //    }
    //空白地方创造墙体
    //void DrawTile(Vector3 vector, Tile tile)
    //{
    //    // Vector3 mousePosition = Input.mousePosition;
    //    // Vector3 wordPosition = Camera.main.ScreenToWorldPoint(vector);
    //    Vector3Int cellPosition = runMap.WorldToCell(vector);
    //    //tilemap.SetTile(cellPosition, gameUI.GetSelectColor().colorData.mTile);
    //    //TileBase tb = tilemap.GetTile(cellPosition);
    //    //if (tb != null)
    //    //{
    //    //    return;
    //    //}
    //    //tb.hideFlags = HideFlags.None;
    //    //Debug.Log("鼠标坐标" + mousePosition + "世界" + wordPosition + "cell" + cellPosition + "tb" + tb.name);
    //    //格子填充
    //    runMap.SetTile(cellPosition, tile);
    //    //tilemap.RefreshAllTiles();

    //}
    ////销毁墙体
    //void RemoveTile()
    //{
    //    Vector3 mousePosition = Input.mousePosition;
    //    Vector3 wordPosition = Camera.main.ScreenToWorldPoint(mousePosition);
    //    Vector3Int cellPosition = runMap.WorldToCell(wordPosition);
    //    //tilemap.SetTile(cellPosition, gameUI.GetSelectColor().colorData.mTile);
    //    TileBase tb = runMap.GetTile(cellPosition);
    //    if (tb == null)
    //    {
    //        return;
    //    }
    //    //tb.hideFlags = HideFlags.None;
    //    Debug.Log("世界" + wordPosition + "cell" + cellPosition + "tb" + tb.name);
    //    //某个地方设置为空，就是把那个地方小格子销毁了
    //    runMap.SetTile(cellPosition, null);
    //    //tilemap.RefreshAllTiles();
    //}

    /// <summary>
    /// 地图生成
    /// </summary>
    /// <returns></returns>
    //IEnumerator InitData()
    //{
    //    //大地图宽高
    //    int levelW = 10;
    //    int levelH = 10;

    //    int colorCount = 6;
    //    arrTiles = new Tile[colorCount];
    //    for (int i = 0; i < colorCount; i++)
    //    {
    //        //想做生命墙，需要自己做个数据层，对应索引id就行
    //        arrTiles[i] = ScriptableObject.CreateInstance<Tile>();//创建Tile，注意，要使用这种方式
    //                                                              //  arrTiles[i].sprite = baseTile.sprite;
    //        arrTiles[i].color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1);
    //    }
    //    for (int i = 0; i < levelH; i++)
    //    {//这里就是设置每个Tile的信息了
    //        for (int j = 0; j < levelW; j++)
    //        {
    //            runMap.SetTile(new Vector3Int(j, i, 0), arrTiles[UnityEngine.Random.Range(0, arrTiles.Length)]);
    //        }
    //        yield return null;
    //    }

    //    while (true)
    //    {
    //        yield return new WaitForSeconds(2);
    //        // int colorIdx = Random.Range(0, colorCount);//前面这个是随机将某个块的颜色改变，然后让Tilemap更新，主要用来更新Tile的变化
    //        // arrTiles[colorIdx].color = new Color(Random.Range(0f, 1f), Random.Range(0f,1f), Random.Range(0f, 1f), 1);
    //        // tilemap.RefreshAllTiles();

    //        Color c = runMap.color;//这里是改变Tilemap的颜色，尝试是否可以整体变色
    //        c.a -= Time.deltaTime;
    //        runMap.color = c;
    //    }
    //}
}
