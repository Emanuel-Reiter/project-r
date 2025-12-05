using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAnimData", menuName = "PlayerAnimation/PlayerAnimData")]
public class PlayerAnimationDataSO : ScriptableObject
{
    [Header("Sampling rate")]
    public float SamplingRate;

    [Header("Frames")]
    public int[] FrameIndex;
}
