using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private PoolSystem pool;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        pool.Initialize();
        gameManager.Initialize();
        uiManager.Initialize();
        gameManager.StartGame();
    }
}
