public sealed class SleepCommand : ICommand
{
    private readonly CorgiActions _actions;

    public SleepCommand(CorgiActions actions)
    {
        _actions = actions;
    }

    public string Name => "Sleep";

    public bool CanExecute() => _actions != null;

    public void Execute()
    {
        _actions?.Sleep();
    }
}
