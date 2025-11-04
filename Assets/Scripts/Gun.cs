using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Referencias")]
    public Transform playerCamera;        // Cámara del jugador
    public ParticleSystem shootParticles; // Partículas de disparo
    public GameObject hitEffect;          // Efecto al impactar algo
    public GameObject destroyEffect;      // Efecto al destruir un objeto

    [Header("Configuración del disparo")]
    public float shotDistance = 10f;      // Alcance del disparo
    public float impactForce = 5f;        // Fuerza aplicada al objeto golpeado
    public LayerMask shotMask;            // Capas que el raycast puede golpear
    public float damage = 25f;            // Daño que hace el arma

    [Header("Audio")]
    public AudioClip fireSound;           // Sonido al disparar
    public AudioClip hitSound;            // Sonido al impactar
    public AudioSource audioSource;       // Fuente de audio

    private RaycastHit showRaycastHit;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // 🎇 Reproducir partículas de disparo
        if (shootParticles != null)
            shootParticles.Play();

        // 🔊 Reproducir sonido de disparo
        if (fireSound != null && audioSource != null)
            audioSource.PlayOneShot(fireSound);

        // 🔦 Disparar raycast desde la cámara
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out showRaycastHit, shotDistance, shotMask))
        {
            Debug.Log("Shot hit: " + showRaycastHit.collider.name);

            // 💥 Efecto visual de impacto
            if (hitEffect != null)
                Instantiate(hitEffect, showRaycastHit.point, Quaternion.LookRotation(showRaycastHit.normal));

            // 💢 Aplicar fuerza física al objeto golpeado
            Rigidbody rb = showRaycastHit.collider.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(-showRaycastHit.normal * impactForce, ForceMode.Impulse);

            // 🔥 Hacer daño al enemigo si tiene componente Enemy
            Enemy enemy = showRaycastHit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // 🔊 Sonido de impacto (solo si golpea algo)
            if (hitSound != null && audioSource != null)
                audioSource.PlayOneShot(hitSound);

            // 💣 Si el objeto tiene tag "Barrel", destruirlo
            if (showRaycastHit.collider.CompareTag("Barrel"))
            {
                if (destroyEffect != null)
                    Instantiate(destroyEffect, showRaycastHit.point, Quaternion.LookRotation(showRaycastHit.normal));

                Destroy(showRaycastHit.collider.gameObject);
            }
        }
    }
}
