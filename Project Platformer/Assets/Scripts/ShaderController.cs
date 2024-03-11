using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShaderController : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private FullScreenPassRendererFeature _fullScreenShockwaveEffect;
    [SerializeField]
    private Material _spriteChangeableMaterial;
    [SerializeField]
    private FullScreenPassRendererFeature _fullScreenBorderEffect;
    [SerializeField]
    private Color _presentColor;
    [SerializeField]
    private Color _futureColor;

    private Material _fullScreenShockwaveEffectMaterial;
    private Material _fullScreenBorderEffectMaterial;

    private int _shaderRadiusID = Shader.PropertyToID("_Radius");
    private int _spriteShaderInvertedID = Shader.PropertyToID("_Inverted");
    private int _shaderFocalPointID = Shader.PropertyToID("_FocalPoint");

    private int _shaderColorID = Shader.PropertyToID("_Color");
    private int _shaderOpacity = Shader.PropertyToID("_Opacity");

    private bool _isShaderTransformationRunning = false;

    private IEnumerator _lastCo;

    private void Awake()
    {
        _fullScreenShockwaveEffect.SetActive(false);
        _fullScreenBorderEffect.SetActive(false);
        _fullScreenShockwaveEffectMaterial = _fullScreenShockwaveEffect.passMaterial;
        _fullScreenBorderEffectMaterial = _fullScreenBorderEffect.passMaterial;
    }

    public void InitShaders()
    {
        _fullScreenShockwaveEffect.SetActive(false);
        _fullScreenBorderEffect.SetActive(false);

        _spriteChangeableMaterial.SetInt(_spriteShaderInvertedID, 0);
        _spriteChangeableMaterial.SetFloat(_shaderRadiusID, 0);

        _fullScreenBorderEffectMaterial.SetColor(_shaderColorID, _presentColor);
    }

    public void StartShaderTransformation(float duration)
    {
        //Debug.Log("Start Shader");
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
            
        _lastCo = ShaderTransformation(duration);
        StartCoroutine(_lastCo);
    }

    private IEnumerator ShaderTransformation(float duration)
    {
        _isShaderTransformationRunning = true;
        _fullScreenShockwaveEffect.SetActive(true);
        _fullScreenBorderEffect.SetActive(true);

        _spriteChangeableMaterial.SetVector(_shaderFocalPointID, playerTransform.position);
        _fullScreenShockwaveEffectMaterial.SetVector(_shaderFocalPointID, Camera.main.WorldToViewportPoint(playerTransform.position));
        _fullScreenBorderEffectMaterial.SetColor(_shaderColorID,
            _spriteChangeableMaterial.GetInt(_spriteShaderInvertedID) == 0 ? _futureColor : _presentColor);

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.fixedDeltaTime;
            //Debug.Log(elapsedTime);

            float radius = Mathf.Lerp(0, 1, (elapsedTime / duration));
            _fullScreenShockwaveEffectMaterial.SetFloat(_shaderRadiusID, radius * 2);
            _spriteChangeableMaterial.SetFloat(_shaderRadiusID, radius);
            _fullScreenBorderEffectMaterial.SetFloat(_shaderOpacity, 1 - radius);

            yield return null;
        }

        int inverted = _spriteChangeableMaterial.GetInt(_spriteShaderInvertedID);
        _spriteChangeableMaterial.SetFloat(_shaderRadiusID, 0);
        _spriteChangeableMaterial.SetInt(_spriteShaderInvertedID, 1 - inverted);

        _fullScreenShockwaveEffect.SetActive(false);
        _fullScreenBorderEffect.SetActive(false);

        _isShaderTransformationRunning = false;
        yield return null;
    }
}
