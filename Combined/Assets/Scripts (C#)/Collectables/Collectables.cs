using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField] private float value; //value the item adds to the player
    [SerializeField] private AudioClip collectionSound;
    public float collectionVolume;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Snowball") return; //ignore projectiles form Player
        if (collision.tag == "Player") 
        {
            if (gameObject.tag == "Health") collision.GetComponent<Health>().AddHealth(value);
            if (gameObject.tag == "Coin") collision.GetComponent<Player>().coinCount += 1;
            SoundManager.instance.PlaySound(collectionSound, collectionVolume);
            gameObject.SetActive(false); //deactivate the collectable
        }
    }
}
