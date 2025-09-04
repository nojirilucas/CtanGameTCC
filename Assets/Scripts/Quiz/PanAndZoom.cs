using UnityEngine;
using UnityEngine.EventSystems;

public class PanAndZoom : MonoBehaviour
{
    [Header("Configura��es de Zoom")]
    [Tooltip("Velocidade do zoom com a roda do mouse.")]
    public float zoomSpeed = 0.5f;
    [Tooltip("Escala m�nima que o painel pode atingir.")]
    public float minZoom = 0.5f;
    [Tooltip("Escala m�xima que o painel pode atingir.")]
    public float maxZoom = 3f;

    private RectTransform rectTransform;
    private Vector2 initialMousePosition;
    private Vector2 initialPanelPosition;
    private bool isDragging = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // AQUI EST� A CORRE��O
    // Esta fun��o � chamada automaticamente quando o objeto � desativado.
    private void OnDisable()
    {
        // Reseta o estado de arraste para garantir que n�o fique "preso".
        isDragging = false;
    }

    void Update()
    {
        HandlePan();
        HandleZoom();
    }

    private void HandlePan()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
            {
                isDragging = true;
                initialMousePosition = Input.mousePosition;
                initialPanelPosition = rectTransform.anchoredPosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 mouseDelta = (Vector2)Input.mousePosition - initialMousePosition;
            rectTransform.anchoredPosition = initialPanelPosition + mouseDelta;
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            Vector3 newScale = rectTransform.localScale + Vector3.one * scroll * zoomSpeed;

            newScale.x = Mathf.Clamp(newScale.x, minZoom, maxZoom);
            newScale.y = Mathf.Clamp(newScale.y, minZoom, maxZoom);
            newScale.z = 1f;

            rectTransform.localScale = newScale;
        }
    }
}