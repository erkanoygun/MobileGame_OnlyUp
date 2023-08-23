using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    private GameManager _gameManagerScr;
    void Start()
    {
        _gameManagerScr = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }



    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            _gameManagerScr.LevelFinish();
        }
    }

}
