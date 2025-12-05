using System.Collections;
using UnityEngine;
using System.Linq;
using System;

public class PlayerAnimationManager : MonoBehaviour
{
    private PlayerDependencies _deps;

    private int _upperBodyFrameIndex = 0;
    private Coroutine _bodyCoroutine;

    private int _baseFrameIndex = 0;
    private Coroutine _legsCoroutine;

    [Header("Idle anim params")]

    [SerializeField] private PlayerAnimationDataSO _idleBodyAnim;
    public PlayerAnimationDataSO IdleBodyAnim => _idleBodyAnim;

    [SerializeField] private PlayerAnimationDataSO _idleLegsAnim;
    public PlayerAnimationDataSO IdleLegsAnim => _idleLegsAnim;

    [Header("Move anim params")]

    [SerializeField] private PlayerAnimationDataSO _moveBodyAnim;
    public PlayerAnimationDataSO MoveBodyAnim => _moveBodyAnim;

    [SerializeField] private PlayerAnimationDataSO _moveLegsAnim;
    public PlayerAnimationDataSO MoveLegsAnim => _moveLegsAnim;

    [Header("Use item anim params")]
    [SerializeField] private PlayerAnimationDataSO _useItemSwingAnim;
    public PlayerAnimationDataSO UseItemSwingAnim => _useItemSwingAnim;
    
    [SerializeField] private PlayerAnimationDataSO _useItemConsumeAnim;
    public PlayerAnimationDataSO UseItemConsumeAnim => _useItemConsumeAnim;

    private const float BASE_ANIM_DURATION = 1f;
    public float BaseAnimDurationConstant => BASE_ANIM_DURATION;

    private void Start()
    {
        _deps = GetComponent<PlayerDependencies>();
    }

    #region Play animation methods

    public void PlayUpperBodyAnimation(PlayerAnimationDataSO data, Action callback)
    {
        if (_deps == null || _deps.NetworkedVisuals == null) return;
        if (_bodyCoroutine != null) StopCoroutine(_bodyCoroutine);

        _bodyCoroutine = StartCoroutine(UpperBodyAnimationCoroutine(data, callback));
    }

    public void PlayBaseAnimation(PlayerAnimationDataSO data)
    {
        if (_deps == null || _deps.NetworkedVisuals == null) return;
        if (_legsCoroutine != null) StopCoroutine(_legsCoroutine);

        _legsCoroutine = StartCoroutine(BaseAnimationCoroutine(data));
    }
    #endregion

    #region Animation coroutines

    private IEnumerator UpperBodyAnimationCoroutine(PlayerAnimationDataSO data, Action callback)
    {
        _upperBodyFrameIndex = 0;

        do
        {
            _deps.NetworkedVisuals.SetArmFSprite(data.FrameIndex[_upperBodyFrameIndex]);
            _deps.NetworkedVisuals.SetHeadSprite(data.FrameIndex[_upperBodyFrameIndex]);
            _deps.NetworkedVisuals.SetBodySprite(data.FrameIndex[_upperBodyFrameIndex]);

            yield return new WaitForSeconds(BASE_ANIM_DURATION / data.SamplingRate);

            _upperBodyFrameIndex++;
            if (_upperBodyFrameIndex >= data.FrameIndex.Length)
            {
                callback?.Invoke();
                yield break;
            }
        }
        while (true);
    }

    private IEnumerator BaseAnimationCoroutine(PlayerAnimationDataSO data)
    {
        _baseFrameIndex = 0;

        do
        {
            bool isUsingItem = _deps.UseItem.GetIsUsingItem();

            if(!isUsingItem) _deps.NetworkedVisuals.SetArmFSprite(data.FrameIndex[_baseFrameIndex]);
            if (!isUsingItem) _deps.NetworkedVisuals.SetHeadSprite(data.FrameIndex[_baseFrameIndex]);
            if (!isUsingItem) _deps.NetworkedVisuals.SetBodySprite(data.FrameIndex[_baseFrameIndex]);
            _deps.NetworkedVisuals.SetLegsSprite(data.FrameIndex[_baseFrameIndex]);

            yield return new WaitForSeconds(BASE_ANIM_DURATION / data.SamplingRate);

            _baseFrameIndex++;
            if (_baseFrameIndex >= data.FrameIndex.Length) _baseFrameIndex = 0;
        }
        while (true);
    }
    #endregion
}