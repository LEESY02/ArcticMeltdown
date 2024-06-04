using UnityEngine.UI;
using UnityEngine;

public class VolumeText : MonoBehaviour
{
    [SerializeField] private string volumeName;
    [SerializeField] private string textIntro; //sound: or Music:
    private Text txt;

    private void Awake()
    {
        txt = GetComponent<Text>();
    }
    
    private void Update()
    {
        UpdateVolume();
    }

    private void UpdateVolume()
    {
        float volumeVlaue = PlayerPrefs.GetFloat(volumeName) * 100; //make the decimal into percentage values
        txt.text = textIntro + volumeVlaue.ToString();
    }
}
