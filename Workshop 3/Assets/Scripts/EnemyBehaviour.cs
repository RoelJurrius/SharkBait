using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBehaviour : MonoBehaviour
{
    private enum State { Idle, Chase, Attack }

    [Header("Ranges")]
    [SerializeField] private float chaseRange = 50f;
    [SerializeField] private float attackRange = 1.6f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private bool faceMovementDirection = true;

    [Header("Damage")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackCooldown = 1.0f;

    [Header("Target")]
    [SerializeField] private Transform player; // drag&drop of auto-find

    private Rigidbody rb;
    private State state = State.Idle;
    private float lastAttackTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void Start()
    {
        TryFindPlayer();
        Debug.Log(player != null ? $"Enemy: player gevonden: {player.name}" : "Enemy: GEEN player gevonden (tag 'Player'?)");
    }

    private void TryFindPlayer()
    {
        if (player != null) return;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    // Update: logica/overgangen (mag in Update)
    private void Update()
    {
        if (player == null)
        {
            // Als player later gespawned/ingeladen wordt, probeer opnieuw
            TryFindPlayer();
            state = State.Idle;
            return;
        }

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange) state = State.Attack;
        else if (dist <= chaseRange) state = State.Chase;
        else state = State.Idle;

        if (state == State.Attack)
            AttackPlayer(); // damage/cooldown is niet physics-gevoelig
    }

    // FixedUpdate: physics movement (Rigidbody)
    private void FixedUpdate()
    {
        if (player == null) return;

        if (state == State.Chase)
            ChasePlayerRb();
    }

private bool CanMove(Vector3 dir, float distance)
{
    CapsuleCollider col = GetComponent<CapsuleCollider>();
    if (col == null) return true;

    Vector3 p1 = transform.TransformPoint(col.center + Vector3.up * (col.height * 0.5f - col.radius));
    Vector3 p2 = transform.TransformPoint(col.center + Vector3.down * (col.height * 0.5f - col.radius));

    return !Physics.CapsuleCast(p1, p2, col.radius, dir, distance);
}

private void ChasePlayerRb()
{
    Vector3 toPlayer = player.position - rb.position;
    toPlayer.y = 0f;
    if (toPlayer.sqrMagnitude < 0.0001f) return;

    Vector3 dir = toPlayer.normalized;
    float step = moveSpeed * Time.fixedDeltaTime;

    if (CanMove(dir, step))
        rb.MovePosition(rb.position + dir * step);

    if (faceMovementDirection)
        rb.MoveRotation(Quaternion.LookRotation(dir, Vector3.up));
}

    private void AttackPlayer()
    {
        // Kijk naar target
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.001f)
            transform.forward = lookDir.normalized;

        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;

        Health hp = player.GetComponent<Health>();
        if (hp != null) hp.TakeDamage(damage);
    }
}