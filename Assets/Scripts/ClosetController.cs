using System.Collections;
using UnityEngine;

public class ClosetController : MonoBehaviour
{
    public Animator closetAnimator;
    public Transform hidingSpot;
    public float interactionCooldown = 1.0f;

    private bool isOpen = false;
    private BoxCollider boxCollider;
    public bool IsInteractable { get; private set; } = true;

    private void Awake()
    {
        closetAnimator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;
        closetAnimator.SetTrigger("Open");
    }

    public void CloseDoor()
    {
        if (!isOpen) return;

        isOpen = false;
        closetAnimator.SetTrigger("Close");
    }

    public void DisableCollider()
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
    }

    public void EnableCollider()
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }
    }

    public void TriggerCooldown()
    {
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        IsInteractable = false;
        yield return new WaitForSeconds(interactionCooldown);
        IsInteractable = true;
    }
}