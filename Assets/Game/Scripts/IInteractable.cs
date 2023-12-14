public interface IInteractable
{
    void Interact(PlayerController player);
}

public enum InteractType
{
    dialogue,
    teleport,
    npc,
    other
}
