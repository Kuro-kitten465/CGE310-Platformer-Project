using System.Linq;
using UnityEngine;

public class TestItem : MonoBehaviour, ICollectible
{
    public void Collect(params object[] args)
    {
        var player = args.First() as PlayerManager;

        if (player != null) Debug.Log("Player Working");

        Destroy(gameObject);
    }
}
