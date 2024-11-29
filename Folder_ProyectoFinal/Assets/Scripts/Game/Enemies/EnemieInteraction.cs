using UnityEngine;

public class EnemieInteraction : MonoBehaviour
{
    public float interactionRadius = 1f;
    public Transform player;
    public EnemyPatrol enemyMovement;
    private bool playerClose = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = true;
            enemyMovement.StartChase();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = false;
            enemyMovement.StopChase();
        }
    }
}