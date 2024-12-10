using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float bodyDamage = 20f;
    public float headshotDamage = 100f;
    public GameObject explosionEffectPrefab;
    public float explosionRadius = 5f;
    public float explosionForce = 1000f;
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
        else if (collision.gameObject.CompareTag("destroy"))
        {
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("tnt"))
        {
            ActivateExplosion(collision.gameObject);
        }
    }

    private void ActivateExplosion(GameObject tntObject)
    {
        if (explosionEffectPrefab != null)
        {
            GameObject explosion = Instantiate(explosionEffectPrefab, tntObject.transform.position, Quaternion.identity);

            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }
        }

        ApplyExplosionForce(tntObject.transform.position);
        Destroy(tntObject);

        Destroy(this.gameObject);
    }

    private void ApplyExplosionForce(Vector3 explosionPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("enemy"))
            {
                Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direction = colliders[i].transform.position - explosionPosition;
                    rb.AddForce(direction.normalized * explosionForce, ForceMode.Impulse);  
                }

                EnemieLife enemyLife = colliders[i].GetComponent<EnemieLife>();
                if (enemyLife != null)                              //
                {
                    enemyLife.TakeDamage(enemyLife.maxHealth);      //
                    enemyLife.Die();                                //
                }
            }
        }
    }
}