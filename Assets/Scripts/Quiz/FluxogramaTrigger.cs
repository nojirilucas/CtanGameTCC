using UnityEngine;

public class FluxogramaTrigger : MonoBehaviour
{
    [Tooltip("Arraste o objeto Canvas do seu fluxograma para cá.")]
    [SerializeField] private GameObject canvasFluxograma;

    public void AbrirFluxograma(Player interactingPlayer)
    {
        if (canvasFluxograma == null)
        {
            Debug.LogError("O Canvas do Fluxograma não foi atribuído no Inspector!");
            return;
        }

        canvasFluxograma.SetActive(true);

        if (interactingPlayer != null)
        {
            interactingPlayer.FreezePlayer(true);
        }
    }
}