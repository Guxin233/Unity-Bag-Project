using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {
    
    // 两个Text显示的内容保持一致
    private Text tooltipText; // 控制大小
    private Text contentText; // 显示内容

    private CanvasGroup canvasGroup;
    private float targetAlpha = 0;
    public float smoothing = 1; // 控制显隐的渐变速度

    private void Start()
    {
        tooltipText = GetComponent<Text>();
        contentText = transform.FindChild("Content").GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        // 渐显渐隐
        if (canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, smoothing * Time.deltaTime);
            if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < 0.01f)
            {
                canvasGroup.alpha = targetAlpha;
            }
        }
    }

    // 显示ToolTip
    public void Show(string text)
    {
        tooltipText.text = text;
        contentText.text = text;
        targetAlpha = 1;
    }

    // 隐藏ToolTip
    public void Hide()
    {
        targetAlpha = 0;
    }

    // 设置位置，跟随鼠标移动
    public void SetPosition(Vector3 position)
    {
        transform.localPosition = position;
    }
}
