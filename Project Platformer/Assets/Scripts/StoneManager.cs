using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : MonoBehaviour
{
    public static StoneManager instance;
    private Stone[] _stones;

    [SerializeField] 
    private LayerMask _groundLayer;
    [SerializeField]
    private Sprite _futureWallSprite;

    private void Awake()
    {
        instance = this;

        _stones = new Stone[transform.childCount];
        int index = 0;
        foreach (Transform childStone in transform)
        {
            _stones[index] = childStone.GetComponent<Stone>();
            index += 1;
        }
    }

    private void Start()
    {
        for (int i = 0; i < _stones.Length; i++)
        {
            _stones[i].futureStone = Instantiate(_stones[i], transform).GetComponent<Stone>();
            _stones[i].futureStone.name = _stones[i].name + "_Future";
            _stones[i].futureStone.GetComponent<SpriteRenderer>().sprite = _futureWallSprite;
            _stones[i].futureStone.gameObject.SetActive(false);
        }
    }

    public void CastStones(bool isToFuture)
    {
        for (int i = 0; i < _stones.Length; i++)
        {
            if (isToFuture)
            {
                RaycastHit2D hit = Physics2D.Raycast(_stones[i].transform.position + Vector3.down * 0.5f, Vector2.down, float.MaxValue, _groundLayer);
                if (hit.collider != null)
                {
                    _stones[i].futureStone.transform.position = hit.point + Vector2.up * 0.5f;
                }
                _stones[i].gameObject.SetActive(false);
                _stones[i].futureStone.gameObject.SetActive(true);
            }
            else
            {
                _stones[i].gameObject.SetActive(true);
                _stones[i].futureStone.gameObject.SetActive(false);
            }
        }
    }
}
