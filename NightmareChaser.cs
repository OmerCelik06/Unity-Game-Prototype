using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NightmareChaser : MonoBehaviour
{
    [Header("Oyuncu ve Algýlama")]
    public Transform player;
    public float detectionRadius = 8.0f;

    [Header("Rastgele Arama Ayarlarý")]
    public float wanderRadius = 10.0f;
    public float wanderTimer = 4.0f;

    [Header("Korkunçluk Ayarlarý")]
    public float pulseSpeed = 15.0f;

    private NavMeshAgent agent;
    private Vector3 originalScale;
    private float timer;

    void Start()
    {
        originalScale = transform.localScale;
        agent = GetComponent<NavMeshAgent>();

        agent.speed = 2.5f;
        agent.angularSpeed = 200f;

        // Timer'ý baþlat
        timer = wanderTimer;
    }

    void Update()
    {
        float scaleJitter = 1.0f + (Mathf.Sin(Time.time * pulseSpeed) * 0.15f);
        transform.localScale = originalScale * scaleJitter;

        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);

                timer = 0;
            }
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}