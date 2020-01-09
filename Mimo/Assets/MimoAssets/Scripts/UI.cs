using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static float scrollSpeed;
    public RawImage img, img2, img3, spike, spike2, bottomSpike, life, p1life, p2life, p1Avatar, p2Avatar, p1Ready, p2Ready, loading;
    private float y, y2, y3, iy, iy2, iy3;
    public Text scoreText, scoreText2, stakeText, statusText, internetText,
        p1Name, p2Name, stakeText2, winningsText, info1, info2, p1GameName, p2GameName, userIDText;
    private Texture2D[] lifeImages = new Texture2D[4];
    public GameObject platform1, platform2, platform3, platform4, p1Profile, p2Profile;
    public GameObject border1, border2, border3, border4;
    public Button restart, menu, ok, back2;
    public bool startingGame, displayingMatchUp, isDraw, oppSpawned;
    public Button single, multi, leaderboard, exit, start, plus, minus, back, cancel, rematch;
    private float spikeY, spike2Y, lifeY, scoreY, platform1Y, platform2Y, platform3Y, platform4Y,
        p1ProfileX, p2ProfileX, p1NameX, p2NameX, stakeY, winningsY, rematchY;
    public GameObject menuPanel, gamePanel, gameOverPanel, UIPanel, UIPanel2, multiMenuPanel, multiPairPanel, tutorialPanel, userNamePanel, lbPanel;
    public Text u1, u2, u3, u4, u5, u6, u7, u8, u9, u10, u11, u12, u13, u14, u15, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, s14, s15;
    public GameObject lv1, lv2, lv3, lv4, lv5, lv6, lv7, lv8, lv9, lv10, lv11, lv12, lv13, lv14, lv15;
    private int[] amounts = { 100,
                              200,
                              500,
                              1000,
                              2000,
                              5000
                                };
    public GameObject opponent, listView;
    public static int index, matchTimer = -1;
    public bool isLoading, hasSetLeaderboard;

    void Start()
    {

        iy = Screen.height / 2;
        iy2 = -Screen.height / 2;
        iy3 = -(Screen.height + Screen.height / 2);

        img.rectTransform.localScale = new Vector3(Screen.width / 98.63f, Screen.height / 100f, 0);
        img2.rectTransform.localScale = new Vector3(Screen.width / 98.63f, Screen.height / 100f, 0);
        img3.rectTransform.localScale = new Vector3(Screen.width / 98.63f, Screen.height / 100f, 0);
        //spike.rectTransform.localScale = new Vector3(Screen.width / 98.63f, Screen.height / 100f, 0);
        bottomSpike.rectTransform.localScale = new Vector3(Screen.height / 100f, Screen.height / 100f, 0);

        border1.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 98.46f, 0);
        border2.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 98.46f, 0);
        border3.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);
        border4.transform.localScale = new Vector3(Screen.height / 1280f, Screen.height / 1280f, 0);

        border1.transform.position = new Vector3(-Screen.width * 0.07f, border1.transform.position.y, 0);
        border2.transform.position = new Vector3(Screen.width + Screen.width * 0.07f, border2.transform.position.y, 0);
        float borX = border3.transform.position.x;
        float off = Screen.height / 48f;
        border3.transform.position = new Vector3(borX, Screen.height - off, 0);
        border4.transform.position = new Vector3(borX, off, 0);

        float x = img.rectTransform.position.x;

        img.rectTransform.position = new Vector3(x, iy, 0);
        img2.rectTransform.position = new Vector3(x, iy2, 0);
        img3.rectTransform.position = new Vector3(x, iy3, 0);


        lifeImages[0] = (Texture2D)Resources.Load("0life");
        lifeImages[1] = (Texture2D)Resources.Load("1life");
        lifeImages[2] = (Texture2D)Resources.Load("2life");
        lifeImages[3] = (Texture2D)Resources.Load("full_life");

        //gamePanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);
        gameOverPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);
        UIPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);
        UIPanel2.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);
        menuPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);
        multiMenuPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);
        multiPairPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);
        userNamePanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);
        tutorialPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);
        lbPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);

        spikeY = spike.rectTransform.position.y;
        spike2Y = bottomSpike.rectTransform.position.y;
        lifeY = life.rectTransform.position.y;
        scoreY = scoreText.rectTransform.position.y;
        platform1Y = platform1.transform.position.y;
        platform2Y = platform2.transform.position.y;
        platform3Y = platform3.transform.position.y;
        platform4Y = platform4.transform.position.y;

        p1ProfileX = p1Profile.transform.position.x;
        p2ProfileX = p2Profile.transform.position.x;
        p1NameX = p1Name.transform.position.x;
        p2NameX = p2Name.transform.position.x;
        stakeY = stakeText2.transform.position.y;
        winningsY = winningsText.transform.position.y;
        rematchY = rematch.transform.position.y;

        setMenu();

        single.onClick.AddListener(() => startSinglePlayer());
        multi.onClick.AddListener(() => multiplayerOptions());
        start.onClick.AddListener(() => startMulti());
        plus.onClick.AddListener(() => increaseBet());
        minus.onClick.AddListener(() => reduceBet());
        back.onClick.AddListener(() => mainMenu());
        back2.onClick.AddListener(() => mainMenu());
        cancel.onClick.AddListener(() => Menu());
        rematch.onClick.AddListener(() => rematchOpponent());
        exit.onClick.AddListener(() => quitGame());
        menu.onClick.AddListener(() => Menu());
        ok.onClick.AddListener(() => setUserID());
        leaderboard.onClick.AddListener(() => displayLeaderBoard());


        Multiplayer.ui = this;

        if (PlayerPrefs.GetString("userName") == null || PlayerPrefs.GetString("userName").Trim().Equals(""))
        {
            Debug.Log("New Player");
            userNamePanel.SetActive(true);
        }
        else
        {
            Debug.Log("Welcome " + PlayerPrefs.GetString("userName"));
            Multiplayer.userName = PlayerPrefs.GetString("userName").ToLower();
        }
    }

    void startSinglePlayer()
    {
        Multiplayer.connection = Multiplayer.Connection.Offline;
        menuPanel.SetActive(false);
        UIPanel2.SetActive(false);
        UIPanel.SetActive(true);
        startingGame = true;
    }
    public void startMultiPlayer()
    {
        multiPairPanel.SetActive(false);
        startingGame = true;
        UIPanel2.SetActive(true);
        UIPanel.SetActive(false);
        p1GameName.text = Multiplayer.userName;
        p2GameName.text = Multiplayer.oppName;
        p1life.texture = lifeImages[GameCode.life];
        p2life.texture = lifeImages[Multiplayer.oppLife];
        if (!oppSpawned)
            Instantiate(opponent, new Vector3((Screen.width / 2), (float)(Screen.height / 1.28), 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Panel").transform);
        oppSpawned = true;
        Multiplayer.ready = false;
        Multiplayer.restarting = false;
        Multiplayer.winnerSet = false;
    }

    void setUserID()
    {
        PlayerPrefs.SetString("userName", userIDText.text);
        userNamePanel.SetActive(false);
    }

    void displayLeaderBoard()
    {
        menuPanel.SetActive(false);
        lbPanel.SetActive(true);
        // GameCode.mp.retreiveLeaderboardData();

    }

    public void setLeaderBoardData()
    {
        hasSetLeaderboard = true;

        lv1.SetActive(false);
        lv2.SetActive(false);
        lv3.SetActive(false);
        lv4.SetActive(false);
        lv5.SetActive(false);
        lv6.SetActive(false);
        lv7.SetActive(false);
        lv8.SetActive(false);
        lv9.SetActive(false);
        lv10.SetActive(false);
        lv11.SetActive(false);
        lv12.SetActive(false);
        lv13.SetActive(false);
        lv14.SetActive(false);
        lv15.SetActive(false);



        if (lbPanel.activeSelf)
        {
            listView.GetComponent<VerticalLayoutGroup>().padding.top = (int)(Screen.height / 3.45f);

            string[] defNames = new string[Multiplayer.lbNames.Count];
            int[] defScores = new int[Multiplayer.lbScores.Count];

            Multiplayer.lbScores.CopyTo(defScores);
            Multiplayer.lbNames.CopyTo(defNames);

            Multiplayer.lbScores.Sort();
            Multiplayer.lbScores.Reverse();

            for (int i = 0; i < Multiplayer.lbScores.Count; i++)
            {
                int score = Multiplayer.lbScores[i];

                for (int j = 0; j < defScores.Length; j++)
                {
                    if (defScores[j] == score)
                    {
                        Multiplayer.lbNames[i] = defNames[j];
                        defScores[j] = 0;
                        j = defScores.Length;
                    }
                }
            }

            if (Multiplayer.lbNames.Count > 14)
            {
                u15.text = Multiplayer.lbNames[14];
                s15.text = Multiplayer.lbScores[14] + "";
                lv15.SetActive(true);
            }
            if (Multiplayer.lbNames.Count > 13)
            {
                u14.text = Multiplayer.lbNames[13];
                s14.text = Multiplayer.lbScores[13] + "";
                lv14.SetActive(true);
            }
            if (Multiplayer.lbNames.Count > 12)
            {
                u13.text = Multiplayer.lbNames[12];
                s13.text = Multiplayer.lbScores[12] + "";
                lv13.SetActive(true);
            }
            if (Multiplayer.lbNames.Count > 11)
            {
                u12.text = Multiplayer.lbNames[11];
                s12.text = Multiplayer.lbScores[11] + "";
                lv12.SetActive(true);
            }
            if (Multiplayer.lbNames.Count > 10)
            {
                u11.text = Multiplayer.lbNames[10];
                s11.text = Multiplayer.lbScores[10] + "";
                lv11.SetActive(true);
            }
            if (Multiplayer.lbNames.Count > 9)
            {
                u10.text = Multiplayer.lbNames[9];
                s10.text = Multiplayer.lbScores[9] + "";
                lv10.SetActive(true);
            }
            if (Multiplayer.lbNames.Count > 8)
            {
                u9.text = Multiplayer.lbNames[8];
                s9.text = Multiplayer.lbScores[8] + "";
                lv9.SetActive(true);
            }
            if (Multiplayer.lbNames.Count > 7)
            {
                u8.text = Multiplayer.lbNames[7];
                s8.text = Multiplayer.lbScores[7] + "";
                lv8.SetActive(true);
            }
            if (Multiplayer.lbNames.Count > 6)
            {
                u7.text = Multiplayer.lbNames[6];
                s7.text = Multiplayer.lbScores[6] + "";
                lv7.SetActive(true);
            }
            if (Multiplayer.lbNames.Count > 5)
            {
                u6.text = Multiplayer.lbNames[5];
                s6.text = Multiplayer.lbScores[5] + "";
                lv6.SetActive(true);
            }
            if (Multiplayer.lbNames.Count > 4)
            {
                u5.text = Multiplayer.lbNames[4];
                s5.text = Multiplayer.lbScores[4] + "";
                lv5.SetActive(true);
            }
            if (Multiplayer.lbNames.Count > 3)
            {
                u4.text = Multiplayer.lbNames[3];
                s4.text = Multiplayer.lbScores[3] + "";
                lv4.SetActive(true);
            }
            if (Multiplayer.lbNames.Count > 2)
            {
                u3.text = Multiplayer.lbNames[2];
                s3.text = Multiplayer.lbScores[2] + "";
                lv3.SetActive(true);
            }
            if (Multiplayer.lbNames.Count > 1)
            {
                u2.text = Multiplayer.lbNames[1];
                s2.text = Multiplayer.lbScores[1] + "";
                lv2.SetActive(true);
            }
            if (Multiplayer.lbNames.Count > 0)
            {
                u1.text = Multiplayer.lbNames[0];
                s1.text = Multiplayer.lbScores[0] + "";
                lv1.SetActive(true);
            }

            Debug.Log(Multiplayer.lbNames.Count + " Names set on Leaderboard");
        }
    }

    void multiplayerOptions()
    {
        menuPanel.SetActive(false);
        multiMenuPanel.SetActive(true);

    }
    void startMulti()
    {
        Debug.Log("Starting Multiplayer");
        setMatchUpDisplay();
        multiMenuPanel.SetActive(false);
        multiPairPanel.SetActive(true);
        setLoading(true);
        Multiplayer.gameID = "mimo";


        //  GameCode.mp.connectToGame();
    }

    void setMatchUpDisplay()
    {
        float off = Screen.height / 1.83f;

        Vector3 offset = new Vector3(0, -off, 0);
        Vector3 offsetL = new Vector3(-off, 0, 0);
        Vector3 offsetR = new Vector3(off, 0, 0);

        p1Profile.transform.position += offsetL;
        p2Profile.transform.position += offsetR;
        p1Name.transform.position += offsetL;
        p2Name.transform.position += offsetR;
        stakeText2.transform.position += offset;
        winningsText.transform.position += offset;
        rematch.transform.position += offset;

    }

    public void showMatchUp()
    {

        Debug.Log("Showing matchup");
        displayingMatchUp = true;
        internetText.text = "";
    }

    void increaseBet()
    {
        if (index < amounts.Length - 1)
            index++;

        Multiplayer.stake = amounts[index];
        stakeText.text = "N " + Multiplayer.stake;
        info1.text = "to win N" + Multiplayer.stake * 2;
        info2.text = "(+10% bet fee = N" + Multiplayer.stake * 1.1 + ")";
    }
    void reduceBet()
    {
        if (index > 0)
            index--;

        Multiplayer.stake = amounts[index];
        stakeText.text = "N " + Multiplayer.stake;
        info1.text = "to win N" + Multiplayer.stake * 2;
        info2.text = "(+10% bet fee = N" + Multiplayer.stake * 1.1 + ")";
    }

    void mainMenu()
    {
        multiMenuPanel.SetActive(false);
        lbPanel.SetActive(false);
        hasSetLeaderboard = false;
        menuPanel.SetActive(true);
        Multiplayer.connection = Multiplayer.Connection.Offline;
    }
    void Menu()
    {
        multiPairPanel.SetActive(false);
        multiMenuPanel.SetActive(false);
        UIPanel2.SetActive(false);

        Multiplayer.connection = Multiplayer.Connection.Offline;

        Multiplayer.state = Multiplayer.State.Searching;
        Multiplayer.ready = false;
        Multiplayer.retreived = false;
        Multiplayer.winner = false;
        Multiplayer.stake = 100;
        Multiplayer.pending = false;
        oppSpawned = false;

        GameCode.running = false;
        GameCode.game.resetElements();


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (Multiplayer.state != Multiplayer.State.Finished)
        {
            // GameCode.mp.cancelPair();
            Debug.Log("Cancelling and deleting database");
        }
        else
            Debug.Log("Cancelling only");

        GameCode.mp.matchID = "";


    }

    private void setMenu()
    {
        float offset = Screen.height / 10;

        spike.rectTransform.position += new Vector3(0, offset, 0);
        spike2.rectTransform.position += new Vector3(0, offset, 0);
        bottomSpike.rectTransform.position += new Vector3(0, -offset, 0);
        life.rectTransform.position += new Vector3(0, offset * 3, 0);
        scoreText.rectTransform.position += new Vector3(0, offset * 3, 0);
        platform1.transform.position += new Vector3(0, -Screen.height, 0);
        platform2.transform.position += new Vector3(0, -Screen.height, 0);
        platform3.transform.position += new Vector3(0, -Screen.height, 0);
        platform4.transform.position += new Vector3(0, -Screen.height, 0);

    }

    public void showWinner()
    {
        if (!Multiplayer.winnerSet)
        {
            if (Multiplayer.oppScore > GameCode.score)
            {
                // GameCode.mp.setWinner(Multiplayer.oppName);
                Multiplayer.winnerSet = true;
                Multiplayer.winner = false;
                Multiplayer.pending = false;
                if (Multiplayer.connection == Multiplayer.Connection.Host)
                    GameCode.setMultiplayerSpawns();

            }
            else if (GameCode.score > Multiplayer.oppScore)
            {
                if (Multiplayer.oppLife <= 0)
                {
                    // GameCode.mp.setWinner(Multiplayer.userName);
                    Multiplayer.winnerSet = true;
                    Multiplayer.winner = true;
                    Multiplayer.pending = false;
                    if (Multiplayer.connection == Multiplayer.Connection.Host)
                        GameCode.setMultiplayerSpawns();

                }
                else
                {
                    if (Multiplayer.oppScore > 0)
                        Multiplayer.pending = true;
                }
            }
            else if (GameCode.score == Multiplayer.oppScore && Multiplayer.oppLife <= 0)
            {
                isDraw = true;
                Multiplayer.winnerSet = true;
                Multiplayer.pending = false;
                if (Multiplayer.connection == Multiplayer.Connection.Host)
                    GameCode.setMultiplayerSpawns();

            }
        }
        if (Multiplayer.pending)
        {
            statusText.text = "OPPONENT STILL IN GAME...";
            stakeText2.text = "AMOUNT TO BE WON : N" + Multiplayer.stake * 2;
        }
        else
        {
            if (isDraw)
            {
                statusText.text = "ITS A DRAW!";
                stakeText2.text = "Nobody won";
            }
            else
            {
                if (Multiplayer.winner)
                    statusText.text = Multiplayer.userName + " WINS!";
                else
                    statusText.text = Multiplayer.oppName + " WINS!";

                stakeText2.text = "AMOUNT WON : N" + Multiplayer.stake * 2;
            }
        }


        winningsText.text = "";

        p1Name.text = Multiplayer.userName + "\n(" + GameCode.score + ")";
        p2Name.text = Multiplayer.oppName + "\n(" + Multiplayer.oppScore + ")";

        //Debug.Log("Displaying Winner");
    }

    private void setLoading(bool isLoading)
    {
        if (isLoading)
            loading.texture = (Texture2D)Resources.Load("progress_bar2");
        else
            loading.texture = (Texture2D)Resources.Load("blank");

        this.isLoading = isLoading;
    }

    void rematchOpponent()
    {
        Debug.Log("Rematching Opponent");
        Multiplayer.state = Multiplayer.State.Playing;
        GameCode.game.resetElements();

        Multiplayer.ready = true;

        FindObjectOfType<PlayerPhysics>().respawn();

    }

    // Update is called once per frame
    void Update()
    {
        if (matchTimer < 0)
        {
            if (Multiplayer.state == Multiplayer.State.Pairing)
            {
                if (Multiplayer.connection == Multiplayer.Connection.Client)
                    statusText.text = "Pairing with Opponent";
                else
                    statusText.text = "Waiting for Opponent";
            }
            if (Multiplayer.state == Multiplayer.State.Waiting)
                statusText.text = "Opponent is Connecting";
            if (Multiplayer.state == Multiplayer.State.Playing)
                statusText.text = "Paired";
        }
        else if (matchTimer < 4)
        {
            statusText.text = "" + matchTimer;
        }
        else
            statusText.text = "Starting match...";


        if (GameCode.running)
        {
            scroll();
            scoreText.text = GameCode.score + "";
            scoreText2.text = GameCode.score + "";
        }
        if (startingGame)
            animateGameAssets();
        if (displayingMatchUp)
            animateMatchUp();

        if (Multiplayer.state == Multiplayer.State.Finished)
        {
            int speed = Screen.height / 42;
            if (rematch.transform.position.y < rematchY)
                rematch.transform.position += new Vector3(0, speed, 0);
        }

        if (multiPairPanel.activeSelf)
        {
            if (Multiplayer.ready)
            {
                p1Ready.texture = (Texture2D)Resources.Load("ready");
                // Debug.Log("Player 1 is Ready");
            }
            else
            {
                p1Ready.texture = (Texture2D)Resources.Load("blank");
            }

            if (Multiplayer.oppReady.Equals("True"))
            {
                p2Ready.texture = (Texture2D)Resources.Load("ready");
                // Debug.Log("Player 2 is Ready");
            }
            else
            {
                p2Ready.texture = (Texture2D)Resources.Load("blank");
            }


        }

    }

    void animateGameAssets()
    {
        int speed = Screen.height / 160;
        int speed2 = Screen.height / 60;

        if (platform1 != null)
        {
            if (spike.rectTransform.position.y > spikeY)
                spike.rectTransform.position += new Vector3(0, -speed, 0);
            if (spike2.rectTransform.position.y > spikeY)
                spike2.rectTransform.position += new Vector3(0, -speed, 0);
            if (bottomSpike.rectTransform.position.y < spike2Y)
                bottomSpike.rectTransform.position += new Vector3(0, speed, 0);
            if (life.rectTransform.position.y > lifeY)
                life.rectTransform.position += new Vector3(0, -speed, 0);
            if (scoreText.rectTransform.position.y > scoreY)
                scoreText.rectTransform.position += new Vector3(0, -speed, 0);

            if (platform1.transform.position.y < platform1Y)
                platform1.transform.position += new Vector3(0, speed2, 0);
            if (platform2.transform.position.y < platform2Y)
                platform2.transform.position += new Vector3(0, speed2, 0);
            if (platform3.transform.position.y < platform3Y)
                platform3.transform.position += new Vector3(0, speed2, 0);
            if (platform4.transform.position.y < platform4Y)
                platform4.transform.position += new Vector3(0, speed2, 0);
            else
            {
                startingGame = false;

                if (Multiplayer.connection != Multiplayer.Connection.Offline)
                    GameCode.running = true;
                else
                    tutorialPanel.SetActive(true);
            }
            // Debug.Log("Setting up game UI for first time");
        }
        else
        {
            if (spike.rectTransform.position.y > spikeY)
                spike.rectTransform.position += new Vector3(0, -speed, 0);
            if (spike2.rectTransform.position.y > spikeY)
                spike2.rectTransform.position += new Vector3(0, -speed, 0);
            if (bottomSpike.rectTransform.position.y < spike2Y)
                bottomSpike.rectTransform.position += new Vector3(0, speed, 0);
            if (life.rectTransform.position.y > lifeY)
                life.rectTransform.position += new Vector3(0, -speed, 0);
            if (scoreText.rectTransform.position.y > scoreY)
                scoreText.rectTransform.position += new Vector3(0, -speed, 0);
            else
            {
                startingGame = false;

                if (Multiplayer.connection != Multiplayer.Connection.Offline)
                    GameCode.running = true;
                else
                    tutorialPanel.SetActive(true);
            }
            Debug.Log("Setting up game UI for concurrent times");
        }
    }

    void animateMatchUp()
    {
        int speed = Screen.height / 42;

        if (p1Profile.transform.position.x < p1ProfileX)
            p1Profile.transform.position += new Vector3(speed, 0, 0);
        if (p2Profile.transform.position.x > p2ProfileX)
            p2Profile.transform.position += new Vector3(-speed, 0, 0);
        if (p1Name.transform.position.x < p1NameX)
            p1Name.transform.position += new Vector3(speed, 0, 0);
        if (p2Name.transform.position.x > p2NameX)
            p2Name.transform.position += new Vector3(-speed, 0, 0);
        if (stakeText2.transform.position.y < stakeY)
            stakeText2.transform.position += new Vector3(0, speed, 0);
        if (winningsText.transform.position.y < winningsY)
        {
            winningsText.transform.position += new Vector3(0, speed, 0);
            setLoading(false);
        }
        else
        {
            displayingMatchUp = false;
            stakeText2.text = "BET : N" + Multiplayer.stake;
            winningsText.text = "TOTAL WINNINGS : N" + Multiplayer.stake * 2;
            p1Name.text = Multiplayer.userName;
            p2Name.text = Multiplayer.oppName;

            Debug.Log("Setting up matchUp");

            if (Multiplayer.state != Multiplayer.State.Finished)
            {
                if (Multiplayer.connection == Multiplayer.Connection.Host)
                {
                    GameCode.setMultiplayerSpawns();
                    Multiplayer.ready = true;
                }
            }

            //Timer t = new Timer();
        }
    }
    private void scroll()
    {
        float x = img.rectTransform.position.x;
        y = img.rectTransform.position.y;
        y2 = img2.rectTransform.position.y;
        y3 = img3.rectTransform.position.y;

        //Debug.Log("Y: " + y + " Y2: " + y2 + " Y3: "+y3);

        img.rectTransform.position = new Vector3(x, y + scrollSpeed, 0);
        img2.rectTransform.position = new Vector3(x, y2 + scrollSpeed, 0);
        img3.rectTransform.position = new Vector3(x, y3 + scrollSpeed, 0);

        // Debug.Log("Y: " + y + " Y2: " + y2);
        y3 = img3.rectTransform.position.y;
        // Debug.Log("Y3: " + y3);

        if (y3 >= Screen.height / 2)
        {
            img.rectTransform.position = new Vector3(x, iy, 0);
            img2.rectTransform.position = new Vector3(x, iy2, 0);
            img3.rectTransform.position = new Vector3(x, iy3, 0);
            // Debug.Log("RESET SCREEN");
        }



    }

    public void updateLife()
    {
        if (GameCode.life < 0)
            GameCode.life = 0;
        life.texture = lifeImages[GameCode.life];

        if (Multiplayer.connection != Multiplayer.Connection.Offline)
        {
            p1life.texture = lifeImages[GameCode.life];
            p2life.texture = lifeImages[Multiplayer.oppLife];
        }
    }

    void quitGame()
    {
        Application.Quit();
    }
}
