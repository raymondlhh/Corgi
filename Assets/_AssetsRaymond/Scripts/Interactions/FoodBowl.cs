using UnityEngine;

public class FoodBowl : MonoBehaviour, IInteractable
{
    [SerializeField] private string needId = "hunger";
    [SerializeField] private ParticleSystem eatEffect;

    public string Prompt => "Eat";

    public bool CanInteract(CorgiPlayer player)
    {
        return player != null && player.NeedsSystem != null && player.NeedsSystem.TryGetNeed(needId, out _);
    }

    public void Interact(CorgiPlayer player)
    {
        if (player == null || player.Actions == null) return;

        player.Actions.Eat();

        if (eatEffect != null)
        {
            eatEffect.Play();
        }
    }
}
