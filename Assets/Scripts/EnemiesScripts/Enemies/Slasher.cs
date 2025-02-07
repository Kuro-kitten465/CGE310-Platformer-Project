using System.Collections;
using UnityEngine;

public class Slasher : MonoBehaviour, IEnemy
{
    [SerializeField] private Transform m_StartPointOBJ;
    [SerializeField] private Transform m_EndPointOBJ;
    [SerializeField] private float m_TravelSpeed = 3f;
    [SerializeField] private bool m_WaitWhenReachTarget = false;
    [SerializeField] private float m_WaitTime = 2f;

    private Vector2 m_StartPoint;
    private Vector2 m_EndPoint;

    private Vector2 m_CurrentTarget;
    private bool m_IsMoving = true;

    private void Awake()
    {
        m_StartPoint = m_StartPointOBJ.transform.position; 
        m_EndPoint = m_EndPointOBJ.transform.position;
    }

    private void Start()
    {
        m_CurrentTarget = m_EndPoint;
        StartCoroutine(MoveBetweenPoints());
    }

    private IEnumerator MoveBetweenPoints()
    {
        while (true)
        {
            if (m_IsMoving)
            {
                transform.position = Vector2.MoveTowards(transform.position, m_CurrentTarget, m_TravelSpeed * Time.deltaTime);

                // Check if reached the target
                if (Vector2.Distance(transform.position, m_CurrentTarget) < 0.1f)
                {
                    if (m_WaitWhenReachTarget)
                    {
                        m_IsMoving = false;
                        yield return new WaitForSeconds(m_WaitTime);
                        m_IsMoving = true;
                    }

                    transform.position = m_CurrentTarget;

                    // Swap target
                    m_CurrentTarget = (m_CurrentTarget == m_StartPoint) ? m_EndPoint : m_StartPoint;
                }
            }

            yield return null; // Wait for the next frame
        }
    }

    public void OnPlayerTrigger(PlayerManager player)
    {
        // Add attack or interaction logic here
    }
}
