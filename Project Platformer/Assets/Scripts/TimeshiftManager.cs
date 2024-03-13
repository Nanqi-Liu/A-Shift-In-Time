using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TimeshiftManager : MonoBehaviour
{
    public static TimeshiftManager instance;

    private ShaderController shaderController;

    [SerializeField]
    private GameObject _presentTilemap;
    [SerializeField]
    private GameObject _futureTilemap;
    [SerializeField]
    private Transform _playerTransform;
    [SerializeField]
    private GameObject _backgroundGameObject;
    private BackgroundColorShift[] _backgroundColorShift;

    [SerializeField]
    private float _effectDuration = 1f;

    private TilemapCollider2D _tcPresent;
    private TilemapCollider2D _tcFuture;

    public bool isFuture;
    private void Awake()
    {
        instance = this;
        shaderController = GetComponent<ShaderController>(); 
    }

    private void Start()
    {
        _tcPresent = _presentTilemap.GetComponent<TilemapCollider2D>();
        _tcFuture = _futureTilemap.GetComponent<TilemapCollider2D>();

        _backgroundColorShift = _backgroundGameObject.GetComponentsInChildren<BackgroundColorShift>();

        _tcFuture.enabled = false;
        shaderController.InitShaders();
    }

    public void ShiftTime()
    {
        TilemapCollider2D enabledCollider;
        if (isFuture)
        {
            isFuture = false;
            _tcPresent.enabled = true;
            _tcFuture.enabled = false;

            enabledCollider = _tcPresent;
        }
        else
        {
            isFuture = true;
            _tcPresent.enabled = false;
            _tcFuture.enabled = true;

            enabledCollider = _tcFuture;
        }

        // Get player out if player clip in
        if (enabledCollider.composite.bounds.Contains((Vector2)_playerTransform.position))
        {
            Vector2 edgePoint = enabledCollider.composite.ClosestPoint(_playerTransform.position);
            Vector2 newPosition = edgePoint + (edgePoint - (Vector2)_playerTransform.position).normalized * 0.1f;
            _playerTransform.position = new Vector3(newPosition.x, newPosition.y, _playerTransform.position.z);
        }

        StoneTransition(isFuture);
        // Starts transition
        BackgroundColorTransition();
        shaderController.StartShaderTransformation(_effectDuration);

        // Play SFX
        AudioManager.instance.PlaySound("Timeshift");
    }

    private void BackgroundColorTransition()
    {
        foreach (BackgroundColorShift bgcs in _backgroundColorShift)
        {
            StartCoroutine(bgcs.ShiftColor(isFuture, _effectDuration / 2));
        }
    }

    private void StoneTransition(bool isToFuture)
    {
        StoneManager.instance.CastStones(isToFuture);
    }
}
