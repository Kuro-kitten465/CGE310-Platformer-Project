using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    /*public enum PlayerLook 
    {
        None, TopLeft, TopRight, DownLeft, DownRight
    }*/

    private Rigidbody2D m_RB2D;

    [Header("Movement Settings")]
    public float m_MoveSpeed = 5f;
    public float m_JumpForce = 10f;

    [Header("Ground Check")]
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_LeftWallCheck;
    [SerializeField] private Transform m_RightWallCheck;
    [SerializeField] private float m_GroundCheckRadius = 0.2f;
    [SerializeField] private LayerMask m_GroundLayer;

    [Header("Attack Config")]
    [SerializeField] private GameObject AttackObj;

    // Private Members
    //public PlayerLook m_PlayerLookAt = PlayerLook.None;

    private bool m_IsGrounded;
    private bool m_IsNearWall;
    private bool m_IsShooting = false;

    private Vector2 m_MoveAxis;
    private Vector2 m_LookAxis;

    private void Awake()
    {
        m_RB2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        m_RB2D.velocity = !m_IsNearWall ? new Vector2(m_MoveAxis.x, m_RB2D.velocity.y) :
                                            new Vector2(m_RB2D.velocity.x, m_RB2D.velocity.y);

        if (m_IsShooting) Debug.Log("Shoot");
    }

    private void Update()
    {
        m_IsGrounded = Physics2D.OverlapCircle(m_GroundCheck.position, m_GroundCheckRadius, m_GroundLayer);
        m_IsNearWall = Physics2D.OverlapCircle(m_LeftWallCheck.position, m_GroundCheckRadius, m_GroundLayer) ||
                        Physics2D.OverlapCircle(m_RightWallCheck.position, m_GroundCheckRadius, m_GroundLayer);

        /*if (m_LookAxis.y == Vector2.up.y)
        {
            if (Mathf.Clamp(m_MoveAxis.x, -1, 1) == Vector2.left.x) m_PlayerLookAt = PlayerLook.TopLeft;
            else m_PlayerLookAt = PlayerLook.TopRight;
        }
        else if (m_LookAxis.y == Vector2.down.y)
        {
            if (Mathf.Clamp(m_MoveAxis.x, -1, 1) == Vector2.left.x) m_PlayerLookAt = PlayerLook.DownLeft;
            else m_PlayerLookAt = PlayerLook.DownRight;
        }
        else
        {
            m_PlayerLookAt = PlayerLook.None;
        }*/
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_MoveAxis = context.ReadValue<Vector2>() * m_MoveSpeed;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        m_LookAxis = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (m_IsGrounded)
            m_RB2D.velocity = new Vector2(m_RB2D.velocity.x, m_JumpForce);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        
    }
}
