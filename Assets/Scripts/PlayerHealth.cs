using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de vida")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    public Image healthBar; // Asigna aquí una imagen tipo "Fill"

    [Header("Efectos y sonidos")]
    public AudioClip hurtSound;
    public AudioClip deathSound;
    public AudioSource audioSource;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (hurtSound && audioSource)
            audioSource.PlayOneShot(hurtSound);

        if (currentHealth <= 0)
            Die();
    }

    void UpdateHealthUI()
    {
        if (healthBar)
            healthBar.fillAmount = currentHealth / maxHealth;
    }

    void Die()
    {
        isDead = true;
        if (deathSound && audioSource)
            audioSource.PlayOneShot(deathSound);

        Debug.Log("💀 Jugador muerto");
        // Puedes añadir reinicio de escena o animación
    }
}
