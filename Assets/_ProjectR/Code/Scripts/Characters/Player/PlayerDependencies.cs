using Unity.Netcode;
using UnityEngine;

public class PlayerDependencies : NetworkBehaviour
{
    public Rigidbody2D Rigidbody { get; private set; }
    public PlayerInputManager Input { get; private set; }
    public PlayerUseItem UseItem { get; private set; }
    public NetworkedPlayerVisuals NetworkedVisuals { get; private set; }
    public PlayerAnimationManager Animation { get; private set; }
    public PlayerLocomotion Locomotion { get; private set; }

    public PlayerItemContainer ItemContainer { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Input = GetComponent<PlayerInputManager>();
        UseItem = GetComponent<PlayerUseItem>();
        NetworkedVisuals = GetComponent<NetworkedPlayerVisuals>();
        Animation = GetComponent<PlayerAnimationManager>();
        Locomotion = GetComponent<PlayerLocomotion>();
        ItemContainer = GetComponentInChildren<PlayerItemContainer>();
    }
}