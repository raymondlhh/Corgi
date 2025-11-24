public interface ICommand
{
    string Name { get; }
    bool CanExecute();
    void Execute();
}
