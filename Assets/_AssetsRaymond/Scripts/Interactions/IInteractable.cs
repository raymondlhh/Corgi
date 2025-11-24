public interface IInteractable
{
    string Prompt { get; }
    bool CanInteract(CorgiPlayer player);
    void Interact(CorgiPlayer player);
}
