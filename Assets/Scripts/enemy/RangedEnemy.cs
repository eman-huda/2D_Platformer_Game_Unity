using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float colliderDistance = 1f;
    [SerializeField] private float range = 3f;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    [Header("Shooting")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;

    [Header("Other")]
    [SerializeField] private int damage = 1;
    [SerializeField] private enemyPatrolling enemyPatrol;
    [SerializeField] private Animator anim;

    private float cooldownTimer = Mathf.Infinity;
    private Transform player;
    private Health playerHealth;

    private void Awake()
    {
        if (anim == null) anim = GetComponent<Animator>();
        if (enemyPatrol == null) enemyPatrol = GetComponent<enemyPatrolling>();
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();
        cooldownTimer = attackCooldown;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight())
        {
            if (enemyPatrol != null) enemyPatrol.enabled = false;
            if (anim != null) anim.SetBool("moving", false);

            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0f;
                Shoot();
            }
        }
        else
        {
            if (enemyPatrol != null) enemyPatrol.enabled = true;
            if (anim != null) anim.SetBool("moving", true);
        }
    }

    public void Shoot()
    {
        if (firePoint == null) return;
        if (FireballPool.Instance == null)
        {
            Debug.LogError("FireballPool not found in scene.");
            return;
        }

        if (anim != null) anim.SetTrigger("shoot");

        GameObject proj = FireballPool.Instance.GetFireball();
        
        
        proj.transform.position = firePoint.position;
        proj.transform.rotation = Quaternion.identity;

        GameObject proj1 = FireballPool.Instance.GetFireball();
        proj1.transform.rotation = Quaternion.identity;
       

        

        float dir = Mathf.Sign(transform.localScale.x);
        proj1.transform.position = firePoint.position + new Vector3(2 * dir, 0, 0);
        Projectile pScript = proj.GetComponent<Projectile>();
        if (pScript != null)
        {
            pScript.SetDirection(dir);
            pScript.movementSpeed = projectileSpeed;
        }
        Projectile p1Script = proj1.GetComponent<Projectile>();
        if (p1Script != null)
        {
            p1Script.SetDirection(dir);
            p1Script.movementSpeed = projectileSpeed;
        }
        proj.SetActive(true);
        proj1.SetActive(true);
    }

    private bool PlayerInSight()
    {
        Vector2 castOrigin = boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance;
        Vector2 castSize = new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y);

        RaycastHit2D hit = Physics2D.BoxCast(castOrigin, castSize, 0f, Vector2.left, 0f, playerLayer);
        if (hit.collider != null)
        {
            player = hit.transform;
            playerHealth = player.GetComponent<Health>();
           
            return true;
        }

        player = null;
        playerHealth = null;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (boxCollider == null) return;
        Gizmos.color = Color.yellow;
        Vector3 drawPos = boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance;
        Vector3 drawSize = new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z);
        Gizmos.DrawWireCube(drawPos, drawSize);
        if (firePoint != null) Gizmos.DrawSphere(firePoint.position, 0.05f);
    }
}
