using UnityEngine;

public sealed class PlayingState : ICorgiState
{
    private static readonly int PlayingBool = Animator.StringToHash("IsPlaying");

    private readonly CorgiPlayer _player;
    private readonly CorgiStateMachine _stateMachine;
    private readonly float _idleTimeout;

    private float _noToyTimer;

    public PlayingState(CorgiPlayer player, CorgiStateMachine stateMachine, float idleTimeout = 1.5f)
    {
        _player = player;
        _stateMachine = stateMachine;
        _idleTimeout = idleTimeout;
    }

    public string Id => "Playing";

    public bool CanEnter()
    {
        return _player != null && _player.Animator != null;
    }

    public void Enter()
    {
        _noToyTimer = 0f;

        if (_player?.Animator != null)
        {
            _player.Animator.SetBool(PlayingBool, true);
        }
    }

    public void Tick(float deltaTime)
    {
        if (_player == null) return;

        if (_player.Actions != null && _player.Actions.IsHoldingItem)
        {
            _noToyTimer = 0f;
            return;
        }

        _noToyTimer += deltaTime;

        if (_noToyTimer >= _idleTimeout)
        {
            _stateMachine.ChangeState<IdleState>();
        }
    }

    public void Exit()
    {
        if (_player?.Animator != null)
        {
            _player.Animator.SetBool(PlayingBool, false);
        }
    }
}
