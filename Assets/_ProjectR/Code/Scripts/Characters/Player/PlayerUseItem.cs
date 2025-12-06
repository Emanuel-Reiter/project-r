using Unity.Netcode;
using UnityEngine;

public class PlayerUseItem : NetworkBehaviour
{
    private PlayerDependencies _deps;

    private bool _isUsingItem = false;
    public bool IsUsingItem => _isUsingItem;

    private bool _queueUseItem = false;

    public override void OnNetworkSpawn()
    {
        _deps = GetComponent<PlayerDependencies>();
    }

    public void Use()
    {

        if (!IsOwner || _isUsingItem) return;

        _queueUseItem = false;
        _isUsingItem = true;

        PlayerAnimationDataSO cachedAnim = _deps.Animation.UseItemSwingAnim;

        float frameTimeMultiplaier = 0.95f;
        float itemUseTime = (_deps.Animation.BaseAnimDurationConstant / cachedAnim.SamplingRate) * (cachedAnim.FrameIndex.Length * frameTimeMultiplaier);

        _deps.Animation.PlayUpperBodyAnimation(cachedAnim, () => { _isUsingItem = false; });
        _deps.ItemContainer.UseItem(itemUseTime);
        _deps.Locomotion.RoatateTowardsAimDirection();
    }

    public bool GetIsUsingItem() => _isUsingItem;
}
