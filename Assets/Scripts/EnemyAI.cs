using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;

    [Header("Ranges")]
    public float detectRange = 12f;
    public float attackRange = 2.2f;
    public float idleRange = 15f;

    [Header("Attack")]
    public float attackCooldown = 1.2f;
    public int attackDamage = 1;
    public float damageDelay = 0.35f;
    float nextAttackTime;

    [Header("Facing")]
    [Tooltip("How fast the body turns toward the move / look direction.")]
    [SerializeField] float rotationSpeed = 12f;
    [Tooltip("If set, we read this transform's local yaw at startup to cancel mesh-vs-root misalignment. If null and the option below is on, we use the first child of this object.")]
    [SerializeField] Transform meshFacingSkewSource;
    [Tooltip("When true, yaw offset is derived from meshFacingSkewSource (or first child) so the model faces the player even when the FBX root is rotated in the prefab.")]
    [SerializeField] bool deriveFacingYawFromChild = true;
    [Tooltip("Extra yaw (degrees) added after auto offset — use for fine-tuning only.")]
    [SerializeField] float modelForwardYawOffsetDegrees = 0f;

    float effectiveFacingYawOffsetDegrees;

    bool attacking = false;
    PlayerHealth playerHealth;

    enum State { Idle, Chase, Attack }
    State state;

    string currentAnim;

    const string IDLE = "Idle";
    const string RUN = "Run";
    const string ATTACK_START = "AttackStart";
    const string ATTACK_RETURN = "AttackReturn";

    void Awake()
    {
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
        // Manual rotation in LateUpdate; leaving this on fights our facing logic and can look wrong with some rigs.
        if (agent != null)
            agent.updateRotation = false;
        // NavMesh + script facing: root motion must not add rotation/translation from clips onto the animated object.
        if (animator != null)
            animator.applyRootMotion = false;

        effectiveFacingYawOffsetDegrees = modelForwardYawOffsetDegrees;
        if (deriveFacingYawFromChild)
        {
            Transform src = meshFacingSkewSource != null
                ? meshFacingSkewSource
                : (transform.childCount > 0 ? transform.GetChild(0) : null);
            if (src != null)
            {
                // Child local Y rotates the mesh relative to this root; rotate the root the other way so world-facing matches the player.
                float childYaw = NormalizeSignedYawDegrees(src.localEulerAngles.y);
                effectiveFacingYawOffsetDegrees -= childYaw;
            }
        }
    }

    static float NormalizeSignedYawDegrees(float eulerY)
    {
        eulerY %= 360f;
        if (eulerY > 180f) eulerY -= 360f;
        if (eulerY < -180f) eulerY += 360f;
        return eulerY;
    }

    void Update()
    {
        if (playerHealth != null && playerHealth.IsDead())
        {
            Idle();
            return;
        }
        
        float distance = Vector3.Distance(transform.position, player.position);

        // ----------------------------
        // STATE DECISION (FIXED LOGIC)
        // ----------------------------

        if (distance > idleRange)
        {
            state = State.Idle;
        }
        else if (distance > attackRange)
        {
            state = State.Chase;
        }
        else
        {
            state = State.Attack;
        }

        // ----------------------------
        // EXECUTE STATE
        // ----------------------------

        switch (state)
        {
            case State.Idle:
                Idle();
                break;

            case State.Chase:
                Chase();
                break;

            case State.Attack:
                Attack();
                break;
        }
    }

    void LateUpdate()
    {
        HandleRotation();
    }

    void Idle()
    {
        agent.isStopped = true;
        agent.ResetPath();

        PlayAnim(IDLE);
    }

    void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        // Player left melee during windup / return — stop chained invokes and allow Run again.
        if (attacking)
        {
            CancelInvoke();
            attacking = false;
            currentAnim = "";
        }

        // Use desired velocity / remaining distance so we pick Run as soon as we chase, not only after velocity ramps (avoids sticking on attack clips).
        bool wantsMove = agent.velocity.sqrMagnitude > 0.01f
            || agent.desiredVelocity.sqrMagnitude > 0.01f
            || agent.remainingDistance > agent.stoppingDistance + 0.05f;

        if (wantsMove)
            PlayAnim(RUN);
        else
            PlayAnim(IDLE);
    }

    void Attack()
    {
        if (attacking)
            return;

        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + attackCooldown;

        CancelInvoke();
        attacking = true;
        agent.isStopped = true;

        PlayAnim(ATTACK_START);

        Invoke(nameof(DamagePlayer), damageDelay);
        Invoke(nameof(PlayReturn), 0.4f);
    }

    void DamagePlayer()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange + 0.5f)
            return;

        if (playerHealth != null && !playerHealth.IsDead())
        {
            bool damageApplied = playerHealth.TakeDamage(attackDamage);
        }
    }

    void PlayReturn()
    {
        PlayAnim(ATTACK_RETURN);
        Invoke(nameof(EndAttack), 0.4f);
    }

    void EndAttack()
    {
        attacking = false;
        // Must clear so PlayAnim(RUN) is not skipped (it compares to this string, not the Animator's real state).
        currentAnim = "";
    }

    void PlayAnim(string anim)
    {
        if (anim == ATTACK_START || anim == ATTACK_RETURN)
        {
            animator.Play(anim, 0, 0f); // FORCE restart attack
            currentAnim = anim;
            return;
        }

        if (currentAnim == anim) return;

        currentAnim = anim;
        animator.Play(anim);
    }

    void ResetAttackState()
    {
        // This allows re-triggering animation cleanly
        currentAnim = "";
    }

    void HandleRotation()
    {
        if (player == null || agent == null)
            return;

        // Prefer actual motion direction so the run cycle lines up with movement; fall back to player or next path point.
        Vector3 dir = agent.velocity.sqrMagnitude > 0.02f
            ? agent.velocity
            : (state != State.Idle
                ? player.position - transform.position
                : agent.steeringTarget - transform.position);

        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f)
            return;

        dir.Normalize();
        // World yaw so root + skewed child mesh aim at dir (see Awake effectiveFacingYawOffsetDegrees).
        Vector3 facingDir = (Quaternion.Euler(0f, effectiveFacingYawOffsetDegrees, 0f) * dir).normalized;
        Quaternion targetRot = Quaternion.LookRotation(facingDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
    }
}