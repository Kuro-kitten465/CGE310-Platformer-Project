using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour, IEnemy
{
    [Header("Enemy Stats")]
    [SerializeField] protected byte m_EnemyHealth = 3;
    [SerializeField] protected byte m_EnemyATK = 1;
    [SerializeField] protected float m_DetectionRadius = 5f;

    [Header("Enemy Config")]
    [SerializeField] protected bool UseDetection = false;

    protected Rigidbody2D m_Rigidbody2D;
    protected Collider2D m_Collider2D;
    protected PlayerManager m_Player;

    protected virtual void Awake()
    {
        m_Collider2D = GetComponent<Collider2D>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public abstract void OnPlayerTrigger(PlayerManager player);
    protected abstract void OnPlayerDetected();

    protected virtual void FixedUpdate()
    {
        if (UseDetection) DetectPlayer();
    }

    protected virtual void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, m_DetectionRadius, LayerMask.GetMask("Player"));

        if (hit != null && hit.TryGetComponent<PlayerManager>(out var player))
        {
            m_Player = player;

            Vector2 direction = (m_Player.gameObject.transform.position - transform.position).normalized;
            RaycastHit2D rayHit = Physics2D.Raycast(transform.position, direction, m_DetectionRadius, LayerMask.GetMask("Player", "Obstacles"));

            if (rayHit.collider != null && rayHit.collider.gameObject.GetComponent<PlayerManager>())
            {
                OnPlayerDetected();
            }
        }
        else
        {
            m_Player = null;
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_DetectionRadius);

        if (m_Player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, m_Player.gameObject.transform.position);
        }
    }
    #endif
}