using System;
using System.Collections;
using UnityEngine;

public class EnemyGunner : Enemy
{
    [Header("Enemy Config")]
    [SerializeField] private bool m_CanChangeDirection = false;

    private Vector2 m_Direction;
    private bool m_IsLeft = true;

    [Header("Shooting Config")]
    [SerializeField] private GameObject m_BulletPrefab;
    [SerializeField] private Transform m_FirePoint;
    [SerializeField] private float m_FireRate = 1.5f;
    [SerializeField] private float m_BulletVelocity = 5f;
    [SerializeField] private AudioSource m_Audio;

    private bool m_CanShoot = true;

    protected override void DetectPlayer()
    {
        var hit = Physics2D.OverlapCircle(transform.position, m_DetectionRadius, LayerMask.GetMask("Player"));

        if (hit != null && hit.TryGetComponent<PlayerManager>(out var player))
        {
            m_Player = player;

            OnPlayerDetected();
        }
        else
        {
            m_Player = null;
        }
    }

    protected override void OnPlayerDetected()
    {
        m_Direction = (m_Player.gameObject.transform.position - transform.position).normalized;

        if (m_CanChangeDirection) FlipHandle();

        if (m_CanShoot)
        {
            Invoke(nameof(AnimTrigger), 0f);
            Invoke(nameof(AnimTrigger), 0.2f);
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        m_CanShoot = false;

        // Instantiate bullet
        GameObject bullet = Instantiate(m_BulletPrefab, m_FirePoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(m_IsLeft ? -m_BulletVelocity : m_BulletVelocity, 0); // Move left or right
        m_Audio.Play();

        yield return new WaitForSeconds(m_FireRate);
        m_CanShoot = true;
    }

    private void AnimTrigger()
    {
        m_Anim.SetTrigger("Shoot");
    }

    protected override void OnPlayerOutOfRange()
    {
        // Stop shooting if player is out of range
        StopAllCoroutines();
        m_CanShoot = true;
    }

    private void FlipHandle()
    {
        if (m_Direction.x < 0) m_IsLeft = true;
        else if (m_Direction.x > 0) m_IsLeft = false;

        if (m_IsLeft) gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
        else gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
