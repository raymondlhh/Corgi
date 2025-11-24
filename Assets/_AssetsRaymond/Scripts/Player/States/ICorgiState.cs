public interface ICorgiState
{
    string Id { get; }
    bool CanEnter();
    void Enter();
    void Tick(float deltaTime);
    void Exit();
}
