
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Firetrap : EnemyDamage
{
    [Header ("Firetrap Timers")]
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    private bool triggered; //when the trap gets triggered
    private bool active; //when trap is active and can hurt the player

    private void Awake() {
        this.anim = GetComponent<Animator>();
        this.spriteRend = GetComponent<SpriteRenderer>();
    }

    private new void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (!triggered) {
                StartCoroutine(ActivateFiretrap()); //trigger the firetrap
            }
            if (active) base.OnTriggerEnter2D(collision);
        }
    }

    private IEnumerator ActivateFiretrap() {
        //turn the sprite red to notify the player and trigger the trap
        triggered = true;
        spriteRend.color = Color.red;

        //wait for delay, activate trap, turn on animation, return color back to normal
        yield return new WaitForSeconds(activationDelay);
        spriteRend.color = Color.white; //turn the sprite back to its initial color
        active = true; //trap is active
        anim.SetBool("Activated", true);

        //wait until X seconds, deactivate trap and reset all variables and animator
        yield return new WaitForSeconds(activeTime);
        //deactivate and detrigger the trap
        active = false;
        triggered = false;
        anim.SetBool("Activated", false);
    }
}