using System.Collections;
using System.Collections.Generic;
using BrackeysGJ.MonoBehaviours;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private HealthBar healthBar;

    // Initial max health (in case a health powerup runs out)
    private float initMaxHealth = 100f; 
    [SerializeField] private float currentHealth = 100f;

    /// <summary>
    /// Is true if the player's health is 0 or bellow.
    /// </summary>
    [SerializeField] private bool isDead = false;

    private PlayerController _controller;

    private bool hasHealthPowerup = false;

    /// <summary>
    /// How many seconds the max health powerup has left.
    /// </summary>
    private float healthPowerupTimer;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<PlayerController>();
        initMaxHealth = maxHealth;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetHealth(currentHealth);
        // If for some reason the player has no health but isn't dead yet
        if (currentHealth <= 0)
        {
            Die();
        }

        if (!isDead)
        {
            if (hasHealthPowerup)
            {
                healthPowerupTimer -= Time.deltaTime;

                if (healthPowerupTimer <= 0)
                {
                    // Reset the player's max health
                    maxHealth = initMaxHealth;

                    if(currentHealth > maxHealth)
                    {
                        currentHealth = maxHealth;
                    }

                    // Should we heal the player back to maxHealth if their currentHealth is 
                    // bellow max health when the powerup ends? I don't think we should.
                }
            }
        }
    }

    /// <summary>
    /// Deal damage to the player
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            isDead = true;
            _controller.Die();
            Die();
        }
    }

    public void GiveHealth(float healthToGive)
    {
        currentHealth += healthToGive;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void UseHealthPowerup(float newMaxHealth, float secondsUntilPowerupRunsOut)
    {
        if (hasHealthPowerup != true)
        {
            maxHealth = newMaxHealth;
            healthPowerupTimer -= secondsUntilPowerupRunsOut;
            hasHealthPowerup = true;
        }
    }

    public void ResetMaxHealth()
    {
        maxHealth = initMaxHealth;
    }

    /// <summary>
    /// Checks if the player is dead
    /// </summary>
    /// <returns>True = Player dead,  False = Player alive</returns>
    public bool CheckIfDead()
    {
        return isDead;
    }

    /// <summary>
    /// Kill the player
    /// </summary>
    private void Die()
    {
        isDead = true;
    }

    public float health()
    {
        return currentHealth;
    }
}
