using UnityEngine;
using System.Collections;

public class DrawerController : MonoBehaviour
{
    [Tooltip("ID da chave necessária para abrir esta gaveta. Use '0' para não exigir chave.")]
    public string requiredKeyId;

    [Header("Movimento da Gaveta")]
    public Vector3 OpenPos;
    public Vector3 ClosePos;
    [SerializeField] private float speed = 2f;
    private bool isOpen = false;

    private bool isMoving = false;

    [Header("Item Interno")]
    [Tooltip("Arraste para cá o COLISOR do item que está dentro da gaveta.")]
    public Collider itemColliderInside;

    public AudioClip lockedSound;
    public AudioClip openSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (itemColliderInside != null)
        {
            itemColliderInside.enabled = false;
        }
    }

    public void TryOpenDrawer()
    {
        if (isMoving)
        {
            return;
        }

        if (requiredKeyId == "0" || InventoryManager.Instance.HasKey(requiredKeyId))
        {
            if (audioSource != null && openSound != null)
            {
                audioSource.PlayOneShot(openSound);
            }
            isOpen = !isOpen;

            if (itemColliderInside != null)
            {
                itemColliderInside.enabled = isOpen;
            }

            StartCoroutine(MoveDrawer(isOpen ? OpenPos : ClosePos));
        }
        else
        {
            if (audioSource != null && lockedSound != null)
            {
                audioSource.PlayOneShot(lockedSound);
            }
            Debug.Log("Gaveta trancada. Requer a chave: " + requiredKeyId);
        }
    }

    private IEnumerator MoveDrawer(Vector3 targetPosition)
    {
        isMoving = true;

        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        transform.localPosition = targetPosition;

        isMoving = false;
    }
}