using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwordController : MonoBehaviour
{
    [Header("References")]
    public StarterAssetsInputs input;
    public Camera cam;
    public GameObject arms;
    public GameObject sword;
    public GameObject hitEffect;
    public AudioSource audioSource;
    public AudioClip swordSwing;
    public AudioClip hitSound;

    [Header("Animation")]
    public Animator armsAnimator;

    public const string IDLE = "Idle";
    public const string ATTACK1 = "Attack 1";
    public const string ATTACK2 = "Attack 2";

    private string currentAnimationState;
    private int attackCount;

    [Header("Sword State")]
    public bool hasSword = true;
    public bool swordEquipped = false;

    [Header("Attack")]
    public float attackDistance = 3f;
    public float attackCooldown = 0.4f;
    public int attackDamage = 1;
    public LayerMask attackLayer;

    private float nextAttackTime;

    private void Start()
    {
        if (sword != null)
        {
            sword.SetActive(swordEquipped);
        }
    }

    private void Update()
    {
        if (input == null)
        {
            return;
        }

        if (input.equipSword)
        {
            ToggleSword();
            input.equipSword = false;
        }

        if (input.attack)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                TryAttack();
            }

            input.attack = false;
        }
    }

    private void ToggleSword()
    {
        if (!hasSword)
        {
            return;
        }

        swordEquipped = !swordEquipped;

        if (sword != null)
        {
            sword.SetActive(swordEquipped);
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (currentAnimationState == newState)
        {
            return;
        }

        currentAnimationState = newState;
        armsAnimator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    }

    private void TryAttack()
    {
        if (!hasSword || !swordEquipped)
        {
            return;
        }

        if (Time.time < nextAttackTime)
        {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;

        if (armsAnimator != null)
        {
            if (attackCount == 0)
            {
                ChangeAnimationState(ATTACK1);
                attackCount++;
            }
            else
            {
                ChangeAnimationState(ATTACK2);
                attackCount = 0;
            }
        }

        if (audioSource != null && swordSwing != null)
        {
            audioSource.PlayOneShot(swordSwing);
        }

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        {
            if (audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }

            if (hitEffect != null)
            {
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            hit.collider.SendMessage("TakeDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
        }
    }
}