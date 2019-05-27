

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
      
}
