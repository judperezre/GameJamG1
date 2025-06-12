using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void LateUpdate ()
    {
        if (player == null) return;

        // Seguimiento directo al jugador
        Vector3 targetPosition = player.position + offset;

        // Redondeamos las posiciones a píxeles enteros para evitar el jitter
        float pixelsPerUnit = 18f; // El mismo que tienes configurado en Pixel Perfect Camera

        targetPosition.x = Mathf.Round(targetPosition.x * pixelsPerUnit) / pixelsPerUnit;
        targetPosition.y = Mathf.Round(targetPosition.y * pixelsPerUnit) / pixelsPerUnit;

        // Como no quieres que la cámara se mueva en Y, solo aplicamos en X
        transform.position = new Vector3(targetPosition.x, offset.y, offset.z);
    }
}
