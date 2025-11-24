using System;
using System.Collections.Generic;
using UnityEngine;

public class CorgiStateMachine : MonoBehaviour
{
    private readonly Dictionary<Type, ICorgiState> _states = new();
    private ICorgiState _currentState;
    private CorgiPlayer _player;
    private bool _isConfigured;

    public event Action<ICorgiState> StateChanged;

    public ICorgiState CurrentState => _currentState;

    public void Configure(CorgiPlayer player)
    {
        _player = player;
        RegisterStates();
        ChangeState<IdleState>();
        _isConfigured = true;
    }

    public void Tick(float deltaTime)
    {
        if (!_isConfigured) return;

        _currentState?.Tick(deltaTime);
    }

    public void ChangeState<T>() where T : ICorgiState
    {
        if (_states.TryGetValue(typeof(T), out ICorgiState state))
        {
            ChangeState(state);
        }
    }

    public void ChangeState(ICorgiState nextState)
    {
        if (nextState == null || nextState == _currentState) return;
        if (!nextState.CanEnter()) return;

        _currentState?.Exit();
        _currentState = nextState;
        _currentState.Enter();

        StateChanged?.Invoke(_currentState);
    }

    private void RegisterStates()
    {
        _states.Clear();

        if (_player == null)
        {
            Debug.LogError("CorgiStateMachine could not locate CorgiPlayer.");
            return;
        }

        AddState(new IdleState(_player, this));
        AddState(new SleepingState(_player, this));
        AddState(new PlayingState(_player, this));
    }

    private void AddState(ICorgiState state)
    {
        if (state == null) return;
        _states[state.GetType()] = state;
    }
}
