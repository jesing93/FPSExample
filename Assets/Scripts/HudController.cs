using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Text scoreText;
    [SerializeField] private Image damageEffect;
    [SerializeField] private float flashTime;

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
}
