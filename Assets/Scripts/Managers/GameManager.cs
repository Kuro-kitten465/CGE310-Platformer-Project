using UnityEngine;
using KuroNeko.Utilities.DesignPattern;

public class GameManager : MonoSingleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
    }
}
