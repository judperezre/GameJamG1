using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;  // puedes ajustar la velocidad
    private GameObject player;

    void Start ()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update ()
    {
        Mover();
    }

    private void Mover ()
    {
        if (player != null)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
        }
    }
}
