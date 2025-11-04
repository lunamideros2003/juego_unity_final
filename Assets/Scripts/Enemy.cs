using UnityEngine;
using UnityEngine.UI; // Para la barra de vida (UI)

public class Enemy : MonoBehaviour
{
    [Header("Vida del enemigo")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    public Image healthBarFill; // Imagen que representa la barra (tipo Fill)

    [Header("Efectos")]
    public GameObject deathEffect; // Efecto al morir (opcional)
    public AudioClip deathSound;
    public AudioSource audioSource;

    private Animator animator; // Si el enemigo tiene animaciones

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        UpdateHealthUI();
    }

    // 🔥 Este método se llama cuando el enemigo recibe daño
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        // Reproducir animación de muerte (si tiene)
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Efecto de muerte
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Sonido de muerte
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Destruir al enemigo después de un corto tiempo
        Destroy(gameObject, 1.5f);
    }
}
