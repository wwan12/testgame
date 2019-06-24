using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferModel : MonoBehaviour
{
    [HideInInspector]
    public string gridName;
    [HideInInspector]
    public float connectionScope = 1f;
    private List<EnergyComponent> energys;
    private SpriteRenderer rangeDis;
    // Start is called before the first frame update
    void Start()
    {
        energys = new List<EnergyComponent>();

        rangeDis = Resources.Load<GameObject>("prefabs/UI/RangeDisplay").GetComponent<SpriteRenderer>();
        rangeDis.size = new Vector2(connectionScope, connectionScope);
        rangeDis.transform.position = gameObject.transform.position;
        rangeDis = GameObject.Instantiate(rangeDis);
        rangeDis.transform.SetParent(gameObject.transform, false);
        rangeDis.enabled = false;

        CircleCollider2D circleCollider= gameObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = connectionScope;
        circleCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowRange()
    {
        rangeDis.enabled = true;
    }

    public void HideRange()
    {
        rangeDis.enabled = false;
    }

    public void DestroyEnergy(EnergyComponent e)
    {
        energys.Remove(e);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnergyComponent e = collision.gameObject.GetComponent<EnergyComponent>();
        if (e!=null)
        {
            energys.Add(e);
        }
      
    }
    /// <summary>
    /// todo 测试销毁是否会触发
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        EnergyComponent e = collision.gameObject.GetComponent<EnergyComponent>();
        if (e != null)
        {
            energys.Remove(e);
        }
    }
}
