using System.Collections.Generic;
using UnityEngine;

public class CorgiActionInput : MonoBehaviour
{
    [SerializeField] private CorgiPlayer player;

    private readonly Dictionary<KeyCode, ICommand> _bindings = new();

    private void Awake()
    {
        player ??= GetComponent<CorgiPlayer>();

        if (player == null)
        {
            Debug.LogError("CorgiActionInput requires a CorgiPlayer reference.");
            enabled = false;
            return;
        }

        BuildBindings();
    }

    private void Update()
    {
        foreach (KeyValuePair<KeyCode, ICommand> binding in _bindings)
        {
            if (Input.GetKeyDown(binding.Key) && binding.Value.CanExecute())
            {
                binding.Value.Execute();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            HandleInteract();
        }

        if (Input.GetKeyDown(KeyCode.Q) && player.Actions.IsHoldingItem)
        {
            player.Actions.Drop();
        }
    }

    private void BuildBindings()
    {
        _bindings.Clear();

        var barkCommand = new BarkCommand(player.Actions);
        var sitCommand = new SitCommand(player.Actions);
        var rollCommand = new RollCommand(player.Actions);

        _bindings.Add(KeyCode.Alpha1, barkCommand);
        _bindings.Add(KeyCode.Alpha2, sitCommand);
        _bindings.Add(KeyCode.Alpha3, rollCommand);
    }

    private void HandleInteract()
    {
        CorgiInteractionDetector detector = player.InteractionDetector;
        if (detector == null)
        {
            return;
        }

        IInteractable interactable = detector.CurrentInteractable;

        if (interactable != null && interactable.CanInteract(player))
        {
            interactable.Interact(player);
            return;
        }

        if (player.Actions.IsHoldingItem)
        {
            player.Actions.Drop();
        }
    }
}
