using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    private GameObject _virtualCameraGameObject;

    private void Awake()
    {
        _virtualCameraGameObject = transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _virtualCameraGameObject.SetActive(true);
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _virtualCameraGameObject.SetActive(false);
    }
}
