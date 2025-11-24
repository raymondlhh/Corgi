using System.Collections;
using UnityEngine;

public class DogBed : MonoBehaviour, IInteractable
{
    [SerializeField, Min(0f)] private float sleepDuration = 4f;

    public string Prompt => "Sleep";

    public bool CanInteract(CorgiPlayer player)
    {
        return player != null && player.Actions != null;
    }

    public void Interact(CorgiPlayer player)
    {
        if (player == null || player.Actions == null) return;

        var sleepCommand = new SleepCommand(player.Actions);
        if (!sleepCommand.CanExecute()) return;

        sleepCommand.Execute();

        if (sleepDuration > 0f)
        {
            player.StartCoroutine(WakeUpRoutine(player));
        }
    }

    private IEnumerator WakeUpRoutine(CorgiPlayer player)
    {
        yield return new WaitForSeconds(sleepDuration);

        if (player != null && player.Actions != null)
        {
            player.Actions.WakeUp();
        }
    }
}
