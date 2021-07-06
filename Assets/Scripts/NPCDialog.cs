using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialog : MonoBehaviour
{
    /// <summary>
    /// 对话框
    /// </summary>
    public GameObject dialogBox;
    /// <summary>
    /// 持续时间
    /// </summary>
    public float displayTime = 4.0f;
    /// <summary>
    /// 时间计算
    /// </summary>
    private float time;
    /// <summary>
    /// 文本内容
    /// </summary>
    public Text dialogText;
    public AudioSource audioSource;
    public AudioClip completeTaskClip;
    private void Start()
    {
        dialogBox.SetActive(false);
        time = 0;
    }
    private void Update()
    {
        if(time>0)
        {
            time -= Time.deltaTime;
            if(time<=0)
            {
                dialogBox.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 显示对话框
    /// </summary>
    public void DisplayDialog()
    {
        time = displayTime;
        dialogBox.SetActive(true);
        UIHealthBar.instance.hasTask = true;
        if(UIHealthBar.instance.fixedNum>=5)
        {
            dialogText.text = "任务完成";
            audioSource.PlayOneShot(completeTaskClip);
        }
    }
}
