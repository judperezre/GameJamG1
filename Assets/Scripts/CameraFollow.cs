using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("A quién seguir")]
    public Transform player;

    [Header("Configuración de cámara")]
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;

    private void Start ()
    {
        BuscarJugadorSiNoHay();
    }

    void LateUpdate ()
    {
        if (player == null)
        {
            BuscarJugadorSiNoHay();
            return;
        }

        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, 4, transform.position.z);
    }

    void BuscarJugadorSiNoHay ()
    {
        if (PlayerController.Instance != null)
        {
            player = PlayerController.Instance.transform;
        }
    }
}
