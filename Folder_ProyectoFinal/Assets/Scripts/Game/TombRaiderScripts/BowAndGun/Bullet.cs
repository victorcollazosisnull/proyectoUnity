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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            EnemieLife enemyLife = other.GetComponent<EnemieLife>();
            if (enemyLife != null)
            {
                enemyLife.TakeDamage(damage);
            }
            Destroy(gameObject); 
        }
    }
}