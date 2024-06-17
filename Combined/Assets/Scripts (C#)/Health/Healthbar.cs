using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image totalHealthbar;
    [SerializeField] private Image currentHealthBar;
    private Health playerHealth;

    private void Start() {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        totalHealthbar.fillAmount = FindObjectOfType<Tracker>().playerStartingHealth / 10;
    }

    private void Update() {
        totalHealthbar.fillAmount = FindObjectOfType<Tracker>().playerStartingHealth / 10;
        currentHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }

}
