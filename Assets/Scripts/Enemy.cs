using System.Collections;
using UnityEngine;

public class Enemy : EnemyBase
{
    [Header("Disparo")]
    [SerializeField] private GameObject projectilePrefab;
    private bool isDeath = false;

    protected override void Start ()
    {
        base.Start();
        StartCoroutine(FireProjectile());
    }

    IEnumerator FireProjectile ()
    {
        while (!isDeath)
        {
            if (estadoActual == EstadosMovimientos.Siguiendo)
            {
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(3);
        }
    }

    protected override void Morir ()
    {
        isDeath = true;
        Destroy(gameObject);
    }
}
