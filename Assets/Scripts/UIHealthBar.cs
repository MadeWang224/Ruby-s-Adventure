using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Image mask;
    private float originalSize;
    public static UIHealthBar instance { get; private set; }
    public bool hasTask;
    public int fixedNum;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //                   大小基于     矩形
        originalSize = mask.rectTransform.rect.width;//mask大小基于矩形的width
    }
    /// <summary>
    /// 血条UI填充显示
    /// </summary>
    /// <param name="fillPercent">填充百分比</param>
    public void SetValue(float fillPercent)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            originalSize * fillPercent);
    }
}
