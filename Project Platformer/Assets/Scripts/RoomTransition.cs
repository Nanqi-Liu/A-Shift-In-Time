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
        if (collision.tag == "Player")
        {
            _virtualCameraGameObject.SetActive(true);
            Debug.Log("Player enter camera: " + transform.name);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _virtualCameraGameObject.SetActive(false);
            Debug.Log("Player exit camera: " + transform.name);
        }
    }
}
