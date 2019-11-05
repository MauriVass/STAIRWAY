using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour {
    [SerializeField]
    bool isAcutuallyUsed;

    public static ScoreBoard instance;

    //DB Site: http://dreamlo.com/lb/nVKA34GAKECU3p1oFM7P4A6GQluXX50ke28SW52pYyAw
    const string privateCode = "nVKA34GAKECU3p1oFM7P4A6GQluXX50ke28SW52pYyAw";
    const string publicCode = "5ab0af59012b2e1068a44835";
    const string webURL = "http://dreamlo.com/lb/";

    public static HighScore[] scoreList;
    
    public Text[] playerNameText, playerScoreText;

   
    public Text connectionText;
    public GameObject[] connectionIcon;

    void MakeSingleton()
    {
        if (instance!=null)
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
        //MakeSingleton();
        instance = this;

        //DeleteScore("");
        
    }

    int i;

    //Used by GameController script to check whether the new name selected is already taken
    public static bool downloaded;
    void Start()
    {
        if (isAcutuallyUsed)
        {
            i = 0;
            downloaded = false;
            //if (GameController.instance.connession)
            {
                //print("ScoreBoard time: " + Time.realtimeSinceStartup + " " + ScoreBoard.downloaded);
                DownloadHighscore(true);
                //print("ScoreBoard time: " + Time.realtimeSinceStartup + " " + ScoreBoard.downloaded);
            }
        }

        //AddNewHighscore("ME!", 93);
        //AddNewHighscore("毛利",77);
        //AddNewHighscore("提多补", 38);
        //AddNewHighscore("Andrea", 27);
    }

    void Update()
    {
        if (downloaded&&isAcutuallyUsed)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PrintScore();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                //DownloadHighscore(false);
                downloadFromButton = true;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                string tmp = Random.Range(0, 26).ToString();
                AddNewHighscore(tmp, Random.Range(1000, 10000));
            }

            if (downloadFromButton)
            {
                DownloadHighscore(true);
                downloadFromButton = false;
            }

        }
    }
    
    IEnumerator UploadNewHighscoreInDB(string username, int score)
    {
        WWW www = new WWW(webURL+privateCode+"/add/"+WWW.EscapeURL(username)+"/"+score);//score+"/"+90 add a new line
        yield return  www;

        if (string.IsNullOrEmpty(www.error))
        {
            //print("Upload succesfull");
            //DownloadHighscore(false);
        }
        else
        {
            print("Error uploading " + www.error);
        }
    }
    public void AddNewHighscore(string username,int score)
    {
        StartCoroutine(UploadNewHighscoreInDB(username,score));
    }


    IEnumerator DownloadHighscoreFromDB()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe"); //webURL + publicCode + "/pipe" all the users
        //WWW www = new WWW(webURL + publicCode + "/pipe/0/10");//webURL + publicCode + "/pipe/0/5 print only the first 5 users
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            //print("Downloaded");
            //Formatting the raw text, saving username and score in scoreList[]
            FormatHighScore(www.text);

            downloaded = true;
            PrintScore();

            if (connectionIcon[0] != null)
            {
                connectionIcon[0].SetActive(false);
                connectionIcon[1].SetActive(false);
            }
        }
        else
        {
            print("Error downloading " + www.error);

            downloaded = false;

            if (connectionIcon[0] != null)
            {
                connectionIcon[0].SetActive(true);
                connectionIcon[1].SetActive(true);
            }
        }
    }
    IEnumerator DownloadHighscoreFromDB(string name)
    {
        WWW www = new WWW(webURL + publicCode + "/pipe-get/"+name); //webURL + publicCode + "/pipe/0/5 print only the first 5 users
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighScore(www.text);
            downloaded = true;
        }
        else
        {
            print("Error downloading " + www.error);
            downloaded = false;
        }
    }
    IEnumerator DownloadHighscoreFromDB(bool First10)
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/0/10");//webURL + publicCode + "/pipe/0/5 print only the first 5 users
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighScore(www.text);
            downloaded = true;
        }
        else
        {
            print("Error downloading " + www.error);
            downloaded = false;
        }
    }
    public static void DownloadHighscore(bool downloadAll)
    {
        if (downloadAll)
        {
            instance.StartCoroutine(instance.DownloadHighscoreFromDB());
        }
        else
        {
            instance.StartCoroutine(instance.DownloadHighscoreFromDB(true));
        }
    }
    public void DownloadHighscore(string name)
    {
        StartCoroutine(DownloadHighscoreFromDB(name));
    }
    void FormatHighScore(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        scoreList = new HighScore[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] {'|'});
            scoreList[i] = new HighScore(entryInfo[0], int.Parse(entryInfo[1]));

            //Check if the new name is already taken
            if (scoreList[i].username!=OptionMenu.tmpName)
            {
                OptionMenu.nameAvable = true;
            }
        }
    }

    bool downloadFromButton;
    public void DownloadHighScoreButton()
    {
        downloadFromButton = true;
    }

    IEnumerator UpdateScoreBoard(float time)
    {
        //You can use a bool variable to not update the ScoreBoard's top player name anymore
        while (true)
        {
            yield return new WaitForSeconds(time);
            DownloadHighscore(false);
        }
    }
    void PrintScore()
    {
        //print("Printed");
        int n;
        if (playerNameText.Length<scoreList.Length)
        {
            n = playerNameText.Length;
        }
        else
        {
            n = scoreList.Length;
        }
        for (int i = 0; i < n; i++)
        {
            playerNameText[i].text = scoreList[i].username;
            playerScoreText[i].text = scoreList[i].score.ToString();
        }
    }

    IEnumerator DeleteScoreFromDB(string username)
    {
        WWW www;
        if (username!="")
        {
            www = new WWW(webURL+privateCode+"/delete/"+username);
        }
        else
        {
            www = new WWW(webURL + privateCode + "/clear");
        }
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            print("Deleted");
        }
        else
        {
            print("Error Deleting" + www.error);
        }
    }
    public void DeleteScore(string username)
    {
        StartCoroutine(DeleteScoreFromDB(username));

        //You need at least one Name in the database to assaign a new random name(GameController-->FindNewName())
        AddNewHighscore("Mauri", -2);
    }
  
}

public struct HighScore
{
    public string username;
    public int score;

    public HighScore(string _username,int _score)
    {
        username = _username;
        score = _score;
    }
}
