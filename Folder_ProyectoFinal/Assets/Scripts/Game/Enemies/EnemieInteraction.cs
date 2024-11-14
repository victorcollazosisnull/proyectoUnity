using UnityEngine;

public class EnemieInteraction : MonoBehaviour
{
    public float interactionRadius = 3f;
    public Transform player;
    public EnemyPatrol enemyMovement;
    private bool playerClose = false;

    private void Update()
    {
        if (playerClose)
        {
            if (Vector3.Distance(transform.position, player.position) < interactionRadius)
            {
                enemyMovement.StartChasing(); 
            }
        }
        else
        {
            enemyMovement.StopChasing(); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = false;
        }
    }
}