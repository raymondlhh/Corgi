using UnityEngine;

public class BallInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Pickupable pickupable;
    [SerializeField] private string funNeedId = "fun";

    public string Prompt => pickupable != null && pickupable.IsHeld ? "Drop" : "Play";

    private void Awake()
    {
        pickupable ??= GetComponent<Pickupable>();
    }

    public bool CanInteract(CorgiPlayer player)
    {
        return player != null && player.Actions != null && pickupable != null;
    }

    public void Interact(CorgiPlayer player)
    {
        if (player == null || player.Actions == null || pickupable == null) return;

        var pickupCommand = new PickupCommand(player.Actions);
        pickupCommand.SetTarget(pickupable);

        if (!pickupCommand.CanExecute()) return;

        pickupCommand.Execute();
        player.StateMachine?.ChangeState<PlayingState>();

        if (!string.IsNullOrWhiteSpace(funNeedId))
        {
            player.NeedsSystem?.RestoreNeedDefault(funNeedId);
        }
    }
}
