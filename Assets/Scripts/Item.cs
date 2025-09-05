using UnityEngine;

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
        Generic
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
            t.gameObject.layer = LayerMask.NameToLayer("ItemRenderer");
        }
    }
}