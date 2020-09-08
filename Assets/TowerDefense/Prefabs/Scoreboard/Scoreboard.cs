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
    private static Scoreboard instance;
    // TODO: Make some good-looking top5 highscore
    static private Highscores emptyScoreboard = new Highscores { highscoresList = new List<ScoreEntry> { emptyScoreEntry, emptyScoreEntry, emptyScoreEntry, emptyScoreEntry, emptyScoreEntry } };
    static private Highscores defaultScoreboard = new Highscores
    {
        highscoresList = new List<ScoreEntry> {
        new ScoreEntry { score = 2500, username = "Simon" },
        new ScoreEntry { score = 500, username = "David" },
        new ScoreEntry { score = 2000, username = "Cris" },
        new ScoreEntry { score = 1000, username = "Lucas" },
        new ScoreEntry { score = 1500, username = "Jacob" }
    }};
    static private string emptyScoreboardJson = JsonUtility.ToJson(emptyScoreboard);
    static private string defaultScoreboardJson = JsonUtility.ToJson(defaultScoreboard);
    private Highscores highscores;
    private int currentScore = 0;
    private string currentUsername = "Username";

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        scoreboard = Instantiate(scoreboardPrefab);
        StartCoroutine(LateStart(0.1f));
        UpdateCurrentScore();
        UpdateScoreboard();
    }

    public static Scoreboard Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("Missing BuildManager");
            }
            return instance;
        }
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        EndpointManager endpoint = FindObjectOfType<EndpointManager>();
        scoreboard.transform.position = new Vector3(endpoint.transform.position.x + 5.24f, endpoint.transform.position.y + 4.63f, endpoint.transform.position.z + 2.86f);
        scoreboard.transform.Rotate(new Vector3(-180f, -90f, 180f));
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateCurrentScore();
    }

    public void SetUsername(string username)
    {
        currentUsername = username;
        UpdateCurrentScore();
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
        // var x = scoreboard.GetComponentsInChildren<Canvas>();
        Canvas currentGameCanvas = scoreboard.GetComponentsInChildren<Canvas>()[6];
        TextMeshProUGUI currentUsernameLabel = currentGameCanvas.GetComponentsInChildren<TextMeshProUGUI>()[1];
        TextMeshProUGUI currentScoreLabel = currentGameCanvas.GetComponentsInChildren<TextMeshProUGUI>()[3];
        currentUsernameLabel.SetText(currentUsername);
        currentScoreLabel.SetText(currentScore.ToString());
    }

    // Add current score entry at the end of the game
    public void AddScoreEntry()
    {
        ScoreEntry scoreEntry = new ScoreEntry { score = currentScore, username = currentUsername };

        highscores.highscoresList.Add(scoreEntry);

        SaveCurrentScoreboard();

        UpdateScoreboard();
    }

    // Get "scoreboard" values, emptyScoreboardJson for default scoreboard
    private Highscores LoadScoreboardData()
    {       
        string jsonString = PlayerPrefs.GetString("scoreboard", defaultScoreboardJson);
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
        PlayerPrefs.SetString("scoreboard", defaultScoreboardJson);
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
