using System;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private InteractType Type;

    public static event Action OnInteract;

    private void Start() {
        InitInteractable();
    }

    private void InitInteractable()
    {
        switch (Type)
        {
            case InteractType.dialogue: 
                OnInteract = Test1;
                break;
            case InteractType.teleport:
                OnInteract = Test2;
                break;
            case InteractType.other:
                OnInteract = Test3;
                break;
        }   
    }

    public void Interact()
    {
        OnInteract?.Invoke();
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
}
