public sealed class PickupCommand : ICommand
{
    private readonly CorgiActions _actions;
    private Pickupable _target;

    public PickupCommand(CorgiActions actions)
    {
        _actions = actions;
    }

    public string Name => "Pickup";

    public void SetTarget(Pickupable pickupable)
    {
        _target = pickupable;
    }

    public bool CanExecute() => _actions != null && _target != null;

    public void Execute()
    {
        if (!CanExecute()) return;

        _actions.Pickup(_target);
    }
}
