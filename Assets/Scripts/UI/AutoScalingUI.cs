using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Auto-scaling according to numerical value
/// 根据数值自动伸缩
/// </summary>
public class AutoScalingUI : MonoBehaviour
{
    public float percentage = 1f;
    private float originalLength;
    private RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = gameObject.GetComponent<RectTransform>();
        originalLength =rect.sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSize(float y)
    {
        Vector2 vector2 = new Vector2(rect.sizeDelta.x, originalLength * y);
        gameObject.GetComponent<RectTransform>().sizeDelta = vector2;
    }
}
