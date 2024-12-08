using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    private Transform player;
    private bool isChasing = false;
    public float chaseSpeed = 3f;

    public void SetPlayer(Transform target)
    {
        player = target;
    }

    public void StartChasingPlayer()
    {
        isChasing = true;
    }

    private void Update()
    {
        if (isChasing && player != null)
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * chaseSpeed * Time.deltaTime;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }
}