using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSwitchTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TimeshiftManager.instance.ShiftTime();
            // Deactivate when triggered
            gameObject.SetActive(false);
        }
    }
}
