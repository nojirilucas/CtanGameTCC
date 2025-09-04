using UnityEngine;

public class HudManager : MonoBehaviour
{
    public static HudManager Instance { get; set; }

    public GameObject middleDot;

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
}