public sealed class SitCommand : ICommand
{
    private readonly CorgiActions _actions;

    public SitCommand(CorgiActions actions)
    {
        _actions = actions;
    }

    public string Name => "Sit";

    public bool CanExecute() => _actions != null;

    public void Execute()
    {
        _actions?.Sit();
    }
}
