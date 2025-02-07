using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Enemy : MonoBehaviour, IEnemy
{
    [Header("Enemy Stats")]
    [SerializeField] protected float m_WalkSpeed = 1f;
    [SerializeField] protected float m_DetectionRadius = 5f;

    [Header("Enemy Config")]
    [SerializeField] protected bool UseDetection = false;
    [SerializeField] protected Int16 m_Score = 200;

    protected Rigidbody2D m_RB2D;
    protected Collider2D m_Collider2D;
    protected PlayerManager m_Player;
    protected Animator m_Anim;

    protected virtual void Awake()
    {
        m_Collider2D = GetComponent<Collider2D>();
        m_RB2D = GetComponent<Rigidbody2D>();
        m_Anim = GetComponent<Animator>();
    }

    public void OnPlayerTrigger(PlayerManager player)
    {
        player.OnKillEnemy(m_Score);
        Destroy(gameObject);
    }

    protected abstract void OnPlayerDetected();
    protected abstract void OnPlayerOutOfRange();

    protected virtual void FixedUpdate()
    {
        if (UseDetection) DetectPlayer();
    }

    protected virtual void DetectPlayer()
    {
        var hit = Physics2D.OverlapCircle(transform.position, m_DetectionRadius, LayerMask.GetMask("Player"));

        if (hit != null && hit.TryGetComponent<PlayerManager>(out var player))
        {
            m_Player = player;

            var direction = (m_Player.gameObject.transform.position - transform.position).normalized;
            var rayHit = Physics2D.Raycast(transform.position, direction, m_DetectionRadius, LayerMask.GetMask("Player", "Obstacles"));

            if (rayHit.collider != null && rayHit.collider.gameObject.GetComponent<PlayerManager>())
            {
                OnPlayerDetected();
            }
        }
        else
        {
            m_Player = null;
            OnPlayerOutOfRange();
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