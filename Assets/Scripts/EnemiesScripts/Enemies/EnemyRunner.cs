using UnityEngine;

public class EnemyRunner : Enemy
{
    private bool m_IsLeft;
    private Vector2 m_Direction;

    private void FlipHandle()
    {
        if (m_Direction.x < 0) m_IsLeft = true;
        else if (m_Direction.x > 0) m_IsLeft = false;

        if (m_IsLeft) gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
        else gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    protected override void OnPlayerDetected()
    {
        m_Direction = (m_Player.gameObject.transform.position - transform.position).normalized;

        FlipHandle();

        m_Anim.SetBool("Attack", true);

        m_RB2D.velocity = new Vector2(m_Direction.x * m_WalkSpeed, m_RB2D.velocity.y);
    }

    protected override void OnPlayerOutOfRange()
    {
        m_Anim.SetBool("Attack", false);
    }
}
