using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIFadeInFadeOut : MonoBehaviour
{
    public float targetAlpha = 1.0f;
    public float alphaSpeed = 2.0f;

    private bool depressFlag = false;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
        Hide();
    }

    void Update()
    {
        if (canvasGroup == null)
        {
            return;
        }

        if (targetAlpha != canvasGroup.alpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, alphaSpeed * Time.deltaTime);
            if (Mathf.Abs(targetAlpha - canvasGroup.alpha) <= 0.01f)
            {
                canvasGroup.alpha = targetAlpha;
            }
        }
    }

    /// <summary>
    /// UI出现与隐藏
    /// </summary>

    void Hide() {
        targetAlpha = 0;
        canvasGroup.blocksRaycasts = false;
        depressFlag = true;
    }

    void Show() {
        targetAlpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        depressFlag = false;
    }
}
