using Unity.Netcode;
using UnityEngine;

public class NetworkedPlayerVisuals : NetworkBehaviour
{
    // Temp animation and frame ref implementation, until item system is implemented
    
    [Header("Temp")]
    [SerializeField] private SpriteRenderer _armFRenderer;
    [SerializeField] private SpriteRenderer _headRenderer;
    [SerializeField] private SpriteRenderer _bodyRenderer;
    [SerializeField] private SpriteRenderer _legsRenderer;
    
    [SerializeField] private Sprite[] _armFSprites;
    [SerializeField] private Sprite[] _headSprites;
    [SerializeField] private Sprite[] _bodySprites;
    [SerializeField] private Sprite[] _legsSprites;

    private NetworkVariable<int> _currentArmFIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<int> _currentHeadIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<int> _currentBodyIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<int> _currentLegsIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    public override void OnNetworkSpawn()
    {
        _currentArmFIndex.OnValueChanged += OnArmFIndexChanged;
        _currentHeadIndex.OnValueChanged += OnHeadIndexChanged;
        _currentBodyIndex.OnValueChanged += OnBodyIndexChanged;
        _currentLegsIndex.OnValueChanged += OnLegsIndexChanged;

        // Update all parts to correct sprite initialization
        UpdateAllParts();
    }

    public override void OnNetworkDespawn()
    {
        _currentArmFIndex.OnValueChanged -= OnArmFIndexChanged;
        _currentHeadIndex.OnValueChanged -= OnHeadIndexChanged;
        _currentBodyIndex.OnValueChanged -= OnBodyIndexChanged;
        _currentLegsIndex.OnValueChanged -= OnLegsIndexChanged;
    }

    private void OnArmFIndexChanged(int oldValue, int newValue)
    {
        UpdateArmFSprite();
    }
    private void OnHeadIndexChanged(int oldValue, int newValue)
    {
        UpdateHeadSprite();
    }
    private void OnBodyIndexChanged(int oldValue, int newValue)
    {
        UpdateBodySprite();
    }
    private void OnLegsIndexChanged(int oldValue, int newValue)
    {
        UpdateLegsSprite();
    }

    private void UpdateAllParts()
    {
        UpdateArmFSprite();
        UpdateHeadSprite();
        UpdateBodySprite();
        UpdateLegsSprite();
    }

    private void UpdateArmFSprite()
    {
        if (_armFRenderer == null || _armFSprites == null || _armFSprites.Length < 1) return;

        int index = Mathf.Clamp(_currentArmFIndex.Value, 0, _armFSprites.Length - 1);
        _armFRenderer.sprite = _armFSprites[index];
    }

    private void UpdateBodySprite()
    {
        if (_bodyRenderer == null || _bodySprites == null || _bodySprites.Length < 1) return;

        int index = Mathf.Clamp(_currentBodyIndex.Value, 0, _bodySprites.Length - 1);
        _bodyRenderer.sprite = _bodySprites[index];
    }

    private void UpdateHeadSprite()
    {
        if (_headRenderer == null || _headSprites == null || _headSprites.Length < 1) return;

        int index = Mathf.Clamp(_currentHeadIndex.Value, 0, _headSprites.Length - 1);
        _headRenderer.sprite = _headSprites[index];
    }

    private void UpdateLegsSprite()
    {
        if (_legsRenderer == null || _legsSprites == null || _legsSprites.Length < 1) return;

        int index = Mathf.Clamp(_currentLegsIndex.Value, 0, _legsSprites.Length - 1);
        _legsRenderer.sprite = _legsSprites[index];
    }

    #region Public methods
    public int GetCurrentArmFIndex() => _currentArmFIndex.Value;
    public void SetArmFSprite(int index)
    {
        if (!IsOwner) return;

        _currentArmFIndex.Value = index;
    }

    public int GetCurrentHeadIndex() => _currentHeadIndex.Value;
    public void SetHeadSprite(int index)
    {
        if (!IsOwner) return;

        _currentHeadIndex.Value = index;
    }

    public int GetCurrentBodyIndex() => _currentBodyIndex.Value;
    public void SetBodySprite(int index)
    {
        if (!IsOwner) return;

        _currentBodyIndex.Value = index;
    }

    public int GetCurrentLegsIndex() => _currentLegsIndex.Value;
    public void SetLegsSprite(int index)
    {
        if (!IsOwner) return;

        _currentLegsIndex.Value = index;
    }
    #endregion
}