using System.Linq;
using UnityEngine;

public class PowerUpHealth : MonoBehaviour, ICollectible
{
    public void Collect(params object[] args)
    {
        var player = args.First() as PlayerManager;
        player.RestoreHealth();

        Destroy(gameObject);
    }
}
