using System.Linq;
using UnityEngine;

public class WinChecker : MonoBehaviour, ICollectible
{
    public void Collect(params object[] args)
    {
        var player = args.First() as PlayerManager;
        GameManager.Instance.OnGameWin();

        Destroy(gameObject);
    }
}
