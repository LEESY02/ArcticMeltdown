using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float attackCooldown; // Edited by SY
    [SerializeField] private Transform firePoint;
    public GameObject[] snowballs;

    [Header ("Audio")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float attackVolume;
    private Tracker tracker;
    private Animator anim;
    private Player playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        tracker = FindFirstObjectByType<Tracker>();
        attackCooldown = tracker.attackCooldown;
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<Player>();
    }

    private void Update() {
        if (playerMovement.CanAttack() && Input.GetMouseButton(0) && cooldownTimer > attackCooldown) { // left click to attack
            Attack();
        }
        cooldownTimer += Time.deltaTime;
    }

    private void Attack() {
        SoundManager.instance.PlaySound(attackSound, attackVolume);
        anim.SetTrigger("Attack");
        cooldownTimer = 0;
        //pool Snowball
        snowballs[FindSnowball()].transform.position = firePoint.position;
        snowballs[FindSnowball()].GetComponent<Snowball>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindSnowball() {
        for (int i = 0; i < snowballs.Length; i++) {
            if (!snowballs[i].activeInHierarchy) {
                return i;
            }
        }
        return 0;
    }

    public void RefreshAttackCooldown()
    {
        this.attackCooldown = tracker.attackCooldown;
    }
}
