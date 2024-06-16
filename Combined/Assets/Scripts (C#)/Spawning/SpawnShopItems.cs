using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShopItems : MonoBehaviour
{
    private Player player;
    private PlayerAttack playerAttack;
    private Tracker tracker;
    private GameObject[] snowballs;
 
    private int rand;
    private GameObject powerUp;
    private bool receivedPowerUp = false;

    // Price
    public int price;

    // Array to store sprites, index 0 set to nothing
    public GameObject[] powerUpSprites;
    // Array to store power up names, to pop up when powerup is purchased
    public GameObject[] powerUpNames;
    // Gameobject to tell players they do not have enough coins
    public GameObject notEnoughCoins;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        playerAttack = player.GetComponent<PlayerAttack>();
        tracker = FindFirstObjectByType<Tracker>().GetComponent<Tracker>();
        snowballs = playerAttack.snowballs;
    }

    private void Update()
    {
        // Debug.Log(snowballs[0] == null);
    }

    void increaseJumpNo_0 ()
    {
        Debug.Log("inside 0");
        tracker.extraJumps += 1;
        player.RefreshExtraJumps();
    }
    void increaseJumpForce_1 ()
    {
        Debug.Log("inside 1");
        tracker.jumpForce += 4;
        player.RefreshJumpForce();
    }
    void increaseProjectileSpeed_2 ()
    {
        Debug.Log("inside 2");
        tracker.snowballSpeed += 2;
        for(int i = 0; i < snowballs.Length; i++)
        {
            snowballs[i].GetComponent<Snowball>().RefreshSnowballSpeed();
        }
    }
    void increaseMoveSpeed_3 ()
    {
        Debug.Log("inside 3");
        tracker.speed *= 2;
        player.RefreshPlayerSpeed();
    }
    void increaseMaxHealth_4 ()
    {
        Debug.Log("inside 4");
        tracker.playerStartingHealth += 1;
    } 
    void reduceAtkInterval_5 ()
    {
        Debug.Log("inside 5");
        tracker.attackCooldown /= 2;
        playerAttack.RefreshAttackCooldown();
    }

    /* Not used at the moment
    void increaseMeleeDmg_3 ()
    {

    }
    void increaseRangeDmg_4 ()
    {

    }
    */

    void applyPowerUp ()
    {
        Debug.Log("inside applyPowerUp");
        switch (rand)
        {
            case 0:
                increaseJumpNo_0();
                break;
            case 1:
                increaseJumpForce_1();
                break;
            case 2:
                increaseMoveSpeed_3();
                break;
            case 3:
                reduceAtkInterval_5();
                break;
            case 4:
                increaseMaxHealth_4();
                break; 
            case 5:
                increaseProjectileSpeed_2();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("inside Collider");
        if (collision.CompareTag("Player") && player.coinCount >= price)
        {
            Debug.Log("Compare Match");
            // Give powerup
            applyPowerUp();
            receivedPowerUp = true;
            GameObject copy1 = Instantiate(powerUpNames[rand], transform.position, Quaternion.identity);
            Destroy(copy1, 3f);

            // Reduce coin count
            player.coinCount -= price;

            // Remove sprite
            Destroy(powerUp); //???? idk
            Destroy(this.gameObject);
        } else if (!receivedPowerUp)
        {
            Debug.Log("Compare Fail");
            // Not enough coins
            GameObject copy2 = Instantiate(notEnoughCoins, transform.position, Quaternion.identity);
            Destroy(copy2, 3f);
        }
    }



    void Start()
    {
        rand = Random.Range(0, powerUpSprites.Length);
        powerUp = Instantiate(powerUpSprites[rand], transform.position, Quaternion.identity);
/*
        string[] powerUpNames = 
        {
            "Increased Jump Count!",
            "Increased Jump Force!",
            "Increased Projectile Speed!",
            "Increased Movement Speed!",
            "Increased Max Health!",
            "Reduced Attack Interval!"
        };
*/
    }
}
