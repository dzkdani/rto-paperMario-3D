using System;
using TOI2D;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractType Type;

    public static event Action OnInteract;
    [SerializeField] private Transform teleportPos;
    public Vector3 lastpos;
    public TeleportTarget teleportTarget;
    private GameplayManager gameplayManager;
    private void Start()
    {
        //InitInteractable();
    }

    private void InitInteractable()
    {
        Debug.Log("Interact");
        switch (Type)
        {
            case InteractType.dialogue:
                OnInteract = Test1;
                break;
            case InteractType.teleport:
                if (GameplayManager.instance.teleportSystem != null)
                    GameplayManager.instance.teleportSystem.InitTeleport(teleportTarget, this);
                break;
            case InteractType.other:
                OnInteract = Test3;
                break;
        }
    }

    public void Interact()
    {
        //OnInteract?.Invoke();
        InitInteractable();
    }

    private void Test1()
    {
        Debug.Log("dialog interaction");
    }

    private void Test2()
    {
        Debug.Log("teleport interaction");
    }

    private void Test3()
    {
        Debug.Log("other interaction");
    }

    public void SetLastPosition(Vector3 posTarget)
    {
        lastpos = posTarget;
    }
    public Vector3 GetLastPosition()
    {
        Vector3 posTarget = lastpos;
        return posTarget;
    }

    public Transform GetTeleportPos()
    {
        return teleportPos;
    }
}
