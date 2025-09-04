using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; set; }

    public List<GameObject> itemSlots;
    public GameObject activeItemSlot;

    private GameObject flashlightSlot = null;

    [Header("Keycards & Keys")]
    public List<int> collectedAccessCardLevels = new List<int>();
    public List<string> collectedKeyIds = new List<string>();

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
        activeItemSlot = itemSlots[0];
    }

    private void Update()
    {
        foreach (GameObject itemSlot in itemSlots)
        {
            if (itemSlot == activeItemSlot || itemSlot == flashlightSlot)
            {
                itemSlot.SetActive(true);
            }
            else
            {
                itemSlot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchActiveSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchActiveSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchActiveSlot(2);
    }

    public void PickupItem(GameObject pickedupItem)
    {
        int? emptySlotIndex = FindFirstEmptySlot();
        if (emptySlotIndex == null)
        {
            Debug.Log("Slots de item estão cheios!");
            return;
        }
        Transform targetSlot = itemSlots[emptySlotIndex.Value].transform;
        pickedupItem.transform.SetParent(targetSlot, true);

        Item item = pickedupItem.GetComponent<Item>();
        pickedupItem.transform.localPosition = item.spawnPosition;
        pickedupItem.transform.localRotation = Quaternion.Euler(item.spawnRotation);
        pickedupItem.transform.localScale = item.spawnScale;

        if (item.animator != null) item.animator.enabled = true;

        pickedupItem.GetComponent<Collider>().enabled = false;

        if (item.thisItemType == Item.ItemType.Flashlight)
        {
            flashlightSlot = itemSlots[emptySlotIndex.Value];
            item.isActiveItem = true;
        }
        else
        {
            SwitchActiveSlot(emptySlotIndex.Value);
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeItemSlot != null && activeItemSlot.transform.childCount > 0 && activeItemSlot != flashlightSlot)
        {
            Item currentItem = activeItemSlot.transform.GetChild(0).GetComponent<Item>();
            currentItem.isActiveItem = false;
        }

        activeItemSlot = itemSlots[slotNumber];

        if (activeItemSlot != null && activeItemSlot.transform.childCount > 0 && activeItemSlot != flashlightSlot)
        {
            Item newItem = activeItemSlot.transform.GetChild(0).GetComponent<Item>();
            newItem.isActiveItem = true;
        }
    }

    public void PickupKey(string keyId)
    {
        if (!collectedKeyIds.Contains(keyId))
        {
            collectedKeyIds.Add(keyId);
            Debug.Log($"Pegou a chave: {keyId}");
        }
    }

    public bool HasKey(string keyId)
    {
        return collectedKeyIds.Contains(keyId);
    }

    private int? FindFirstEmptySlot()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].transform.childCount == 0)
            {
                return i;
            }
        }
        return null;
    }

    public void PickupAccessCard(AccessCard card)
    {
        if (!collectedAccessCardLevels.Contains(card.accessLevel))
        {
            collectedAccessCardLevels.Add(card.accessLevel);
            collectedAccessCardLevels.Sort();
        }
        Debug.Log($"Pegou Cartão de Acesso Nível {card.accessLevel}");
    }

    public bool HasAccessLevel(int requiredLevel)
    {
        if (requiredLevel <= 0)
        {
            return true;
        }

        return collectedAccessCardLevels.Any(playerCardLevel => playerCardLevel >= requiredLevel);
    }
}