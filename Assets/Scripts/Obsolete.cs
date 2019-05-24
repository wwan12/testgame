

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
}
