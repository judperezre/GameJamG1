using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // 1. Variables principales
    [SerializeField] private int daño = 1;
    [SerializeField] private bool dañoContinuo = false;
    [SerializeField] private float tiempoEntreDaños = 1.0f;
    [SerializeField] private AudioSource audioSource; // Para reproducir sonido de daño si es necesario
    [SerializeField] private AudioClip audioDaño;

    private float timer = 0f;
    private bool jugadorDentro = false;

    private void Start ()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // 2. Trigger enter
    private void OnTriggerEnter2D ( Collider2D other )
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.RestarVida(daño);
            jugadorDentro = true;
            timer = tiempoEntreDaños; // resetea el timer para daño continuo
            Destroy(gameObject);
        }
    }

    // 3. Trigger exit
    private void OnTriggerExit2D ( Collider2D other )
    {
        if (other.CompareTag("Player"))
        {
            jugadorDentro = false;
        }
    }

    // 4. Update (solo si es daño continuo)
    private void Update ()
    {
        if (dañoContinuo && jugadorDentro)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                GameManager.Instance.RestarVida(daño);
                timer = tiempoEntreDaños; // reinicia el cooldown
            }
        }
    }

}
