using UnityEngine;
using System.Collections;

public class FireTrap : MonoBehaviour
{
    private Animator anim;
    private bool active = false;
    public float activeTime = 10f;    // How long fire stays on
    public float offTime = 5f;        // How long fire stays off
    public int damage = 1;            // Damage to player
    public float damageCooldown = 1f; // Time between damage hits

    private bool canDamage = true;    // Prevent rapid damage

    private void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(FireRoutine());
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            // Fire off
            active = false;
            anim.SetTrigger("deactivate");
            yield return new WaitForSeconds(offTime);

            // Fire on
            anim.SetTrigger("activate");
            yield return new WaitForSeconds(0.5f); // wait for Hit animation
            active = true;
            yield return new WaitForSeconds(activeTime);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (active && canDamage && collision.CompareTag("Player"))
        {
            StartCoroutine(DealDamageWithCooldown(collision.GetComponent<Health>()));
        }
    }

    private IEnumerator DealDamageWithCooldown(Health playerHealth)
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }

        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
