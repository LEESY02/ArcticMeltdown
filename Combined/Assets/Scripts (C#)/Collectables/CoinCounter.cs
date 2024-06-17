using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    private Player player;
    private Text counter;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        counter = GetComponent<Text>();
    }

    private void Update()
    {
        counter.text = "X " + player.coinCount.ToString();
    }
}
