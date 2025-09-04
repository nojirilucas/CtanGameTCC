using UnityEngine;

public class FluxogramaTrigger : MonoBehaviour
{
    [Tooltip("Arraste o objeto Canvas do seu fluxograma para c�.")]
    [SerializeField] private GameObject canvasFluxograma;

    public void AbrirFluxograma(Player interactingPlayer)
    {
        if (canvasFluxograma == null)
        {
            Debug.LogError("O Canvas do Fluxograma n�o foi atribu�do no Inspector!");
            return;
        }

        canvasFluxograma.SetActive(true);

        if (interactingPlayer != null)
        {
            interactingPlayer.FreezePlayer(true);
        }
    }
}