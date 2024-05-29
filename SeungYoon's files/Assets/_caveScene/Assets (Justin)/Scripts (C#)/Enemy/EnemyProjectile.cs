using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : EnemyDamage //will damage the player every time they touch
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    [SerializeField] private float direction;
    private float lifetime;
    private Animator anim;
    private bool moving;

    private void Awake() {
        anim = GetComponent<Animator>();
    }
    
    public void ActivateProjectile(Vector3 position) {
        transform.position = position;
        lifetime = 0;
        gameObject.SetActive(true);
        moving = true;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }

    private void Update() {
        if (!gameObject.activeInHierarchy) return;
        if (moving) {
            float movementSpeed = speed * Time.deltaTime * Mathf.Sign(direction);
            transform.Translate(movementSpeed, 0, 0);
        }
        lifetime += Time.deltaTime;
        if (lifetime > resetTime) Deactivate();
    }

    private new void OnTriggerEnter2D(Collider2D collision) {
        gameObject.GetComponent<Animator>().SetTrigger("explode");
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        base.OnTriggerEnter2D(collision); //execute logic from parent script first
        StartCoroutine(WaitAndDeactivate()); //deactive fireball upon collision
    }

    private IEnumerator WaitAndDeactivate() {
        moving = false;
        float duration = GetAnimationClipDuration("Explode");
        yield return new WaitForSeconds(duration + 0.02f);
        Deactivate();
    }

    private float GetAnimationClipDuration(string animationName) {
        RuntimeAnimatorController ac = anim.runtimeAnimatorController;
        foreach (AnimationClip clip in ac.animationClips) {
            if (clip.name == animationName) {
                return clip.length;
            }
        }
        return 0f; // Return 0 if the animation clip is not found
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }
}
