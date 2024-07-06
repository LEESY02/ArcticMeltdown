using System.Collections;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private GameObject shopScreen;
    private bool shopActive = false;
    [SerializeField] private float shopCooldown;

    private void Awake()
    {
        shopScreen = FindObjectOfType<UIManager>().gameObject.transform.Find("ShopScreen").gameObject;
        
    }

    // private void Update()
    // {
    //     Debug.Log(shopScreen == null); // true means cannot find
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !shopActive)
        {
            StartCoroutine(ActivateShopScreen());
        }
    }

    private IEnumerator ActivateShopScreen()
    {
        shopActive = true;
        Time.timeScale = 0; // Pause the game
        shopScreen.SetActive(true);

        yield return new WaitForSecondsRealtime(shopCooldown); // Wait for the cooldown period before allowing reactivation

        shopActive = false; // Shop can be activated again
    }
}
