using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform target;           // Ponto de origem (ex: cabeça do player)
    public Transform cameraToMove;     // A câmera ou holder que será movido
    public float minDistance = 0.2f;   // Distância mínima permitida
    public float maxDistance = 2f;     // Distância máxima (posição normal)
    public float smooth = 10f;         // Suavidade do movimento
    public LayerMask collisionMask;    // Máscara de colisão

    private float currentDistance;
    private Vector3 desiredPosition;

    void LateUpdate()
    {
        Vector3 direction = (cameraToMove.position - target.position).normalized;

        // Detecta colisão
        if (Physics.SphereCast(target.position, 0.2f, direction, out RaycastHit hit, maxDistance, collisionMask))
        {
            currentDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            currentDistance = maxDistance;
        }

        desiredPosition = target.position + direction * currentDistance;

        // Move a câmera
        cameraToMove.position = Vector3.Lerp(cameraToMove.position, desiredPosition, smooth * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        if (target != null && cameraToMove != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(target.position, cameraToMove.position);
            Gizmos.DrawWireSphere(cameraToMove.position, 0.2f);
        }
    }
}
