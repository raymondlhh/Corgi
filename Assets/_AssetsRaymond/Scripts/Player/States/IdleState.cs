using UnityEngine;

public sealed class IdleState : ICorgiState
{
    private static readonly int SpeedParam = Animator.StringToHash("Speed");
    private static readonly int SleepBool = Animator.StringToHash("IsSleeping");

    private readonly CorgiPlayer _player;
    private readonly CorgiStateMachine _stateMachine;

    public IdleState(CorgiPlayer player, CorgiStateMachine stateMachine)
    {
        _player = player;
        _stateMachine = stateMachine;
    }

    public string Id => "Idle";

    public bool CanEnter() => true;

    public void Enter()
    {
        if (_player?.Animator != null)
        {
            _player.Animator.SetBool(SleepBool, false);
        }
    }

    public void Tick(float deltaTime)
    {
        if (_player?.Animator == null) return;

        float speed = _player.Movement != null ? _player.Movement.CurrentSpeed : 0f;
        _player.Animator.SetFloat(SpeedParam, speed);
    }

    public void Exit()
    {
    }
}
