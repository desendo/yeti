using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BtnScript : MonoBehaviour {

    public void NewGameBtn(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
