using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{

    static private ScoreEntry emptyScoreEntry = new ScoreEntry { score = 0, username = "Username" };
    [SerializeField]
    private GameObject scoreboardPrefab;
    private GameObject scoreboard;
    // TODO: Make some good-looking top5 highscore
    static private Highscores emptyScoreboard = new Highscores { highscoresList = new List<ScoreEntry> { emptyScoreEntry, emptyScoreEntry, emptyScoreEntry, emptyScoreEntry, emptyScoreEntry } };
    static private string emptyScoreboardJson = JsonUtility.ToJson(emptyScoreboard);
    private Highscores highscores;
    private int currentScore;
    private string currentUsername;

    void Start()
    {
        // TODO: Get username from keyboard + reseting scoretable
        scoreboard = Instantiate(scoreboardPrefab);
        currentScore = 0;
        currentUsername = "Dawid";
        SetScoreboardOnScene();
        UpdateCurrentScore();
        UpdateScoreboard();
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateCurrentScore();
    }

    void SetScoreboardOnScene()
    {
        EndpointManager endpoint = FindObjectOfType<EndpointManager>();
        scoreboard.transform.position = new Vector3(endpoint.transform.position.x -0.2f, endpoint.transform.position.y + 4.97f, endpoint.transform.position.z + 5.58f);
        scoreboard.transform.Rotate(new Vector3(90f, 0f, 180f));
    }

    void SetPlayerInfo()
    {
        // Przypisanie currentUsername
    }

    private void SetScoreOnPlace(int place, ScoreEntry score)
    {
        Canvas scoreCanvas = scoreboard.GetComponentsInChildren<Canvas>()[place];
        TextMeshProUGUI scoreLabel = scoreCanvas.GetComponentsInChildren<TextMeshProUGUI>()[1];
        TextMeshProUGUI usernameLabel = scoreCanvas.GetComponentsInChildren<TextMeshProUGUI>()[2];
        scoreLabel.SetText(score.score.ToString());
        usernameLabel.SetText(score.username);
    }

    public void UpdateCurrentScore()
    {
        // var x = GetComponentsInChildren<Canvas>();
        Canvas currentGameCanvas = scoreboard.GetComponentsInChildren<Canvas>()[6];
        TextMeshProUGUI currentUsernameLabel = currentGameCanvas.GetComponentsInChildren<TextMeshProUGUI>()[1];
        TextMeshProUGUI currentScoreLabel = currentGameCanvas.GetComponentsInChildren<TextMeshProUGUI>()[3];
        currentUsernameLabel.SetText(currentUsername);
        currentScoreLabel.SetText(currentScore.ToString());
    }

    // TODO - na koniec gierki dodajemy nasz wynik do scoreboarda
    private void AddScoreEntry(int score, string username)
    {
        ScoreEntry scoreEntry = new ScoreEntry { score = score, username = username };

        highscores.highscoresList.Add(scoreEntry);

        SaveCurrentScoreboard();

        UpdateScoreboard();
    }

    private Highscores LoadScoreboardData()
    {
        // Get "scoreboard" values, emptyScoreboardJson for default scoreboard
        string jsonString = PlayerPrefs.GetString("scoreboard", emptyScoreboardJson);
        return JsonUtility.FromJson<Highscores>(jsonString);
    }

    private void SaveCurrentScoreboard()
    {
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("scoreboard", json);
        PlayerPrefs.Save();
    }

    public void UpdateScoreboard()
    {
        highscores = LoadScoreboardData();
        highscores.GetTop5Scores();
        for(int i = 0; i < highscores.highscoresList.Count; i++)
        {
            int place = i + 1;
            ScoreEntry score = highscores.highscoresList[i];
            SetScoreOnPlace(place, score);
        }
    }

    public void ResetScoreboard()
    {
        PlayerPrefs.SetString("scoreboard", emptyScoreboardJson);
        PlayerPrefs.Save();
        UpdateScoreboard();
    }
}

[System.Serializable]
public class ScoreEntry
{
    public int score;
    public string username;
}

public class Highscores
{
    public List<ScoreEntry> highscoresList;

    public void GetTop5Scores()
    {
        List<ScoreEntry> result = new List<ScoreEntry>();

        highscoresList.Sort(delegate (ScoreEntry score1, ScoreEntry score2) { return score2.score.CompareTo(score1.score); });

        for(int i = 0; i < 5; i++)
        {
            result.Add(highscoresList[i]);
        }

        highscoresList = result;
    }
}
