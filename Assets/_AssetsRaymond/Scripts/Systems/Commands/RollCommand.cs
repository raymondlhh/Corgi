public sealed class RollCommand : ICommand
{
    private readonly CorgiActions _actions;

    public RollCommand(CorgiActions actions)
    {
        _actions = actions;
    }

    public string Name => "Roll";

    public bool CanExecute() => _actions != null;

    public void Execute()
    {
        _actions?.Roll();
    }
}
