using UnityEngine;

public class Bullet : MonoBehaviour, IEnemy
{
    //[SerializeField] private float m_Speed = 4f;
    [SerializeField] private float m_Lifetime = 2f;

    void Start()
    {
        Destroy(gameObject, m_Lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    public void OnPlayerTrigger(PlayerManager player)
    {
        Destroy(gameObject);
    }
}
