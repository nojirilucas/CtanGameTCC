using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isHiding { get; private set; } = false;

    private Vector3 positionBeforeHiding;
    private Quaternion rotationBeforeHiding;
    private ClosetController currentCloset;
    private bool isInteractingWithCloset = false;

    private PlayerMovement playerMovement;
    private CharacterController characterController;
    private MouseMovement mouseMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        characterController = GetComponent<CharacterController>();
        mouseMovement = GetComponent<MouseMovement>();
    }

    public void Hide(ClosetController closet)
    {
        if (isHiding || isInteractingWithCloset) return;

        currentCloset = closet;
        currentCloset.OpenDoor();
        currentCloset.DisableCollider();
        positionBeforeHiding = transform.position;
        rotationBeforeHiding = transform.rotation;

        FreezePlayer(false);
        StartCoroutine(MoveToPosition(currentCloset.hidingSpot, true));
    }

    public void Unhide()
    {
        if (!isHiding || isInteractingWithCloset) return;

        currentCloset.OpenDoor();
        StartCoroutine(MoveToPosition(null, false));
    }

    public void FreezePlayer(bool unlockMouseCursor)
    {
        if (playerMovement != null) playerMovement.enabled = false;
        if (characterController != null) characterController.enabled = false;

        if (mouseMovement != null)
        {
            if (unlockMouseCursor)
            {
                mouseMovement.UnlockMouse();
            }
            mouseMovement.enabled = false;
        }
    }

    public void UnfreezePlayer()
    {
        if (playerMovement != null) playerMovement.enabled = true;
        if (characterController != null) characterController.enabled = true;

        if (mouseMovement != null)
        {
            mouseMovement.enabled = true;
            mouseMovement.LockMouse();
        }
    }

    private IEnumerator MoveToPosition(Transform destinationTransform, bool hiding)
    {
        isInteractingWithCloset = true;

        float duration = 0.65f;
        float elapsedTime = 0f;

        Vector3 startingPosition = transform.position;
        Quaternion startingRotation = transform.rotation;

        Vector3 targetPosition = hiding ? destinationTransform.position : positionBeforeHiding;
        Quaternion targetRotation = hiding ? destinationTransform.rotation : rotationBeforeHiding;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
        isHiding = hiding;

        if (isHiding)
        {
            currentCloset.CloseDoor();
        }
        else
        {
            UnfreezePlayer();
            currentCloset.CloseDoor();
            currentCloset.EnableCollider();
            currentCloset.TriggerCooldown();
            currentCloset = null;
        }

        isInteractingWithCloset = false;
    }
}