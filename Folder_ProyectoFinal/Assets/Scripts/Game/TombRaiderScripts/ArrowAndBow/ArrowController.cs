using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float bodyDamage = 20f; 
    public float headshotDamage = 100f; 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            EnemieLife enemy = collision.gameObject.GetComponent<EnemieLife>();

            if (enemy != null)
            {
                enemy.TakeDamage(bodyDamage); 
            }

            Destroy(this.gameObject); 
        }
        else if (collision.gameObject.CompareTag("head"))
        {
            EnemieLife enemy = collision.transform.root.GetComponent<EnemieLife>();

            if (enemy != null)
            {
                enemy.TakeDamage(headshotDamage); 
            }
            Destroy(this.gameObject); 
        }
    }
}