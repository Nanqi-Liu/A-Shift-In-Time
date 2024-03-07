using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class ShaderController : MonoBehaviour
{
    PlayerControls inputActions;

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private FullScreenPassRendererFeature _fullScreenShockwaveEffect;
    [SerializeField]
    private Material _spriteChangeableMaterial;
    [SerializeField]
    private float _effectDuration = 1f;

    private Material _fullScreenShockwaveEffectMaterial;

    private int _shaderRadiusID = Shader.PropertyToID("_Radius");
    private int _spriteShaderInvertedID = Shader.PropertyToID("_Inverted");
    private int _shaderFocalPointID = Shader.PropertyToID("_FocalPoint");

    private bool _isShaderTransformationRunning = false;

    // Debug Variables
    private IEnumerator _lastCo;

    private void Awake()
    {
        inputActions = new PlayerControls();
        _fullScreenShockwaveEffect.SetActive(false);
        _fullScreenShockwaveEffectMaterial = _fullScreenShockwaveEffect.passMaterial;
    }

    private void OnEnable()
    {
        inputActions.Test.Switch.performed += StartShaderTransformation;
        inputActions.Test.Switch.Enable();
    }

    private void OnDisable()
    {
        inputActions.Test.Switch.performed -= StartShaderTransformation;
        inputActions.Test.Switch.Disable();
    }

    void StartShaderTransformation(InputAction.CallbackContext obj)
    {
        Debug.Log("Start Shader");
        if (_lastCo != null)
        {
            StopCoroutine(_lastCo);
            if (_isShaderTransformationRunning)
            {
                int inverted = _spriteChangeableMaterial.GetInt(_spriteShaderInvertedID);
                _spriteChangeableMaterial.SetInt(_spriteShaderInvertedID, 1 - inverted);
            }
            _isShaderTransformationRunning = false;
        }
            
        _lastCo = ShaderTransformation();
        StartCoroutine(_lastCo);
    }

    private IEnumerator ShaderTransformation()
    {
        _isShaderTransformationRunning = true;
        _fullScreenShockwaveEffect.SetActive(true);

        _spriteChangeableMaterial.SetVector(_shaderFocalPointID, playerTransform.position);
        _fullScreenShockwaveEffectMaterial.SetVector(_shaderFocalPointID, Camera.main.WorldToViewportPoint(playerTransform.position));

        float elapsedTime = 0f;
        while (elapsedTime < _effectDuration)
        {
            elapsedTime += Time.fixedDeltaTime;
            //Debug.Log(elapsedTime);

            float radius = Mathf.Lerp(0, 1, (elapsedTime / _effectDuration));
            _fullScreenShockwaveEffectMaterial.SetFloat(_shaderRadiusID, radius * 2);
            _spriteChangeableMaterial.SetFloat(_shaderRadiusID, radius);

            yield return null;
        }

        int inverted = _spriteChangeableMaterial.GetInt(_spriteShaderInvertedID);
        _spriteChangeableMaterial.SetFloat(_shaderRadiusID, 0);
        _spriteChangeableMaterial.SetInt(_spriteShaderInvertedID, 1 - inverted);

        _isShaderTransformationRunning = false;
        yield return null;
    }
}