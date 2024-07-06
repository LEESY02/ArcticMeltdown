using UnityEngine;

public class Snowball : MonoBehaviour
{
    [SerializeField] private GameObject snowballEffect;
    private bool hit;
    private float direction;
    private float lifetime;
    private Tracker tracker;
    private float speed; // Edited by SY

    private CircleCollider2D circleCollider;
    private Animator anim;

    private void Awake() {
        tracker = FindFirstObjectByType<Tracker>();
        anim = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
        speed = tracker.snowballSpeed;
    }

    // private void OnEnable()
    // {
    //     Debug.Log(speed);
    // }

    private void Update() {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 4) Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Coin") || collision.CompareTag("Health")) return;
        hit = true;
        circleCollider.enabled = false;
        GameObject effect = Instantiate(snowballEffect, transform.position, transform.rotation);
        Destroy(effect, 1.5f);
        Deactivate();

        if (collision.CompareTag("Enemy")) //damage enemies
            collision.GetComponent<Health>().TakeDamage(1);
    }

    public void SetDirection(float direction) {
        lifetime = 0;
        this.direction = direction;
        gameObject.SetActive(true);
        hit = false;
        circleCollider.enabled = true;

        // flip the direction of the snowball respectiely
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != direction) {
            localScaleX = -localScaleX;
        }
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }

    public void RefreshSnowballSpeed()
    {
        this.speed = tracker.snowballSpeed;
    }

    
}
