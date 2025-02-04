using UnityEngine;

public class EnemyTest : Enemy
{
    public override void OnPlayerTrigger(PlayerManager player)
    {
        m_Player = player;
    }

    protected override void OnPlayerDetected()
    {
        Debug.Log("Found Player");
        Vector2 direction = (m_Player.gameObject.transform.position - transform.position).normalized;
        m_Rigidbody2D.velocity = new Vector2(direction.x * 2, m_Rigidbody2D.velocity.y);
    }
}
