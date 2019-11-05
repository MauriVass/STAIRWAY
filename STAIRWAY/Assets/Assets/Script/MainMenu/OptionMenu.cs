using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour {

    public Text currentPlayerNameText, HighscoreText,currentLanguageText,LabelText;
    string[] language = new string[] { "中文", "English", "Italiano" };
    void Start () {
        HighscoreText.text = GameController.GetHighScore().ToString();

        currentLanguageText.text = language[GameController.instance.GetLaguage()];

        for (int i = 0; i < buttonLanguage.Length; i++)
        {
            buttonLanguage[i].GetComponent<Image>().color = Color.gray;
        }
        buttonLanguage[GameController.instance.GetLaguage()].GetComponent<Image>().color = Color.white;

        //Initial check for the audio
        if (GameController.instance.GetAudio() == 0)
        {
            buttonAudio.GetComponent<Image>().sprite = audioSpriteButton[0];
        }
        else
        {
            buttonAudio.GetComponent<Image>().sprite = audioSpriteButton[1];
        }

        LabelText.GetComponent<Text>().text = "";

        GameController.instance.ChangeLanguage(null);
       
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerNameText.text = GameController.instance.GetPlayerName();
    }

    //
    public Text inputFieldText;
    public static string tmpName;
    public static bool nameAvable;
    public void ChangePlayerName()
    {
        if (ScoreBoard.downloaded)
        {
            tmpName = inputFieldText.text;
            
            if (tmpName != "")
            {
                nameAvable = true;

                //1st Check: if in the new name there are some characters not accepted { "space", "*" }
                char[] notAvaibleCharacter = new char[] { ' ', '*' };
                for (int i = 0; i < tmpName.Length; i++)
                {
                    char tmpChar = tmpName[i];
                    for (int j = 0; j < notAvaibleCharacter.Length; j++)
                    {
                        if (tmpChar==notAvaibleCharacter[j])
                        {
                            nameAvable = false;
                            j = notAvaibleCharacter.Length;
                            i = tmpName.Length;
                        }
                    }
                }

                if (nameAvable)
                {
                    //2nd Check: if the new name is already taken
                    for (int i = 0; i < ScoreBoard.scoreList.Length; i++)
                    {
                        if (ScoreBoard.scoreList[i].username == tmpName)
                        {
                            nameAvable = false;
                        }
                    }
                }
                

                //If the new name is not taken...
                if (nameAvable)
                {
                    //Save on tmp variable and delete the previous data
                    int tmpScore = GameController.GetHighScore();
                    ScoreBoard.instance.DeleteScore(GameController.instance.GetPlayerName());

                    //print(tmpScore);
                    //Save old data as a new name
                    GameController.instance.SetPlayerName(tmpName);
                    GameController.instance.SetHighScore(tmpScore);
                    ScoreBoard.instance.AddNewHighscore(tmpName, tmpScore);
                    currentPlayerNameText.text = GameController.instance.GetPlayerName();
                }
                else
                {
                    print("Name not Avaible");
                }
            }

        }
        else
        {
            print("Connection Error");
        }
    }

    public GameObject buttonAudio;
    public Sprite[] audioSpriteButton;
    public void SetAudio()
    {
        if (GameController.instance.GetAudio()==1)
        {
            //In this case the audio il off(0)
            GameController.instance.SetAudio(0);
            GameController.instance.audioSource.Stop();

            buttonAudio.GetComponent<Image>().sprite = audioSpriteButton[0];
        }
        else
        {
            //In this case the audio il on(1)
            GameController.instance.SetAudio(1);
            GameController.instance.audioSource.Play();

            buttonAudio.GetComponent<Image>().sprite = audioSpriteButton[1];
        }
    }

    public void SetLanguage(GameObject obj)
    {
        string tmp = obj.GetComponent<Text>().text;
        switch (tmp)
        {
            case "中文":                
                GameController.instance.SetLanguage(0);
                break;

            case "English":
                GameController.instance.SetLanguage(1);
                break;

            case "Italiano":
                GameController.instance.SetLanguage(2);
                break;
        }

        GameController.instance.ChangeLanguage(GameController.instance.textObjects);

        obj.GetComponent<Text>().text = "";
    }

    public GameObject[] buttonLanguage;
    public void SetLanguage(string langugae)
    {
        switch (langugae)
        {
            case "中文":
                GameController.instance.SetLanguage(0);
                break;

            case "English":
                GameController.instance.SetLanguage(1);
                break;

            case "Italiano":
                GameController.instance.SetLanguage(2);
                break;
        }

        GameController.instance.ChangeLanguage(GameController.instance.textObjects);

        for (int i = 0; i < buttonLanguage.Length; i++)
        {
            buttonLanguage[i].GetComponent<Image>().color= Color.gray;
        }
        buttonLanguage[GameController.instance.GetLaguage()].GetComponent<Image>().color = Color.white;
    }

}
