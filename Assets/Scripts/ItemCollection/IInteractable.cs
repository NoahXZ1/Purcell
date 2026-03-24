public interface IInteractable
{
    string PromptText { get; }
    void Interact(PlayerInteraction player);
}
