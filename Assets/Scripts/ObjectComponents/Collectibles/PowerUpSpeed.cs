using System.Linq;
using UnityEngine;

public class PowerUpSpeed : MonoBehaviour, ICollectible
{
    public void Collect(params object[] args)
    {
        var player = args.First() as PlayerManager;
        player.SpeedUp();

        Destroy(gameObject);
    }
}
