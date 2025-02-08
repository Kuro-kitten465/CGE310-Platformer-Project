using UnityEngine;
using UnityEngine.InputSystem;
using KuroNeko.Utilities.DesignPattern;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_RB2D;

    [Header("Movement Settings")]
    public float m_MoveSpeed = 5f;
    public float m_JumpForce = 10f;
    [SerializeField] float m_ItemSpeedPower = 1.25f;
    [SerializeField] private AudioSource m_AttackAudio;
    [Header("Ground Check")]
    [SerializeField] private Transform m_GroundCheck;
    [SerializeField] private Transform m_LeftWallCheck;
    [SerializeField] private Transform m_RightWallCheck;
    [SerializeField] private float m_GroundCheckRadius = 0.2f;
    [SerializeField] private LayerMask m_GroundLayer;

    private bool m_IsGrounded;
    private bool m_IsNearWall;
    private bool m_IsAttacking = false;
    private bool m_IsLeft = false;
    private bool m_IsWalk = false;
    public bool IsSpeedUp = false;

    private Vector2 m_MoveAxis;
    private Animator m_Anim;
    private float m_CurrentSpeed
    {
        get
        {
            if (IsSpeedUp) return m_MoveSpeed * m_ItemSpeedPower * m_MoveAxis.x;
            else return m_MoveSpeed * m_MoveAxis.x;
        }
    }

    private void Awake()
    {
        m_RB2D = GetComponent<Rigidbody2D>();
        m_Anim = GetComponent<Animator>();
        StaticEventBus.Register(KeysContainer.PlayerAttack, OnAttackingHandle);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGameWin) return;

        if (!m_IsAttacking)
            WalkHandle();

        if (m_IsGrounded)
            m_Anim.SetBool("IsJump", false);
    }

    private void Update()
    {
        m_IsGrounded = Physics2D.OverlapCircle(m_GroundCheck.position, m_GroundCheckRadius, m_GroundLayer);
        m_IsNearWall = Physics2D.OverlapCircle(m_LeftWallCheck.position, m_GroundCheckRadius, m_GroundLayer) ||
                        Physics2D.OverlapCircle(m_RightWallCheck.position, m_GroundCheckRadius, m_GroundLayer);
    }

    private void WalkHandle()
    {
        if (m_MoveAxis.x < 0) m_IsLeft = true;
        else if (m_MoveAxis.x == 0 && m_IsLeft) m_IsLeft = true;
        else m_IsLeft = false;

        if (m_IsLeft) gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
        else gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);

        m_Anim.SetBool("IsWalk", m_IsWalk);

        m_RB2D.velocity = !m_IsNearWall || m_IsGrounded ?
                            new Vector2(m_CurrentSpeed, m_RB2D.velocity.y) :
                            new Vector2(m_RB2D.velocity.x, m_RB2D.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_MoveAxis = context.ReadValue<Vector2>();
        m_IsWalk = m_MoveAxis.x != 0;
    }

    public void OnGoDown(InputAction.CallbackContext context)
    {
        Debug.Log("DownBTN Work!");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (m_IsGrounded)
            m_RB2D.velocity = new Vector2(m_RB2D.velocity.x, m_JumpForce);

        m_Anim.SetBool("IsJump", true);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!m_IsGrounded) return;

        if (context.started)
        {
            m_Anim.SetTrigger("IsAttack");
            m_AttackAudio.Play();
        }
    }

    public void OnAttackingHandle()
    {
        m_IsAttacking = !m_IsAttacking;
    }

    private void OnDestroy()
    {
        StaticEventBus.Unregister("Attack");
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_LeftWallCheck.transform.position, m_GroundCheckRadius);
        Gizmos.DrawWireSphere(m_RightWallCheck.transform.position, m_GroundCheckRadius);
        Gizmos.DrawWireSphere(m_GroundCheck.transform.position, m_GroundCheckRadius);
    }
#endif
}
