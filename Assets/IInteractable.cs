using System.Diagnostics;

public interface IInteractable
{
    /// <summary>
    /// Checks if the interactable can be interacted with.
    /// </summary>
    /// <returns>True if interaction is possible, false otherwise.</returns>
    bool CanInteract();

    /// <summary>
    /// Method to call when the interactable is interacted with.
    /// </summary>
    void Interact();
}
