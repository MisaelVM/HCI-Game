using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;

    public HealthBarScript healthBar;

    [SerializeField] private Image damageImage = null;
    [SerializeField] private float damageTimer = 0.1f;

    [SerializeField] private AudioClip damageSound = null;
    private AudioSource damageAudioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        damageAudioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TakeDamage(20);
        if (Input.GetKeyDown(KeyCode.F))
            TakeDamage(5);
    }

    IEnumerator HurtFlash() {
        //damageImage.enabled = true;
        damageAudioSource.PlayOneShot(damageSound);

        Color damageColor = damageImage.color;
        const float fadeDuration = 0.1f;
        float elapsedTime = 0.0f;
        // Fade in
        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            damageColor.a = t;
            damageImage.color = damageColor;
            yield return null;
        }
        damageColor.a = 1.0f;
        damageImage.color = damageColor;

        yield return new WaitForSeconds(damageTimer);

        // Fade out
        elapsedTime = 0.0f;
        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            damageColor.a = 1 - t;
            damageImage.color = damageColor;
            yield return null;
        }
        damageColor.a = 0.0f;
        damageImage.color = damageColor;

        //damageImage.enabled = false;
    }

    void TakeDamage(int damage) {
        StartCoroutine(HurtFlash());
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0) {
            StopAllCoroutines();
            currentHealth = 0;
            healthBar.SetHealth(0);
            Color damageColor = damageImage.color;
            damageColor.a = 1.0f;
            damageImage.color = damageColor;
        }
    }
}
