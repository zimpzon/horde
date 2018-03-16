using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    public string MainScene;

    void Start()
    {
        StartCoroutine(Logic());
    }

    IEnumerator Logic()
    {
        // Make sure splash screen is noticed
        yield return new WaitForSeconds(0.5f);

        // Load main scene
        SceneManager.LoadScene(MainScene, LoadSceneMode.Single);
    }
}
