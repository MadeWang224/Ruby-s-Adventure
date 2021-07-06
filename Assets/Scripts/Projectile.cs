using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private float time = 0;
    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        time += Time.deltaTime;
        if(time>=5)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// 射击
    /// </summary>
    /// <param name="direction">方向</param>
    /// <param name="force">力</param>
    public void Launch(Vector2 direction,float force)
    {
        rigidbody2d.AddForce(direction * force);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if(enemyController!=null)
        {
            enemyController.Fix();
        }
        Destroy(gameObject);
    }
}
