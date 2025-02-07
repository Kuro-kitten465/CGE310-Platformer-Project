using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    private PlayerManager m_Player;

    private void Start()
    {
        m_Player = GetComponentInParent<PlayerManager>();    
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent<IEnemy>(out var enemy))
        {
            enemy.OnPlayerTrigger(m_Player);
        }
    }
}
