using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // 1. Variables principales
    [SerializeField] private int da�o = 1;
    [SerializeField] private bool da�oContinuo = false;
    [SerializeField] private float tiempoEntreDa�os = 1.0f;
    [SerializeField] private AudioSource audioSource; // Para reproducir sonido de da�o si es necesario
    [SerializeField] private AudioClip audioDa�o;

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
            GameManager.Instance.RestarVida(da�o);
            jugadorDentro = true;
            timer = tiempoEntreDa�os; // resetea el timer para da�o continuo
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

    // 4. Update (solo si es da�o continuo)
    private void Update ()
    {
        if (da�oContinuo && jugadorDentro)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                GameManager.Instance.RestarVida(da�o);
                timer = tiempoEntreDa�os; // reinicia el cooldown
            }
        }
    }

}
