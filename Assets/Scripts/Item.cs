using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    public bool isActiveItem;

    [Header("Configurações do Item")]
    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    public Vector3 spawnScale;
    internal Animator animator;

    [Header("Tipo de Item")]
    public ItemType thisItemType;
    public enum ItemType
    {
        Generic,
        Flashlight
    }

    [Header("Ação do Item (Clique Direito)")]
    [Tooltip("Define o que acontece quando o jogador clica com o botão direito.")]
    public UnityEvent onRightClickAction;

    [Header("Componentes Específicos (Opcional)")]
    public GameObject lightSource;
    private bool isLightOn = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (thisItemType == ItemType.Flashlight && lightSource != null)
        {
            lightSource.SetActive(isLightOn);
        }
    }

    void Update()
    {
        if (!isActiveItem)
        {
            foreach (Transform t in GetComponentsInChildren<Transform>(true))
            {
                t.gameObject.layer = LayerMask.NameToLayer("Default");
            }
            return;
        }

        foreach (Transform t in GetComponentsInChildren<Transform>(true))
        {
            // AQUI ESTÁ A MUDANÇA
            t.gameObject.layer = LayerMask.NameToLayer("ItemRenderer");
        }

        if (thisItemType == ItemType.Flashlight)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                ToggleFlashlight();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            onRightClickAction.Invoke();
        }
    }

    public void ToggleFlashlight()
    {
        if (lightSource == null) return;
        isLightOn = !isLightOn;
        lightSource.SetActive(isLightOn);
    }
}