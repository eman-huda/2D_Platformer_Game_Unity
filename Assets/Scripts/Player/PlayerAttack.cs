using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class playerAttack : MonoBehaviour
{
    private PlayerMovement  playerMovement;
    private Animator animator;
    private float coolDownTimer = 0;
    public float coolDownTime = 0.2f;

    public GameObject fireBallPrefab;
    public GameObject firePoint;
    public AudioClip fireballClip;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerMovement.canAttack() && coolDownTimer > coolDownTime)
        {
            audioSource.PlayOneShot(fireballClip);
            attack();
        }
        coolDownTimer += Time.deltaTime;
    }
    //UI Button
    public void attackUI()
    {
        if (playerMovement.canAttack() && coolDownTimer > coolDownTime)
        {
            audioSource.PlayOneShot(fireballClip);
            attack();
        }
        coolDownTimer += Time.deltaTime;
    }
    private void attack()
    {
        coolDownTimer = 0f;
        animator.SetTrigger("attack");

        GameObject fireball = FireballPool.Instance.GetFireball();
        fireball.transform.position = firePoint.transform.position;
        fireball.GetComponent<Projectile>().SetDirection(transform.localScale.x);
        fireball.SetActive(true);
    }

}