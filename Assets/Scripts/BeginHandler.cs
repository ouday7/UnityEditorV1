using UnityEngine;

public class BeginHandler : MonoBehaviour
{
    [SerializeField]private ObjectPooler _pool;
    [SerializeField]private GameManager _gm;
    private void Awake()
    {
        _pool.Initialize();
        _pool.Begin();
        _gm.Init();
        _gm.Begin();
    }
}