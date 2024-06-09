using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private Player player;
    private Text counter;

    private void Awake()
    {
        counter = GetComponent<Text>();
    }

    private void Update()
    {
        counter.text = "X " + player.coinCount.ToString();
    }
}
