using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    public int killsRequired1;
    public int killsRequired2;
    public int killsRequired3;

    private int killsRequired;
    private Player player;
    private Text counter;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        counter = GetComponent<Text>();
        killsRequired = SceneManager.GetActiveScene().buildIndex == 2
                        ? killsRequired1
                        : SceneManager.GetActiveScene().buildIndex == 3
                            ? killsRequired2
                            : killsRequired3;
    }

    private void Update()
    {
        counter.text = $"X {player.killCount}/{killsRequired}";
    }
}
