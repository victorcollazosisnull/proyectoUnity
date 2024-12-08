using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    private Transform player;
    public int maxHits = 3;
    private int currentHits = 0;
    public float chaseSpeed = 3f;

    private Animator animator;
    private bool isChasing = false;
    private bool isAttacking = false;

    [Header("Attack Settings")]
    public float attackRadius = 1.5f;  // Rango para iniciar el ataque
    public float detectionRadius = 5f; // Rango de detección para empezar a perseguir

    // Añadir variables para manejo de ataque
    private float attackCooldown = 1f;  // Tiempo entre ataques
    private float lastAttackTime = 0f;  // Último ataque realizado

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Si está dentro del rango de ataque, atacar
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                AttackPlayer();
            }
            else if (distanceToPlayer > attackRadius)
            {
                // Perseguir al jugador si no está dentro del rango de ataque
                ChasePlayer();
            }

            // Si se aleja fuera del rango de persecución, detener la persecución
            if (distanceToPlayer > detectionRadius)
            {
                StopChase();
            }
        }
        else
        {
            Patrol(); // Si no está persiguiendo, patrullar
        }
    }

    private void Patrol()
    {
        // Aquí puede ir la lógica para patrullar entre nodos o puntos
        // Ejemplo simple de movimiento hacia un destino
        if (player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRadius && !isChasing)
        {
            StartChase();
        }
    }

    public void StartChase()
    {
        isChasing = true;
    }

    public void StopChase()
    {
        isChasing = false;
        isAttacking = false;
        // Volver a patrullar o realizar otra acción
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            // Moverse hacia el jugador
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * chaseSpeed * Time.deltaTime;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    private void AttackPlayer()
    {
        // Evitar ataques continuos antes de tiempo
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        isAttacking = true;
        animator.SetTrigger("Attack");

        // Registrar el tiempo del último ataque
        lastAttackTime = Time.time;

        // Verificar si el jugador aún está dentro del rango de ataque después de la animación
        Invoke("OnAttackHit", 0.5f); // Aquí puedes ajustar el tiempo para que se dispare el daño después de un tiempo
    }

    private void OnAttackHit()
    {
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRadius)
        {
            // Aquí activas el daño al jugador
            // Puedes utilizar un evento o algo similar para manejar el daño al jugador
            Debug.Log("Player is hit by enemy attack.");
        }

        // Terminar el ataque
        EndAttack();
    }

    private void EndAttack()
    {
        isAttacking = false;
    }
}