using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _mainMenuUI;
    [SerializeField] GameObject _instructionsUI;

    private void Start()
    {
        _mainMenuUI.SetActive(true);
        _instructionsUI.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowInstructions(bool toggle)
    {
        _instructionsUI.SetActive(toggle);
        _mainMenuUI.SetActive(!toggle);
    }
}
