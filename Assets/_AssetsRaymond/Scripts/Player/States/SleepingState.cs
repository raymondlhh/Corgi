using UnityEngine;

public sealed class SleepingState : ICorgiState
{
    private static readonly int SleepBool = Animator.StringToHash("IsSleeping");

    private readonly CorgiPlayer _player;
    private readonly CorgiStateMachine _stateMachine;
    private readonly string _energyNeedId;
    private readonly float _energyRegenPerSecond;

    public SleepingState(CorgiPlayer player, CorgiStateMachine stateMachine, string energyNeedId = "energy", float energyRegenPerSecond = 20f)
    {
        _player = player;
        _stateMachine = stateMachine;
        _energyNeedId = energyNeedId;
        _energyRegenPerSecond = energyRegenPerSecond;
    }

    public string Id => "Sleeping";

    public bool CanEnter()
    {
        return _player != null && _player.Animator != null;
    }

    public void Enter()
    {
        if (_player?.Animator == null) return;

        _player.Animator.SetBool(SleepBool, true);
    }

    public void Tick(float deltaTime)
    {
        if (_player?.NeedsSystem != null && !string.IsNullOrWhiteSpace(_energyNeedId))
        {
            _player.NeedsSystem.RestoreNeed(_energyNeedId, _energyRegenPerSecond * deltaTime);
        }
    }

    public void Exit()
    {
        if (_player?.Animator == null) return;

        _player.Animator.SetBool(SleepBool, false);
    }
}
