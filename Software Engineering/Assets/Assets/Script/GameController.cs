using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;
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
        MakeSingleton();
    }

    [SerializeField]
    public AudioSource audioSource;
    public AudioClip[] audioClip;
    public AudioClip currentAudioClip;

    public GameObject[] textObjects;
    public Font font;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip[0];

        if (GetAudio() == 0)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.Play();
        }

        //DeletePlayerData(); 

        IsPlayerNew();
        
        //ChangeLanguage(textObjects);


        //print("PlayerName: "+GetPlayerName()+" Score: "+GetHighScore());
    }

    [Range(0,1)]
    public float volume;
    bool deactiveAudio=false,activeAudio=false;
    void Update()
    {
        if (ScoreBoard.downloaded)
        {
            IsPlayerNew();
        }
        currentAudioClip = audioSource.clip;

        if (deactiveAudio)
        {
            audioSource.volume -= Time.deltaTime/2;
            if (audioSource.volume<=0.6f)
            {
                deactiveAudio = false;
                audioSource.volume = 0.6f;
            }
        }
        if (activeAudio)
        {
            audioSource.volume += Time.deltaTime;
            if (audioSource.volume >  0.9f)
            {
                activeAudio = false;
                audioSource.volume = 1f;
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            audioSource.clip = audioClip[1];
            audioSource.Play();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            //textObjects = GameObject.FindGameObjectsWithTag("Text");
        }
        
    }

    float tmp,n=0;
    public void ChangeAudio(AudioClip audioClip)
    {
        /*audioSource.clip = audioClip;
        audioSource.Play();*/
        StartCoroutine(Change_Audio(false,audioClip));
    }
    public void ChangeAudio(bool off)
    {
        if (off)
        {
            deactiveAudio = true;
        }
        if (!off)
        {
            //audioSource.volume = 0.5f;
            activeAudio = true;
        }
    }
    IEnumerator Change_Audio(bool off, AudioClip audioClip)
    {
        if (off)
        {
            float tmp = audioSource.volume;
            tmp -= Time.deltaTime;
            audioSource.volume = tmp;
            yield return StartCoroutine(MyCoroutine.MyWaitForSeconds(.11f));

        }
        else
        {
            audioSource.volume = 1;
            //ChangeAudio(false);
            audioSource.clip = audioClip;
            audioSource.Play();

            yield return StartCoroutine(MyCoroutine.MyWaitForSeconds(.1f));
            /*tmp = audioSource.volume;
            audioSource.volume = tmp;
            tmp += Time.deltaTime;*/
            ChangeAudio(false);
        }

    }

    void IsPlayerNew()
    {
        //If the player is new the system will give him a random(successive) name.
        //Check if there is an internet connection
        if (!PlayerPrefs.HasKey("IsPlayerNew"))
        {
            print("GameController time: " + Time.realtimeSinceStartup+" "+ScoreBoard.downloaded);
            
            if (ScoreBoard.downloaded)
            {
                PlayerPrefs.SetInt("IsPlayerNew", 10);
                string tmpName = FindPlayerName();
                print(tmpName);
                SetPlayerName(tmpName);

                print("Connection ok: " + GetPlayerName());
            }
            else//There is any internet connection
            {
                if (!PlayerPrefs.HasKey("IsPlayerNew"))
                {
                    PlayerPrefs.SetInt("IsPlayerNew", 10);
                    SetPlayerName("Player" + Random.Range(100, 999));
                }
                print("Connection no: " + GetPlayerName());
            }
            SetHighScore(0);
            SetAudio(1);
            SetLanguage(1);
        }
    }
    string FindPlayerName()
    {
        int i;
        string tmpName1,tmpName2="";
        bool found;

        for (i = 0; i < ScoreBoard.scoreList.Length; i++)
        {
            found = false;
            if (i<9)
            {
                tmpName1 = "Player0" + i;
            }
            else
            {
                tmpName1 = "Player" + i;
            }
            string tmp = ScoreBoard.scoreList[i].username;
            print(i+tmp);
            if (ScoreBoard.scoreList[i].username == tmpName1)
            {
                found = true;
            }

            if (found == false)
                tmpName2 = tmpName1;
        }

        if (tmpName2=="Player00")
        {
            tmpName2 = "Player01";
        }
        return tmpName2;
    }

    public string PlayerName = "PlayerName";
    public string Highscore = "Highscore";
    public string Audio = "Audio";
    public string Language = "Language";

    public void SetPlayerName(string playerName)
    {
        PlayerPrefs.SetString(PlayerName, playerName);
    }
    public string GetPlayerName()
    {
        return PlayerPrefs.GetString(PlayerName);
    }

    public void SetHighScore(int newScore)
    {
        PlayerPrefs.SetInt(Highscore, newScore);
    }
    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt(instance.Highscore);
    }

    public void SetAudio(int audio)
    {
        PlayerPrefs.SetInt(Audio, audio);
    }
    public int GetAudio()
    {
        return PlayerPrefs.GetInt(Audio);
    }

    /*
     * 0 --> Chinese
     * 1 --> English
     * 2 --> Italian
     */
    public void SetLanguage(int language)
    {
        PlayerPrefs.SetInt(Language, language);
    }
    public int GetLaguage()
    {
        return PlayerPrefs.GetInt(Language);
    }
    public void ChangeLanguage(GameObject[] textObj)
    {
        int tmp = GetLaguage();
        textObj = GameObject.FindGameObjectsWithTag("Text");

        for (int i = 0; i < textObj.Length; i++)
        {
            textObj[i].GetComponent<Text>().text = textObj[i].GetComponent<ChangeText>().languageText[tmp];
            textObj[i].GetComponent<Text>().font = font;
        }
    }


    public void DeletePlayerData()
    {
        print("Player data deleted");
        PlayerPrefs.DeleteAll();
    }
}
