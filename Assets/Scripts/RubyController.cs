using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    /// <summary>
    /// Ruby的速度
    /// </summary>
    public float speed;
    /// <summary>
    /// 刚体
    /// </summary>
    private Rigidbody2D rubyRigidbody;
    /// <summary>
    /// 动画组件
    /// </summary>
    private Animator animator;
    private Vector2 lookDirection = new Vector2(0, -1);
    /// <summary>
    /// 最大血量
    /// </summary>
    public int maxHealth;
    /// <summary>
    /// 当前血量
    /// </summary>
    public int CurrentHealth { private set; get; }
    /// <summary>
    /// 子弹预制件
    /// </summary>
    public GameObject projectilePrefab;
    /// <summary>
    /// 记录重生点
    /// </summary>
    private Vector3 respawnPosition;
    /// <summary>
    /// 计时
    /// </summary>
    private float time;
    public float intervalTime = 2.0f;
    /// <summary>
    /// 是否无敌
    /// </summary>
    public bool isInvincible = false;
    public AudioSource audioSource;
    public AudioSource walkAudioSource;
    public AudioClip playerHit;
    public AudioClip attackSoundClip;
    public AudioClip walkSound;
    private void Start()
    {
        rubyRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        CurrentHealth = maxHealth;
        respawnPosition = transform.position;
    }
    private void Update()
    {
        //移动
        Move();
        //攻击
        if(Input.GetKeyDown(KeyCode.K))
        {
            Launch();
        }
        Invincible();
        Talk();
    }
    /// <summary>
    /// Ruby移动方法
    /// </summary>
    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);
        if(!Mathf.Approximately(move.x,0)||!Mathf.Approximately(move.y,0))
        {
            //是否有输入
            lookDirection = move;
            lookDirection.Normalize();
            if(!walkAudioSource.isPlaying)
            {
                walkAudioSource.clip = walkSound;
                walkAudioSource.Play();
            }
        }
        else
        {
            walkAudioSource.Stop();
        }
        //动画控制
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        Vector2 position = this.transform.position;
        position += speed * move * Time.deltaTime;
        //在使用Rigidbody方法进行移动前,先计算变换后的位置
        //不能直接将Rigidbody的位置进行加法处理
        rubyRigidbody.MovePosition(position);
    }
    /// <summary>
    /// 血量变化
    /// </summary>
    /// <param name="value"></param>
    public void ChangeHealth(int value)
    {
        if(value<=0)
        {
            if(isInvincible)
            {
                return;
            }
            isInvincible = true;
            time = intervalTime;
            animator.SetTrigger("Hit");
            PlaySound(playerHit);
        }
        CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, maxHealth);
        if(CurrentHealth<=0)
        {
            Respawn();
        }
        UIHealthBar.instance.SetValue(CurrentHealth/(float)maxHealth);
    }
    /// <summary>
    /// 攻击
    /// </summary>
    private void Launch()
    {
        if(!UIHealthBar.instance.hasTask)
        {
            return;
        }
        GameObject projectileObject = Instantiate(projectilePrefab, 
            rubyRigidbody.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        animator.SetTrigger("Launch");
        PlaySound(attackSoundClip);
    }
    /// <summary>
    /// 重生
    /// </summary>
    private void Respawn()
    {
        ChangeHealth(maxHealth);
        transform.position = respawnPosition;
    }
    /// <summary>
    /// 无敌时间
    /// </summary>
    private void Invincible()
    {
        if(isInvincible)
        {
            time -= Time.deltaTime;
            if(time<=0)
            {
                isInvincible = false;
            }
        }
    }
    /// <summary>
    /// 与NPC对话
    /// </summary>
    private void Talk()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            RaycastHit2D hit = Physics2D.Raycast(rubyRigidbody.position + Vector2.up * 0.2f,
                lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if(hit.collider!=null)
            {
                NPCDialog npcDialog = hit.collider.GetComponent<NPCDialog>();
                if(npcDialog!=null)
                {
                    npcDialog.DisplayDialog();
                }
            }
        }
    }
    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
