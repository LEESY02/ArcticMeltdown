using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("PowerUps")]
    [SerializeField] private GameObject[] powerUps;
    [SerializeField] private GameObject purchaseText;

    [Header("Visual Effects")]
    [SerializeField] private float scaleMagnitude;
    [SerializeField] private float fadeDuration;

    private Player player;
    private PlayerAttack playerAttack;
    private Tracker tracker;
    private int currentPos;
    private Vector3 originalScale;
    private Vector3 selectedScale;
    private Text purchaseTextComponent;

    // Power Ups
    private const int HEALTH = 0;
    private const int MOVEMENTSPEED = 1;
    private const int JUMPCOUNT = 2;
    private const int JUMPfORCE = 3;
    private const int ATTACKCOOLDOWN = 4;

    private const int PRICE = 3;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        playerAttack = player.GetComponent<PlayerAttack>();
        tracker = FindFirstObjectByType<Tracker>().GetComponent<Tracker>();
        currentPos = 0;
        originalScale = powerUps[0].GetComponent<RectTransform>().localScale;
        selectedScale = new Vector3(originalScale.x * scaleMagnitude, originalScale.y * scaleMagnitude, originalScale.z);
        purchaseTextComponent = purchaseText.GetComponent<Text>();
    }

    private void Update()
    {
        ScaleChanger();

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) //move pointer right
        {
            ChangePosition(1);
            Debug.Log(currentPos);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) //move pointer left
        {
            ChangePosition(-1);
            Debug.Log(currentPos);
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) // when the user confirms purchase
        {
            if (player.coinCount >= PRICE)
            {
                ApplyPowerUp();
                player.coinCount -= PRICE; // Reduce price from Player's coin count
            }
            else
            {
                NotEnoughCoins();
            }
        }
    }

    private void ChangePosition(int pos)
    {
        if (currentPos == 0 && pos < 0) // make current position the index of the last power-up
        {
            currentPos = powerUps.Length - 1;
        } 
        else if (currentPos == powerUps.Length - 1 && pos > 0) // make current position the index of the first power-up
        {
            currentPos = 0;
        }
        else
        {
            currentPos += pos;
        }
    }

    private void ScaleChanger()
    {
        for(int i = 0; i < powerUps.Length; i++)
        {
            if (i == currentPos)
                powerUps[i].GetComponent<RectTransform>().localScale = selectedScale;
            else
                powerUps[i].GetComponent<RectTransform>().localScale = originalScale;
        }
    }

    private void ApplyPowerUp()
    {
        switch (currentPos)
        {
            case HEALTH:
                IncreaseMaxHealth();
                break;
            case MOVEMENTSPEED:
                IncreaseMoveSpeed();
                break;
            case JUMPCOUNT:
                IncreaseJumpCount();
                break;
            case JUMPfORCE:
                IncreaseJumpForce();
                break;
            case ATTACKCOOLDOWN:
                ReduceAtkInterval();
                break;
        }
        purchaseTextComponent.color = Color.green;
        purchaseTextComponent.text = "-3";
        StartCoroutine(FadeOutText());
    }

    #region PowerUps
    private void IncreaseJumpCount()
    {
        tracker.extraJumps += 1;
        player.RefreshExtraJumps();
    }
    
    private void IncreaseJumpForce()
    {
        tracker.jumpForce += 4;
        player.RefreshJumpForce();
    }

    private void IncreaseMoveSpeed()
    {
        tracker.speed *= 2;
        player.RefreshPlayerSpeed();
    }

    private void IncreaseMaxHealth()
    {
        tracker.playerStartingHealth += 1;
    }
    
    private void ReduceAtkInterval()
    {
        tracker.attackCooldown /= 2;
        playerAttack.RefreshAttackCooldown();
    }
    #endregion

    private void NotEnoughCoins()
    {
        purchaseTextComponent.color = Color.red;
        purchaseTextComponent.text = "Not Enough Coins!";
        StartCoroutine(FadeOutText());
    }

    private IEnumerator FadeOutText()
    {
        float startAlpha = purchaseTextComponent.color.a;
        float rate = 1.0f / fadeDuration;
        float progress = 0.0f;

        while (progress < 1.0f)
        {
            Color tmpColor = purchaseTextComponent.color;
            purchaseTextComponent.color = new Color(tmpColor.r, tmpColor.g, tmpColor.b, Mathf.Lerp(startAlpha, 0, progress));
            progress += rate * Time.unscaledDeltaTime; // Use unscaled time to ignore time scale
            yield return null;
        }
        
        purchaseTextComponent.color = new Color(purchaseTextComponent.color.r, purchaseTextComponent.color.g, purchaseTextComponent.color.b, 0);
    }
}
