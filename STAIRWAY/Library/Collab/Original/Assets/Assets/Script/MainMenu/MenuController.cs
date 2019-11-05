using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;


public class MenuController : MonoBehaviour {

    MenuController instance;
    void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Awake()
    {
        MakeSingleton();

        DontDestroyOnLoad(panel);
    }


    public GameObject panel;
    GameObject fadeImage;

    void Start () {
        fadeImage = panel.transform.GetChild(0).gameObject;
	}

	IEnumerator FadeScene(string sceneName)
    {
        panel.SetActive(true);
        yield return StartCoroutine(MyCoroutine.MyWaitForSeconds(1));

        SceneManager.LoadScene(sceneName);

        fadeImage.GetComponent<Animator>().SetTrigger("StartAnimation");
        yield return StartCoroutine(MyCoroutine.MyWaitForSeconds(1));
        panel.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ShowRewardedVideo();
        }
	}

    public void ShowRewardedVideo()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show("rewardedVideo", options);
    }

    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");
            // Reward your player here.
        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");

        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
        }
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(FadeScene(scene));
    }
}
