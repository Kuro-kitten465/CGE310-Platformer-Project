using System.Linq;
using UnityEngine;

public class CheckPoint : MonoBehaviour, ICollectible
{
    public void Collect(params object[] args)
    {
        var player = args.First() as PlayerManager;

        player.Checkpoint = gameObject.transform;
    }
}
