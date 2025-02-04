using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Properties")]
    [SerializeField] protected byte m_PlayerHealth = 3;
    [SerializeField] protected byte m_PlayerATK = 1;
    [SerializeField] private float m_CollectRange = 1f;  // Range within which player detects items

    [Header("Detection Config")]
    [SerializeField] private LayerMask m_ItemLayer;

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

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_CollectRange);
    }
    #endif
}