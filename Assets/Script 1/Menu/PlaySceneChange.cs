using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlaySceneChange : MonoBehaviour {

    public static PlaySceneChange ins;
    public string GamePlaySceneName;
    public string GameEndSceneName;
    void Awake()
    {
        ins = this;
    }
    public void SceneChange(int Winner)
    {
        if (Winner == Utillity.BLUEPLAYER)
            GameData.ins.RedIndex++;
        else
            GameData.ins.BlueIndex++;
        StartCoroutine("ieSceneChange",Winner);
    }
    public IEnumerator ieSceneChange(int Winner)
    {
        yield return StartCoroutine(GameData.ins.EndComent(Winner));
        yield return StartCoroutine(FadeOut.ins.RunFadeOut());
        yield return new WaitForSeconds(0.5f);
        if (GameData.ins.BlueIndex == 2 || GameData.ins.RedIndex == 2)
        {
            SceneManager.LoadScene(GameEndSceneName);
        }
        else
        {
            UIImageChanger.Release();
            UIIsVisible.Release();
            UINumberSet.Release();
            UIProgressBar.Release();
            Application.LoadLevel(GamePlaySceneName);

        }
    }
    public void ProgramExit()
    {

    }
}
