using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    [SerializeField] private float value; //value the item adds to the player
    [SerializeField] private AudioClip collectionSound;
    [SerializeField] private float collectionVolume;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Snowball") return; //ignore projectiles form Player
        if (collision.tag == "Player") {
            if (gameObject.tag == "Health") collision.GetComponent<Health>().AddHealth(value);
            //if (tag == "Coin") collision.GetComponent<>()
            SoundManager.instance.PlaySound(collectionSound, collectionVolume);
            gameObject.SetActive(false); //deactivate the collectable
        }
    }
}
