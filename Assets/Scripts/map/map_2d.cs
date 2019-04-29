using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class map_2d : MonoBehaviour
{

    public Tilemap tilemap;//引用的Tilemap
    public Tile baseTile;//使用的最基本的Tile，我这里是白色块，然后根据数据设置不同颜色生成不同Tile
    Tile[] arrTiles;//生成的Tile数组
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void mapChange() {
        //销毁墙体
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 wordPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(wordPosition);
            //tilemap.SetTile(cellPosition, gameUI.GetSelectColor().colorData.mTile);
            TileBase tb = tilemap.GetTile(cellPosition);
            if (tb == null)
            {
                return;
            }
            //tb.hideFlags = HideFlags.None;
            Debug.Log("鼠标坐标" + mousePosition + "世界" + wordPosition + "cell" + cellPosition + "tb" + tb.name);
            //某个地方设置为空，就是把那个地方小格子销毁了
            tilemap.SetTile(cellPosition, null);
            //tilemap.RefreshAllTiles();
        }

        //空白地方创造墙体
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 wordPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(wordPosition);
            //tilemap.SetTile(cellPosition, gameUI.GetSelectColor().colorData.mTile);
            TileBase tb = tilemap.GetTile(cellPosition);
            if (tb != null)
            {
                return;
            }
            //tb.hideFlags = HideFlags.None;
            //Debug.Log("鼠标坐标" + mousePosition + "世界" + wordPosition + "cell" + cellPosition + "tb" + tb.name);
            //格子填充
            tilemap.SetTile(cellPosition, baseTile);
            //tilemap.RefreshAllTiles();
        }
    }


    /// <summary>
    /// 地图生成
    /// </summary>
    /// <returns></returns>
    IEnumerator InitData()
    {
        //大地图宽高
        int levelW = 10;
        int levelH = 10;

        int colorCount = 6;
        arrTiles = new Tile[colorCount];
        for (int i = 0; i < colorCount; i++)
        {
            //想做生命墙，需要自己做个数据层，对应索引id就行
            arrTiles[i] = ScriptableObject.CreateInstance<Tile>();//创建Tile，注意，要使用这种方式
            arrTiles[i].sprite = baseTile.sprite;
            arrTiles[i].color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        }
        for (int i = 0; i < levelH; i++)
        {//这里就是设置每个Tile的信息了
            for (int j = 0; j < levelW; j++)
            {
                tilemap.SetTile(new Vector3Int(j, i, 0), arrTiles[Random.Range(0, arrTiles.Length)]);
            }
            yield return null;
        }

        while (true)
        {
            yield return new WaitForSeconds(2);
            // int colorIdx = Random.Range(0, colorCount);//前面这个是随机将某个块的颜色改变，然后让Tilemap更新，主要用来更新Tile的变化
            // arrTiles[colorIdx].color = new Color(Random.Range(0f, 1f), Random.Range(0f,1f), Random.Range(0f, 1f), 1);
            // tilemap.RefreshAllTiles();

            Color c = tilemap.color;//这里是改变Tilemap的颜色，尝试是否可以整体变色
            c.a -= Time.deltaTime;
            tilemap.color = c;
        }
    }
}
