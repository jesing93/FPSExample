using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI currentAmmo;
    [SerializeField] private TextMeshProUGUI ammoStorage;
    [SerializeField] private Image damageEffect;
    [SerializeField] private float flashTime;
    [SerializeField] private GameObject endPanel;

    private Coroutine dissapearCoroutine;

    public static HudController instance;


    private void Awake()
    {
        instance = this;
    }

    public void UpdateHealth(float currentHealth)
    {
        healthBar.value = currentHealth;
        if (currentHealth == 0)
        {
            Color color = new Color(192f, 0f, 0f, 0f);
            healthBar.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = color;
        }
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString("00000");
    }

    public void UpdateCurrentAmmo(string ammo)
    {
        currentAmmo.text = ammo;
    }

    public void UpdateAmmoStorage(string ammo)
    {
        ammoStorage.text = ammo;
    }

    /// <summary>
    /// Turn on the damage flash on damaged
    /// </summary>
    public void ShowDamageFlash()
    {
        //If there is the coroutine stop it
        if(dissapearCoroutine != null)
        {
            StopCoroutine(dissapearCoroutine);
        }

        //Start coroutine
        dissapearCoroutine = StartCoroutine(DamageDissappear());
    }

    private IEnumerator DamageDissappear()
    {
        //restart the image color
        damageEffect.color = Color.white;

        //Alpha color reset to 1
        float alpha = 1.0f;

        while(alpha > 0.0f)
        {
            alpha -= (1.0f / flashTime) * Time.deltaTime;
            damageEffect.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            yield return null;
        }
    }

    public void OpenEndPanel()
    {
        endPanel.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
