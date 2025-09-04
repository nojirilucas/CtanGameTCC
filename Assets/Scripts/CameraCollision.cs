using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    public Transform target;           // Ponto de origem (ex: cabe�a do player)
    public Transform cameraToMove;     // A c�mera ou holder que ser� movido
    public float minDistance = 0.2f;   // Dist�ncia m�nima permitida
    public float maxDistance = 2f;     // Dist�ncia m�xima (posi��o normal)
    public float smooth = 10f;         // Suavidade do movimento
    public LayerMask collisionMask;    // M�scara de colis�o

    private float currentDistance;
    private Vector3 desiredPosition;

    void LateUpdate()
    {
        Vector3 direction = (cameraToMove.position - target.position).normalized;

        // Detecta colis�o
        if (Physics.SphereCast(target.position, 0.2f, direction, out RaycastHit hit, maxDistance, collisionMask))
        {
            currentDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            currentDistance = maxDistance;
        }

        desiredPosition = target.position + direction * currentDistance;

        // Move a c�mera
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
