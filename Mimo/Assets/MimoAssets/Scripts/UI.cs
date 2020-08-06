using System;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static float scrollSpeed;
    public RawImage img, img2, img3, spike, spike2, bottomSpike, life, p1life, p2life, p1Avatar, p2Avatar, p1Ready, p2Ready, loading;
    private float y, y2, y3, iy, iy2, iy3;
    public Text scoreText, scoreText2, stakeText, statusText, internetText, confirmText,
        p1Name, p2Name, stakeText2, winningsText, info1, info2, p1GameName, p2GameName, userIDText;
    private Texture2D[] lifeImages = new Texture2D[4];
    public GameObject platform1, platform2, platform3, platform4, p1Profile, p2Profile, listViewChild, leaderboard;
    public GameObject border1, border2, border3, border4;
    public Button restart, menu, ok;
    public bool startingGame, displayingMatchUp, isDraw, oppSpawned;
    public static bool doneLoading;
    public Button single, multi, leaderboardButton, exit, start, plus, minus, cancel, rematch;
    public Button newRoyal;
    private float spikeY, spike2Y, lifeY, scoreY, platform1Y, platform2Y, platform3Y, platform4Y,
        p1ProfileX, p2ProfileX, p1NameX, p2NameX, stakeY, winningsY, rematchY;
    public GameObject confirmPanel, loaderPanel, menuPanel, gameOverPanel, UIPanel, UIPanel2, multiMenuPanel, OneVOnePanel, RoyalPanel, LeaguePanel, multiPairPanel, tutorialPanel, lbPanel;
    private int[] amounts = { 100,
                              200,
                              500,
                              1000,
                              2000,
                              5000,
                              10000,
                              20000,
                              50000
                                };
    public GameObject opponent, listView;
    public static int index, matchTimer = -1;
    public bool isLoading;

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

        gameOverPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);
        UIPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);
        UIPanel2.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1280f, 0);
        menuPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);
        multiMenuPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);
        multiPairPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);
        tutorialPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);
        lbPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);
        OneVOnePanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);
        RoyalPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);
        LeaguePanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);
        loaderPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);
        //confirmPanel.transform.localScale = new Vector3(Screen.width / 720f, Screen.width / 720f, 0);

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
        cancel.onClick.AddListener(() => Menu());
        rematch.onClick.AddListener(() => rematchOpponent());
        exit.onClick.AddListener(() => quitGame());
        menu.onClick.AddListener(() => Menu());
        leaderboardButton.onClick.AddListener(() => displayLeaderBoard());


        Multiplayer.ui = this;

#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) ||
            !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
