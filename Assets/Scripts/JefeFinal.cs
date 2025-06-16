using System.Collections;
using TMPro;
using UnityEngine;

public class JefeFinal : EnemyBase
{
    [Header("UI Jefe")]
    [SerializeField] private TextMeshProUGUI vidaJefeText;
    [SerializeField] private GameObject panelJefe;

    [Header("Disparo")]
    [SerializeField] private GameObject projectilePrefab;
    private bool isDeath = false;

    protected override void Start ()
    {
        base.Start();
        transform.rotation = Quaternion.Euler(0, 180, 0);
        ActualizarUIJefe();
        StartCoroutine(FireProjectile());
    }

    protected override void EstadoEsperando ()
    {
        base.EstadoEsperando();
        if (estadoActual == EstadosMovimientos.Siguiendo)
        {
            panelJefe.SetActive(true);
        }
    }

    IEnumerator FireProjectile ()
    {
        while (!isDeath)
        {
            if (estadoActual == EstadosMovimientos.Siguiendo)
            {
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            }
            yield return new WaitForSeconds(2f);
        }
    }

    protected override void Morir ()
    {
        isDeath = true;
        Destroy(gameObject);
        GameManager.Instance.WinGame();
    }

    public override void RecibirDaño ( int dañoRecibido )
    {
        base.RecibirDaño(dañoRecibido);
        ActualizarUIJefe();
    }

    private void ActualizarUIJefe ()
    {
        vidaJefeText.text = "Vida: " + vida.ToString();
    }
}
