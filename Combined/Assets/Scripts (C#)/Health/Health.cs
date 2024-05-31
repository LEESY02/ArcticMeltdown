using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] public float startingHealth;
    // [Header ("Camera")]
    // [SerializeField] private CameraController cam;
    [Header ("iFrames")]
    [SerializeField] private float invulnerabilityDuration;
    [SerializeField] private int numberOfFlashes; //number of times player will flash red before returning back to normal state

    [Header("Audio")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private float hurtVolume;
    [SerializeField] private AudioClip deadSound;
    [SerializeField] private float deadVolume;

    private SpriteRenderer spriteRend;
    private GameObject room;
    private GameObject startingPlatform;
    public float currentHealth {get; private set;} //all scripts can access the value, but ONLY this scrpt can set the value
    private Animator anim;

    private void Start() {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage) {
        currentHealth = Math.Clamp(currentHealth - damage, 0, startingHealth);
        if (currentHealth > 0) {
            //player hurt
            SoundManager.instance.PlaySound(hurtSound, hurtVolume);
            anim.SetTrigger("Hurt");
            //respawn player back to the start
            //Spawn();
            //iframes
            StartCoroutine(Invunerability());
        } else {
            anim.SetTrigger("Dead");
            spriteRend.color = Color.red;
            //player
            if (GetComponent<Player>() != null)
            {
                GetComponent<Player>().enabled = false;
                GetComponent<Rigidbody2D>().gravityScale = 0.2f; //player floats down
                GetComponent<CircleCollider2D>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
            }
            //enemy
            if (GetComponent<MeleeEnemy>() != null)
                GetComponent<MeleeEnemy>().enabled = false; //stop enemy attacking
            if (GetComponent<Horizontal>() != null)
                GetComponent<Horizontal>().enabled = false; //stop enemy moving
            SoundManager.instance.PlaySound(deadSound, deadVolume);
            deadSound = null;
        }
    }

    private void Spawn() {
        transform.position  = new Vector3(
                startingPlatform.transform.position.x,
                startingPlatform.transform.position.y + 1,
                startingPlatform.transform.position.z);
    }

    public void AddHealth(float _value) {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
    
    private IEnumerator Invunerability() {
        Physics2D.IgnoreLayerCollision(9, 10, true); //ignore collisions for layers 9(Player) & 10(Enemy)
        //invulnerability duration
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(0,0,0, 0.5f); //code for Red:1,0,0 -> make player red
            yield return new WaitForSeconds(invulnerabilityDuration / (numberOfFlashes * 2)); // make code wait for 1 second
            spriteRend.color = Color.white; // return player to original color
            yield return new WaitForSeconds(invulnerabilityDuration / (numberOfFlashes * 2)); // wait another 1 second

        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

    public float GetStartingHealth() {
        return startingHealth;
    }

}
