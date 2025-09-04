using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Weapon;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;
    public GameObject activeWeaponSlot;

    private GameObject flashlightSlot = null;

    [Header("Keycards")]
    public List<int> collectedAccessCardLevels = new List<int>();

    [Header("Keys")]
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
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot || weaponSlot == flashlightSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchActiveSlot(2);
        }
    }

    public void PickupWeapon(GameObject pickedupWeapon)
    {
        int? emptySlotIndex = FindFirstEmptySlot();
        if (emptySlotIndex == null)
        {
            Debug.Log("Slots de arma estão cheios!");
            return;
        }
        Transform targetSlot = weaponSlots[emptySlotIndex.Value].transform;

        pickedupWeapon.transform.SetParent(targetSlot, true);

        Weapon weapon = pickedupWeapon.GetComponent<Weapon>();
        pickedupWeapon.transform.localPosition = weapon.spawnPosition;
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation);
        pickedupWeapon.transform.localScale = weapon.spawnScale;

        if (weapon.animator != null)
        {
            weapon.animator.enabled = true;
        }
        pickedupWeapon.GetComponent<Collider>().enabled = false;

        if (weapon.thisWeaponModel == WeaponModel.Flashlight)
        {
            flashlightSlot = weaponSlots[emptySlotIndex.Value];
            weapon.isActiveWeapon = true;
        }
        else
        {
            SwitchActiveSlot(emptySlotIndex.Value);
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
        for (int i = 0; i < weaponSlots.Count; i++)
        {
            if (weaponSlots[i].transform.childCount == 0)
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

    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot != null && activeWeaponSlot.transform.childCount > 0 && activeWeaponSlot != flashlightSlot)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot != null && activeWeaponSlot.transform.childCount > 0 && activeWeaponSlot != flashlightSlot)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }
}