using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _backGrounds;
    [SerializeField] private GameObject _environment;
    [SerializeField] private GameObject _player;

    public bool _isGamePause = false;

    [Header("UI")]
    [SerializeField] private GameObject _levelUpCanvas;

    private int _part;
    void Start()
    {
        _part = 0;
    }

    
    void Update()
    {
        
    }

    public void LevelFinish()
    {
        _levelUpCanvas.SetActive(true);
        PauseGame();
    }

    public void GoNextLevel()
    {
        _part++;
        if(_part == 1)
        {
            SetActiveChangePart(_backGrounds);
            SetActiveChangePart(_environment);
        }
        if(_part == 2)
        {
            SetActiveChangePart(_backGrounds);
            SetActiveChangePart(_environment);
        }
    }

    private void SetActiveChangePart(GameObject _go)
    {
        _go.transform.GetChild(_part - 1).gameObject.SetActive(false);
        _go.transform.GetChild(_part).gameObject.SetActive(true);
        _player.transform.position = new Vector3(-1f, -3f, 0);

        _levelUpCanvas.SetActive(false);
        ResumeGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        _isGamePause = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        _isGamePause = false;
    }
}
