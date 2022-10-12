using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;

    public HealthBarScript healthBar;

    // Start is called before the first frame update
    void Start()
    {
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

    public bool TakeDamage(int damage) {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        return (currentHealth > 0);
    }

    public void DisableHealthBar() {
        healthBar.gameObject.SetActive(false);
    }
}