#endif

    }

    void startSinglePlayer()
    {
        Multiplayer.type = Multiplayer.GameType.Single;
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

    public void startRoyalRumbleMatch(){

        Debug.Log("STARTING ROYAL RUMBLE");
        Multiplayer.type = Multiplayer.GameType.Royal;
        Multiplayer.connection = Multiplayer.Connection.Client;
        menuPanel.SetActive(false);
        UIPanel2.SetActive(false);
        UIPanel.SetActive(true);
        confirmPanel.SetActive(false);
        RoyalPanel.SetActive(false);
        Multiplayer.spawnIndex = 0;
        startingGame = true;
    }

    void displayLeaderBoard()
    {
        menuPanel.SetActive(false);
        lbPanel.SetActive(true);
        FindObjectOfType<GameCode>().resetLeaderboard();

        doneLoading = false;
        loaderPanel.SetActive(true);
        
        GameCode.mp.retreiveLeaderboardData();
    }

    public void setLeaderBoardData()
    {

        listView.GetComponent<VerticalLayoutGroup>().padding.left = (int)(Screen.width / 2.12f);

        if (lbPanel.activeSelf && FindObjectOfType<GameCode>().leaderboardItems.Count == 0)
        {
            //hasSetLeaderboard = true;
            Debug.Log("Setting Leaderboard data");
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

            for (int i = 0; i < Multiplayer.lbNames.Count; i++)
            {
                Instantiate(listViewChild.gameObject, GameObject.FindGameObjectWithTag("ListView").transform);
            }
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
        OneVOnePanel.SetActive(false);
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

    public static string getNaira(double amount)
    {
        string number = amount.ToString();
        string result = number;

        if (number.Length > 3)
        {
            if (number.Length > 6)
                result = number.Insert(number.Length - 6, ",");

            result = result.Insert(result.Length - 3, ",");
        }
            
            return "N"+result;
    }

    void increaseBet()
    {
        if (index < amounts.Length - 1)
            index++;

        Multiplayer.stake = amounts[index];
        stakeText.text = getNaira(Multiplayer.stake);
        info1.text = "to win " + getNaira(Multiplayer.stake * 2);
        info2.text = "(+10% bet fee = " + getNaira(Multiplayer.stake * 1.1) + ")";
    }
    void reduceBet()
    {
        if (index > 0)
            index--;

        Multiplayer.stake = amounts[index];
        stakeText.text = getNaira(Multiplayer.stake);
        info1.text = "to win " + getNaira(Multiplayer.stake * 2);
        info2.text = "(+10% bet fee = " + getNaira(Multiplayer.stake * 1.1) + ")";
    }

    public void shareScore()
    {

        string screenshotName = "mimo_highscore.png";
        // wait for graphics to render
        new WaitForEndOfFrame();
        string screenShotPath = Application.persistentDataPath + "/" + screenshotName;
        ScreenCapture.CaptureScreenshot(screenshotName, 1);
        new WaitForSeconds(0.5f);

        string appPackageName = Application.identifier;

        var shareSubject = "Fam! I challenge you to beat my high score in" +
                    " Mimo";
        var shareMessage = "Fam! I challenge you to beat my high score in" +
                    " Mimo" +
                    "\nDownload Mimo from the link below." +
                     "\n\n" +
                    "https://play.google.com/store/apps/details?id=" + appPackageName;

        NativeShare shareIntent = new NativeShare();
        shareIntent.AddFile(screenShotPath, null);
        shareIntent.SetSubject(shareSubject);
        shareIntent.SetText(shareMessage);
        shareIntent.SetTitle("Share your score with friends...");

        shareIntent.Share();

    }

    public void mainMenu()
    {
        multiMenuPanel.SetActive(false);
        lbPanel.SetActive(false);
        menuPanel.SetActive(true);
        Multiplayer.connection = Multiplayer.Connection.Offline;
        Multiplayer.type = Multiplayer.GameType.Single;
    }
    public void multiMenu()
    {
        
        OneVOnePanel.SetActive(false);
        RoyalPanel.SetActive(false);
        LeaguePanel.SetActive(false);
        multiMenuPanel.SetActive(true);
        Debug.Log("Back to MultiMenu");
    }

    public void oneVOneMenu()
    {       
        if (Application.platform == RuntimePlatform.Android)
        {
            CustomAndroidToast("One vs One is coming soon");
        }
        else { }
        //commented out for now as this feature is not implemented yet.
        /**
         * multiMenuPanel.SetActive(false);
         * OneVOnePanel.SetActive(true);
         **/
    }
    public void royalMenu()
    {
        multiMenuPanel.SetActive(false);
        RoyalPanel.SetActive(true);
        FindObjectOfType<RoyalRumbleScript>().Initialize();
    }
    public void leagueMenu()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            CustomAndroidToast("League is coming soon");
        }
        else{}
        //commented out for now as this feature is not implemented yet.
        /**
         * multiMenuPanel.SetActive(false);
         * LeaguePanel.SetActive(true);
         **/
    }

    void Menu()
    {
        multiPairPanel.SetActive(false);
        multiMenuPanel.SetActive(false);
        UIPanel2.SetActive(false);

        Multiplayer.connection = Multiplayer.Connection.Offline;
        Multiplayer.type = Multiplayer.GameType.Single;

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
        if(doneLoading && loaderPanel.activeSelf)
            loaderPanel.SetActive(false);
        
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
        if (lbPanel.activeSelf)
        {
            RectTransform rectTransform = leaderboard.GetComponent<RectTransform>();
            float newTop = ((Screen.height - 1280f) / 2) + 145;
            float newBottom = ((Screen.height - 1280f) / 2) + 99;
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, newBottom);
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -newTop);
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

    public static void CustomAndroidToast(string message) {
        AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject javaClass = new AndroidJavaClass("com.gambeat.mimo.paystack.paystack.AndroidBridge");
        if (javaClass != null)
        {
            javaClass.CallStatic("customToast", activity, message);
        }
    } 
}
