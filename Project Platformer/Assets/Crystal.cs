using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField]
    private GameObject _crystalText;
    [SerializeField]
    private GameObject _invWall;
    private void Start()
    {
        _crystalText.SetActive(false);
        _invWall.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AudioManager.instance.PlaySound("Crystal");
            PlayerManager.instance.isTimeshiftWhenJump = true;
            _crystalText.SetActive(true);
            _invWall.SetActive(true);
            Destroy(gameObject);
        }
    }
}
