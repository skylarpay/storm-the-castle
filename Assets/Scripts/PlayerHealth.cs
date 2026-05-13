using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    public int currentHealth;

    [Header("UI")]
    public TextMeshProUGUI livesText;

    [Header("Damage")]
    public float damageCooldown = 1f;

    [Header("Death")]
    public float restartDelay = 1f;

    private float nextDamageTime;
    private bool isDead;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateLivesText();
    }

    public bool TakeDamage(int damage)
    {
        if (isDead || Time.time < nextDamageTime)
        {
            return false;
        }

        currentHealth -= damage;
        nextDamageTime = Time.time + damageCooldown;

        UpdateLivesText();

        if (currentHealth <= 0)
        {
            Die();
        }

        return true;
    }

    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentHealth;
        }
    }

    private void Die()
    {
        isDead = true;
        Invoke(nameof(RestartScene), restartDelay);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsDead()
    {
        return isDead;
    }
}