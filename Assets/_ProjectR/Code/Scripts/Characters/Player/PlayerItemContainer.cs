using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerItemContainer : NetworkBehaviour
{
    public SpriteRenderer ItemRenderer { get; private set; }

    private NetworkVariable<bool> _itemRendererActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<float> _itemContainerRotation = new NetworkVariable<float> (0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    private Vector2 _itemContainerAngleMinMax = new Vector2(45f, -140f);

    private Coroutine _useItemCoroutine;
   
    public override void OnNetworkSpawn()
    {
        ItemRenderer = GetComponentInChildren<SpriteRenderer>();

        _itemRendererActive.OnValueChanged += OnItemRendererActiveChanged;
        _itemContainerRotation.OnValueChanged += OnItemContainerRotationChanged;

        // Setup item visibility
        ToggleItemRenderer();
    }

    public override void OnNetworkDespawn()
    {
        _itemRendererActive.OnValueChanged -= OnItemRendererActiveChanged;
        _itemContainerRotation.OnValueChanged -= OnItemContainerRotationChanged;
    }

    private void OnItemRendererActiveChanged(bool oldValue, bool newValue)
    {
        ToggleItemRenderer();
    }

    private void OnItemContainerRotationChanged(float oldValue, float newValue)
    {
        UpdateItemContainerRotation();
    }

    private void ToggleItemRenderer()
    {
        ItemRenderer.gameObject.SetActive(_itemRendererActive.Value);
    }

    public void SetItemRendererActive(bool active)
    {
        if (!IsOwner) return;
        _itemRendererActive.Value = active;
    }

    public void SetItemContainerZRotation(float zRot)
    {
        if (!IsOwner) return;
        _itemContainerRotation.Value = zRot;
    }

    private void UpdateItemContainerRotation()
    {
        Vector3 currentRot = new Vector3(0f, 0f, _itemContainerRotation.Value);
        transform.localEulerAngles = currentRot;
    }

    public void UseItem(float animDuration)
    {
        if (!IsOwner) return;

        if (_useItemCoroutine != null) StopCoroutine(_useItemCoroutine);
        _useItemCoroutine = StartCoroutine(UseItemCorroutine(animDuration));
    }

    private IEnumerator UseItemCorroutine(float animDuration)
    {
        // Settup pivot rotation
        float startRotation = _itemContainerAngleMinMax.x;
        float targetRotation = _itemContainerAngleMinMax.y;
        SetItemRendererActive(true);

        float currentRot = startRotation;

        float time = 0.0f;
        while (time < animDuration)
        {
            time += Time.deltaTime;
            currentRot = Mathf.Lerp(startRotation, targetRotation, time / animDuration);
            SetItemContainerZRotation(currentRot);
            yield return null; 
        }

        SetItemRendererActive(false);
        currentRot = targetRotation;
        SetItemContainerZRotation(currentRot);
    }
}
