using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public float damage = 20f;

    private void Start()
    {
        Destroy(gameObject, lifeTime); 
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            EnemyIA enemyIA = collision.gameObject.GetComponent<EnemyIA>(); 

            if (enemyIA != null)
            {
                enemyIA.TakeDamage(); 
            }

            Destroy(gameObject); 
        }
        else if (collision.gameObject.CompareTag("Boss")) 
        {
            BossController bossController = collision.gameObject.GetComponent<BossController>(); 

            if (bossController != null)
            {
                bossController.TakeDamage(); 
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("destroy")) 
        {
            Destroy(this.gameObject);
        }
    }
}