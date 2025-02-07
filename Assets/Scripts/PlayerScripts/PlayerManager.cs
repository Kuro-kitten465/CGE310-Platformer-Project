using System;
using System.Collections;
using KuroNeko.Utilities.DesignPattern;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Properties")]
    [SerializeField] protected byte m_CurrentPlayerHealth = 3;
    [SerializeField] protected byte m_MaxPlayerHealth = 255;
    [SerializeField] private float m_CollectRange = 1f;  // Range within which player detects items

    [Header("Detection Config")]
    [SerializeField] private LayerMask m_ItemLayer;

    [Header("Game Config")]
    [SerializeField] Transform m_RespawnPoint;
    [SerializeField] float m_ItemSpeedTime = 20f;

    public int PlayerHealth => m_CurrentPlayerHealth;
    public int PlayerScore => m_Score;

    public Transform Checkpoint;
    private int m_Score;
    private PlayerController m_PlayerCon;
    private float m_CurrentTime = 0f;

    private void Awake()
    {
        m_PlayerCon = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        Detection();
    }

    private void Detection()
    {
        var item = Physics2D.OverlapCircle(transform.position, m_CollectRange, m_ItemLayer);

        if (item == null) return;

        if (item.TryGetComponent<ICollectible>(out var collectible))
        {
            collectible.Collect(this);
        }

        if (item.TryGetComponent<IDestroyable>(out var destroyable))
        {
            destroyable.Destroy();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGameWin) return;

        if (collision.gameObject.GetComponent<IEnemy>() == null) return;

        OnReciveDamage();
    }

    public void SpeedUp()
    {
        if (m_PlayerCon.IsSpeedUp)
        {
            m_CurrentTime = m_ItemSpeedTime;
            return;
        }

        m_PlayerCon.IsSpeedUp = true;
        m_CurrentTime = m_ItemSpeedTime;
        StartCoroutine(SpeedTime());
    }

    IEnumerator SpeedTime()
    {
        while (m_CurrentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            m_CurrentTime--;
        }

        m_PlayerCon.IsSpeedUp = false;
    }

    public void RestoreHealth()
    {
        if (m_CurrentPlayerHealth < m_MaxPlayerHealth)
        {
            m_CurrentPlayerHealth++;
            StaticEventBus.Invoke(KeysContainer.AddHeart);
        }
    }

    private void OnReciveDamage()
    {
        m_CurrentPlayerHealth--;
        StaticEventBus.Invoke(KeysContainer.ReduceHeart);

        if (m_CurrentPlayerHealth <= 0)
            GameManager.Instance.OnGameOver(this);

        if (GameManager.Instance.IsGameOver) return;

        if (Checkpoint != null)
            gameObject.transform.position = Checkpoint.position;
        else
            gameObject.transform.position = m_RespawnPoint.position;
    }

    public void OnKillEnemy(Int16 score)
    {
        m_Score += score;
        StaticEventBus.Invoke(KeysContainer.ScoreUpdated);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_CollectRange);
    }
#endif
}