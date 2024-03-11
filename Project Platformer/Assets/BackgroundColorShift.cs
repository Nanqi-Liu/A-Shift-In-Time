using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorShift : MonoBehaviour
{
    private SpriteRenderer _sr;

    [SerializeField]
    private Color _presentColor;
    [SerializeField]
    private Color _futureColor;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public IEnumerator ShiftColor(bool isShiftToFuture, float shiftDuration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < shiftDuration)
        {
            elapsedTime += Time.fixedDeltaTime;
            if (isShiftToFuture)
            {
                _sr.color = Color.Lerp(_presentColor, _futureColor, (elapsedTime / shiftDuration));
            }
            else
            {
                _sr.color = Color.Lerp(_futureColor, _presentColor, (elapsedTime / shiftDuration));
            }

            yield return null;
        }
        yield return null;
    }
}
