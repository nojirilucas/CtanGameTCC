using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice; // LÓGICA RESTAURADA

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public float maxInteractionDistance = 7f;
    public Player player;

    public Item hoveredItem = null;
    public AccessCard hoveredAccessCard = null;
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
        else if ((hoveredAccessCard = obj.GetComponent<AccessCard>()) != null)
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
        else if (obj.CompareTag("CameraAccessItem"))
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

            GameObject interactedObject = lastHoveredObject;

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
            else if (hoveredAccessCard != null)
            {
                InventoryManager.Instance.PickupAccessCard(hoveredAccessCard);
                Destroy(hoveredAccessCard.gameObject);
                ClearAllHovers();
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
            // LÓGICA RESTAURADA
            else if (interactedObject != null && interactedObject.CompareTag("CameraAccessItem"))
            {
                // Este script CameraSystemManager não foi fornecido, mas a lógica para chamá-lo está aqui.
                // Se você tiver o script, pode descomentar.
                // if (CameraSystemManager.Instance != null)
                // {
                //     CameraSystemManager.Instance.GrantCameraAccess();
                // }
                Destroy(interactedObject);
                ClearAllHovers();
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
        hoveredAccessCard = null;
        hoveredCloset = null;
        hoveredKey = null;
        hoveredDrawer = null;
        hoveredFluxogramaTrigger = null;
        lastHoveredObject = null;
    }

    // LÓGICA RESTAURADA
    private void ToggleOutline(GameObject obj, bool enabled)
    {
        // Este script 'cakeslice.Outline' não foi fornecido, mas a lógica para chamá-lo está aqui.
        // Se você tiver o script, pode descomentar.
        Outline[] outlinesInChildren = obj.GetComponentsInChildren<Outline>();
        foreach (Outline outline in outlinesInChildren)
        {
            outline.enabled = enabled;
        }
    }
}