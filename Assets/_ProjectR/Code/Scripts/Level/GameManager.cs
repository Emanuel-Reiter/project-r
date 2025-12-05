using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [Header("Player")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _playerCameraPrefab;

    private void Awake()
    {
        // Don't destroy on load settup
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        // Instance initialization
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

}
