using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManage : MonoBehaviour
{
    private bool isRunLay;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunLay)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
             //   Debug.DrawLine(ray.origin, hit.point);
             //   Debug.Log("碰撞位置：" + hit.normal);
                gameObject.transform.position = hit.point;
                gameObject.transform.up = hit.normal;
                gameObject.transform.Translate(Vector3.up * 0.5f * gameObject.transform.localScale.y, Space.Self);
            }
        }
       
    }

    public void StartLay()
    {
        isRunLay = true;
    }

    public void StopLay()
    {
        isRunLay = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (image.color != Color.red)
        {
            image.color = Color.red;
        }
        if (Input.GetMouseButton(0))
        {
            BuildThis();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        image.color = Color.white;
    }

    void BuildThis() {
        isRunLay = false;
    }
}
