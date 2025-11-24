public sealed class BarkCommand : ICommand
{
    private readonly CorgiActions _actions;

    public BarkCommand(CorgiActions actions)
    {
        _actions = actions;
    }

    public string Name => "Bark";

    public bool CanExecute() => _actions != null;

    public void Execute()
    {
        _actions?.Bark();
    }
}
