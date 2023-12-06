using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{

    [SerializeField]
    int sceneNumber;
    [SerializeField]
    InputAction openTest1, openTest2;

    public void Awake()
    {
        openTest1.Enable();
        openTest1.canceled += LoadTest1;
        openTest2.Enable();
        openTest2.canceled += LoadTest2;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(sceneNumber);
    }

    void LoadTest1(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(0);
    }

    void LoadTest2(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(1);
    }

    public void OnDestroy()
    {
        openTest1.Dispose();
        openTest2.Dispose();
    }
}
