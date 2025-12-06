using UnityEngine;

public class PlayerDebugUI : MonoBehaviour
{
    // Exteral player references
    private PlayerDependencies _deps;
    private PlayerStateManager _state;
    private PlayerLocomotion _locomotion;

    // Framerate calculation
    private int _lastFrameIndex;
    private float[] _frameDeltaTimeArray;
    private int _framerate;

    private bool _isCursorEnabled = false;
    public bool IsCursorLocked => _isCursorEnabled;

    [Header("Params")]
    [SerializeField] private bool _showFPS = true;
    [SerializeField] private bool _showPlayerMetrics = true;

    private void Awake()
    {
        _frameDeltaTimeArray = new float[512];
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 0;
    }

    private void Start()
    {
        _deps = GetComponent<PlayerDependencies>();
        _state = GetComponent<PlayerStateManager>();
        _locomotion = GetComponent<PlayerLocomotion>();

        ToggleCursor(true);
    }

    private void Update()
    {
        DisplayFrametate();

        if (Input.GetKeyDown(KeyCode.P)) Debug.Break();
    }

    private void ToggleCursor(bool toggle)
    {
        if (toggle)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        
        _isCursorEnabled = toggle;
    }

    private void DisplayFrametate()
    {
        _frameDeltaTimeArray[_lastFrameIndex] = Time.unscaledDeltaTime;
        _lastFrameIndex = (_lastFrameIndex + 1) % _frameDeltaTimeArray.Length;

        _framerate = Mathf.RoundToInt(CalculateAverageFramerate());
    }

    private float CalculateAverageFramerate()
    {
        float total = 0.0f;

        foreach (float deltaTime in _frameDeltaTimeArray)
        {
            total += deltaTime;
        }

        return _frameDeltaTimeArray.Length / total;
    }

    private int GUIPositionY(int row, int height)
    {
        return row * height;
    }

    private void OnGUI()
    {
        int xOffset = 32;
        int width = 512;
        int height = 32;

        GUI.skin.label.fontSize = 24;

        if (_showFPS) GUI.Label(new Rect(xOffset, GUIPositionY(1, height), width, height), $"fps: {_framerate}");

        if (!_showPlayerMetrics) return;

        GUI.Label(new Rect(xOffset, GUIPositionY(2, height), width, height), $"curState: {_state.CurrentState}");
        GUI.Label(new Rect(xOffset, GUIPositionY(3, height), width, height), $"horizontalVel: {_locomotion.HorizontalVel}");
        GUI.Label(new Rect(xOffset, GUIPositionY(4, height), width, height), $"verticalVel: {_locomotion.VerticalVel}");
        
        GUI.Label(new Rect(xOffset, GUIPositionY(6, height), width, height), $"-= ANIMATION INDEXES =-");
        GUI.Label(new Rect(xOffset, GUIPositionY(7, height), width, height), $"armF: {_deps.NetworkedVisuals.GetCurrentArmFIndex()}");
        GUI.Label(new Rect(xOffset, GUIPositionY(8, height), width, height), $"head: {_deps.NetworkedVisuals.GetCurrentHeadIndex()}");
        GUI.Label(new Rect(xOffset, GUIPositionY(9, height), width, height), $"body: {_deps.NetworkedVisuals.GetCurrentBodyIndex()}");
        GUI.Label(new Rect(xOffset, GUIPositionY(10, height), width, height), $"legs: {_deps.NetworkedVisuals.GetCurrentLegsIndex()}");
        
        GUI.Label(new Rect(xOffset, GUIPositionY(12, height), width, height), $"useItemHold: {_deps.Input.IsUseItemHold}");
        
        GUI.Label(new Rect(xOffset, GUIPositionY(14, height), width, height), $"isGrounded: {_deps.Locomotion.IsGrounded}");
        GUI.Label(new Rect(xOffset, GUIPositionY(15, height), width, height), $"isJuping: {_deps.Locomotion.IsJumping}");
    }
}
