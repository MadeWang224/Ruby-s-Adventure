using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// 速度
    /// </summary>
    public float speed = 3;
    /// <summary>
    /// 机器人是否故障
    /// </summary>
    private bool broken = true;
    /// <summary>
    /// 轴向控制
    /// </summary>
    public bool Vertical;
    /// <summary>
    /// 方向控制
    /// </summary>
    private int direction = 1;
    /// <summary>
    /// 刚体
    /// </summary>
    private Rigidbody2D rigidbody2d;
    /// <summary>
    /// 动画组件
    /// </summary>
    private Animator animator;
    /// <summary>
    /// 无敌时间设置
    /// </summary>
    private float time = 0;
    public float changeTime = 3.0f;
    /// <summary>
    /// 烟雾特效
    /// </summary>
    private ParticleSystem smokeEffect;
    /// <summary>
    /// 子弹特效
    /// </summary>
    public GameObject launchEffect;

    private AudioSource audioSource;
    public AudioClip fixedSound;
    public AudioClip[] hitSounds;
    private void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        time = changeTime;
        smokeEffect = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if(!broken)
        {
            return;
        }
        Move();
        
    }
    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {
        time -= Time.deltaTime;
        if(time<=0)
        {
            direction = -direction;
            time = changeTime;
        }
        Vector2 position = rigidbody2d.position;
        //垂直
        if (Vertical)
        {
            position.y += Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);

        }
        else//水平
        {
            position.x += Time.deltaTime * speed * direction;
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }
        rigidbody2d.MovePosition(position);
    }
    /// <summary>
    /// 碰撞检测
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController rubyController = collision.gameObject.GetComponent<RubyController>();
        if (rubyController != null)
        {
            rubyController.ChangeHealth(-1);
        }
    }
    /// <summary>
    /// 修复
    /// </summary>
    public void Fix()
    {
        Instantiate(launchEffect, transform.position, Quaternion.identity);
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        int randomNum = Random.Range(0, 2);
        audioSource.Stop();
        audioSource.PlayOneShot(hitSounds[randomNum]);
        UIHealthBar.instance.fixedNum++;
    }
}
