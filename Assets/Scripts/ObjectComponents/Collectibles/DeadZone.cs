using UnityEngine;

public class DeadZone : MonoBehaviour, IEnemy
{
    public void OnPlayerTrigger(PlayerManager player)
    {
        player.gameObject.transform.position = player.Checkpoint.transform.position;
    }
}
