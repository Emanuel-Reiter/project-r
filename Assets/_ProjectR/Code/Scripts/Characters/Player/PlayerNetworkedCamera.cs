using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkedCamera : NetworkBehaviour
{
    private Camera _camera;
    private AudioListener _audioListener;

    private GameObject _target;

    private Vector2 _offset = new Vector2(0f, 2f);

    public override void OnNetworkSpawn()
    {
        float invokeDelay = 0.0f;
        Invoke("HandleCameraLoading", invokeDelay);
    }

    private void HandleCameraLoading()
    {
        if (!IsOwner) return;

        _target = this.gameObject;

        _camera = GetComponentInChildren<Camera>(true);
        _audioListener = GetComponentInChildren<AudioListener>(true);

        _camera.enabled = true;
        _audioListener.enabled = true;

        _camera.transform.parent = null;
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;

        Vector3 target = new Vector3(_target.transform.position.x + _offset.x, _target.transform.position.y + _offset.y, -10f);
        _camera.transform.position = target;
    }
}
