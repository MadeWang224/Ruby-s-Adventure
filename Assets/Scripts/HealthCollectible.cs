using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip audioClip;

    public GameObject addHealthEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController rubyController = collision.GetComponent<RubyController>();
        if(rubyController!=null)
        {
            if(rubyController.CurrentHealth<rubyController.maxHealth)
            {
                rubyController.ChangeHealth(1);
                rubyController.PlaySound(audioClip);
                Instantiate(addHealthEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
