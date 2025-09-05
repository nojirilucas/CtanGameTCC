using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }
    public InventoryManager inventoryManager;

    public float maxInteractionDistance = 7f;
    public Player player;

    public Item hoveredItem = null;
    public ClosetController hoveredCloset = null;
    public Key hoveredKey = null;
    public DrawerController hoveredDrawer = null;
    public FluxogramaTrigger hoveredFluxogramaTrigger = null;

    private GameObject lastHoveredObject = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        Outline[] allOutlinesInScene = FindObjectsOfType<Outline>(true);
        foreach (Outline outline in allOutlinesInScene)
        {
            if (outline.enabled)
            {
                outline.enabled = false;
            }
        }
    }

    private void Update()
    {
        if (Camera.main == null || player == null) return;
        HandleInteractionInput();

        if (player.isHiding)
        {
            if (lastHoveredObject != null) ClearAllHovers();
            return;
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxInteractionDistance))
        {
            GameObject currentHitObject = hit.transform.gameObject;

            if (currentHitObject != lastHoveredObject)
            {
                ClearAllHovers();
                ProcessNewHover(currentHitObject);
            }
        }
        else
        {
            if (lastHoveredObject != null)
            {
                ClearAllHovers();
            }
        }
    }

    private void ProcessNewHover(GameObject obj)
    {
        lastHoveredObject = obj;
        hoveredCloset = obj.GetComponentInParent<ClosetController>();

        if (hoveredCloset != null)
        {
            ToggleOutline(hoveredCloset.gameObject, true);
        }
        else if ((hoveredKey = obj.GetComponent<Key>()) != null)
        {
            ToggleOutline(obj, true);
        }
        else if ((hoveredDrawer = obj.GetComponent<DrawerController>()) != null)
        {
            ToggleOutline(obj, true);
        }
        else if ((hoveredItem = obj.GetComponent<Item>()) != null && !hoveredItem.isActiveItem)
        {
            ToggleOutline(obj, true);
        }
        else if ((hoveredFluxogramaTrigger = obj.GetComponent<FluxogramaTrigger>()) != null)
        {
            ToggleOutline(obj, true);
        }
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (player.isHiding)
            {
                player.Unhide();
                return;
            }

            if (hoveredCloset != null && hoveredCloset.IsInteractable)
            {
                player.Hide(hoveredCloset);
            }
            else if (hoveredKey != null)
            {
                InventoryManager.Instance.PickupKey(hoveredKey.keyId);
                Destroy(hoveredKey.gameObject);
                ClearAllHovers();
            }
            else if (hoveredDrawer != null)
            {
                hoveredDrawer.TryOpenDrawer();
            }
            else if (hoveredItem != null)
            {
                InventoryManager.Instance.PickupItem(hoveredItem.gameObject);
                ClearAllHovers();
            }
            else if (hoveredFluxogramaTrigger != null)
            {
                hoveredFluxogramaTrigger.AbrirFluxograma(player);
                ClearAllHovers();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Primeiro, verifica se o livro já está aberto
            if (BookViewController.Instance.isBookOpen)
            {
                // Se estiver aberto, simplesmente o fecha
                BookViewController.Instance.CloseBook();
            }
            else // Se não estiver aberto, executa a lógica para tentar abrir
            {
                // Pega o item que está no slot ativo do inventário
                if (inventoryManager.activeItemSlot != null && inventoryManager.activeItemSlot.transform.childCount > 0)
                {
                    GameObject activeItemObject = inventoryManager.activeItemSlot.transform.GetChild(0).gameObject;

                    // Verifica se este item tem os dados de um livro
                    BookData bookData = activeItemObject.GetComponent<BookData>();
                    if (bookData != null)
                    {
                        // Se for um livro, chama o controlador para abri-lo
                        BookViewController.Instance.OpenBook(bookData);
                    }
                }
            }
        }
    }

    private void ClearAllHovers()
    {
        if (lastHoveredObject != null)
        {
            var closet = lastHoveredObject.GetComponentInParent<ClosetController>();
            if (closet != null)
            {
                ToggleOutline(closet.gameObject, false);
            }
            else
            {
                ToggleOutline(lastHoveredObject, false);
            }
        }

        hoveredItem = null;
        hoveredCloset = null;
        hoveredKey = null;
        hoveredDrawer = null;
        hoveredFluxogramaTrigger = null;
        lastHoveredObject = null;
    }

    private void ToggleOutline(GameObject obj, bool enabled)
    {
        Outline[] outlinesInChildren = obj.GetComponentsInChildren<Outline>();
        foreach (Outline outline in outlinesInChildren)
        {
            outline.enabled = enabled;
        }
    }
}