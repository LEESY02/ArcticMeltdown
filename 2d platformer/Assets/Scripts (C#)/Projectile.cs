using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject snowballEffect;
    private bool hit;
    private float direction;
    private float lifetime;

    private CircleCollider2D circleCollider;
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update() {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 4) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        hit = true;
        circleCollider.enabled = false;
        GameObject effect = Instantiate(snowballEffect, transform.position, transform.rotation);
        Destroy(effect, 1.5f);
        Deactivate();
    }

    public void SetDirection(float _direction) {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        circleCollider.enabled = true;

        // flip the direction of the snowball respectiely
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction) {
            localScaleX = -localScaleX;
        }
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }
}
