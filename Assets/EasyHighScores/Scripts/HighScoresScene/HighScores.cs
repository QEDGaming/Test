/**************************************************************************************************
 *  * 
 *  *      QED Gaming PTY. LTD. 
 *  *      50 Goorgool Rd    
 *  *      Bangor            
 *  *      NSW               
 *  *      2234              
 *  *      AUSTRALIA         
 *  *                        
 *  *      
 *  **************************************************************************************************
 *  *      SYSTEM Unity3d - MYAPPS - IOS - UNITY4 - ASSETSTORE - HIGHSCORES
 *  **************************************************************************************************
 *  * 
 *  * Filename     : HighScores.cs
 *  * Country      : All
 *  * Market       : ALL
 *  * Technology   : Unity3D
 *  * Date created : Saturday 18 Jan 2014 -- 17:31:26 
 *  * Author       : P. Smith 
 *  * Purpose      : 
 *  * 
 *  **************************************************************************************************/
using UnityEngine;
using UnityEngine.SocialPlatforms;
#if UNITY_STANDALONE_WIN || UNITY_WP8 || UNITY_WINRT
//using UnityEngine.
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.Security;
#pragma warning disable 162

public struct Highscores_struct
{
  public string UserName;
  public int Score;
  public string ScoreTxt;

  public Highscores_struct(int dummy)
  {
    UserName = "- EMPTY -";
    Score = 0;
    ScoreTxt = string.Format("{0:00000000}",Score);
  }
  public void SetScore(int value)
  {
    Score = value;
    ScoreTxt = string.Format("{0:00000000}",Score);
  }

}
public struct key_struct
{
  public int Index;
  public GameObject game_object;
  public string name_txt;
  public char character;
  public Vector3 Startpos;

  public key_struct(int index,GameObject go,string name,char ch)
  {
    Index = index;
    game_object = go;
    name_txt = name;
    character = ch;
    Startpos = Vector3.zero;
  }
}

public class HighScores : MonoBehaviour
{
  public enum State
  {
    BLANK,
    IDLE,
    SHOWKEYBOARD,
    SHOWINGKEYBOARD,
  };
  public enum Server
  {
    NONE,
    GAMECENTRE,
    OURSERVER,
    MISSING,
  };
  public bool DebugServer = false;
  public Server DefaultUseServer;
  public Server EditorUseServer;
  public Server IOSUseServer;
  public Server OSXUseServer;
  public Server AndroidUseServer;
 
  string secretKey = "123456";
  string PostScoreUrl = "http://qedgaming.com/game_images/Test/PHP/postScore.php?";
  string GetHighscoreUrl = "http://qedgaming.com/game_images/Test/PHP/getHighscore.php";

  private string ServerScoreString = "";

  public GUISkin customskin;
  public Material BackgroundMaterial;
  public float NaturalHorizontalRes = 1024.0f;
  public float NaturalVerticalRes = 768.0f;
  public Camera Camera3d;
  public Rect MenuButtonPos;
  public Rect GameCenterButtonPos;
  public Rect RankButtonPos;
  public Rect NextButtonPos;

  const int MAXKEYS = 40;
  const int MAXNAME = 10;
  const int MAXENTRIES = 100;
  static Highscores_struct[] LocalScores = new Highscores_struct[MAXENTRIES + 1];
  static Highscores_struct Player;

  public GUIStyle namestyle;
  public Vector2 namesize;
  public int numrows;
  public int rowspace;
  public float RowHeight;
  public AudioClip Sfx_keyclick;
  public AudioClip Sfx_keyboardon;
  public AudioClip Sfx_keyboardoff;
  public AudioClip SFX_PressedMenu;
  public float KeyboardAnimSpeed;
  Vector3 KBDownPosition;
  Quaternion KBDownRotation;
  Vector3 KBUpPosition;
  Quaternion KBUpRotation;

  static ILeaderboard Leaderboard;
  static string[] userIDs = new string[MAXENTRIES + 1];
  public State state; /* This is public just so you can tell what state the process is in for debugging */

  public Server ServerWanted = Server.NONE;
  Server ServerFound;
  bool PleaseWait;
  int NumScores = 0;
  GameObject keyboard;
  TextMesh name_textmesh;
  int currentscore;
  
  Vector3 position;
  int highlightidx;
  Rect col1;
  Rect col2;
  Rect col3;
  int page;
  Quaternion StartRot;
  Quaternion TargetRot;
  Vector3 StartPos;
  Vector3 TargetPos;
  float Dir;
  float TimePassed;
  bool UsingMouse;
 

  public key_struct[] keys = { 
    new key_struct(0,null,"Key 0",'0'), 
    new key_struct(1,null,"Key 1",'1'), 
    new key_struct(2,null,"Key 2",'2'), 
    new key_struct(3,null,"Key 3",'3'), 
    new key_struct(4,null,"Key 4",'4'), 
    new key_struct(5,null,"Key 5",'5'), 
    new key_struct(6,null,"Key 6",'6'), 
    new key_struct(7,null,"Key 7",'7'), 
    new key_struct(8,null,"Key 8",'8'), 
    new key_struct(9,null,"Key 9",'9'), 
    new key_struct(10,null,"Key q",'Q'), 
    new key_struct(11,null,"Key w",'W'), 
    new key_struct(12,null,"Key e",'E'), 
    new key_struct(13,null,"Key r",'R'), 
    new key_struct(14,null,"Key t",'T'), 
    new key_struct(15,null,"Key y",'Y'), 
    new key_struct(16,null,"Key u",'U'), 
    new key_struct(17,null,"Key i",'I'), 
    new key_struct(18,null,"Key o",'O'), 
    new key_struct(19,null,"Key p",'P'),
    new key_struct(20,null,"Key a",'A'), 
    new key_struct(21,null,"Key s",'S'), 
    new key_struct(22,null,"Key d",'D'), 
    new key_struct(23,null,"Key f",'F'), 
    new key_struct(24,null,"Key g",'G'), 
    new key_struct(25,null,"Key h",'H'), 
    new key_struct(26,null,"Key j",'J'), 
    new key_struct(27,null,"Key k",'K'), 
    new key_struct(28,null,"Key l",'L'), 
    new key_struct(29,null,"Key z",'Z'),
    new key_struct(30,null,"Key x",'X'), 
    new key_struct(31,null,"Key c",'C'), 
    new key_struct(32,null,"Key v",'V'), 
    new key_struct(33,null,"Key b",'B'), 
    new key_struct(34,null,"Key n",'N'), 
    new key_struct(35,null,"Key m",'M'), 
    new key_struct(36,null,"Key space",' '), 
    new key_struct(36,null,"Key del",' '), 
    new key_struct(36,null,"Key done",' '), 
    new key_struct(99,null,"xxx",'x') };
  /************************************************************************************************
  * NAME   : Awake
  * ACTION : 
  * INPUT  : 
  * OUTPUT : 
  *************************************************************************************************/
  void Awake()
  {
  }

  /************************************************************************************************
  * NAME   : Start
  * ACTION : 
  * INPUT  : 
  * OUTPUT : 
  *************************************************************************************************/
  void Start()
  {
    CreateBackground();
  
    keyboard = GameObject.Find("keyboard");
    GameObject nameobj = GameObject.Find("PlayerName");
    name_textmesh = nameobj.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
    KBDownPosition = new Vector3(0,-1.5f,1);
    KBDownRotation = Quaternion.Euler(0,-180,0);
    KBUpPosition = keyboard.transform.position;
    KBUpRotation = keyboard.transform.rotation;
    keyboard.transform.position = KBDownPosition;                                        /* hide the keyboard */
    keyboard.transform.rotation = KBDownRotation;

    state = State.BLANK;
    currentscore = PlayerPrefs.GetInt("CurrentScore",0);
    D.logx("CurrentScore to Publish: {0}",currentscore);

    Player.UserName = "";
    name_textmesh.text = Player.UserName;
    Player.SetScore(currentscore);
    highlightidx = 0;
    col1 = new Rect(90,180,60,44);
    col2 = new Rect(170,180,500,44);
    col3 = new Rect(690,180,230,44);
    page = 0;
    PleaseWait = true;

    LoadLocalScores();                                               /* default to load local stores from playerprefs */
    //StartCoroutine(PostScoreToServer("Test",100)); //TODO take out Do this in house to init score system

    ServerFound = Server.NONE;
    ServerWanted = DefaultUseServer;                                                     /* default to this server */

#if UNITY_EDITOR
    ServerWanted = EditorUseServer;
#else

#if UNITY_IPHONE
    ServerWanted = IOSUseServer;
#endif
#if UNITY_ANDROID
    ServerWanted = AndroidUseServer;
#endif
#if UNITY_STANDALONE_OSX
    ServerWanted = OSXUseServer;
#endif



#endif
    D.logx("ServerWanted: {0}",ServerWanted);

    switch(ServerWanted)
    {
      case Server.NONE:
        if(currentscore > 0)
          state = State.SHOWKEYBOARD;
        else
          state = State.IDLE;
        PleaseWait = false;
        break;

      case Server.OURSERVER:
        StartCoroutine("GetScoreFromServer");                                            /* Get scores from our server */
        if(currentscore > 0)
          state = State.SHOWKEYBOARD;
        else
          state = State.IDLE;
        break;

      case Server.GAMECENTRE:
        state = State.IDLE;
        Social.localUser.Authenticate(ProcessAuthentication);                            /* get game centre scores and names */
        break;
    }
         
    UsingMouse = false;                                                                  /* set input method to mouse or finger */
#if UNITY_STANDALONE_WIN
    UsingMouse = true;
#endif
#if UNITY_STANDALONE_OSX
    UsingMouse = true;
#endif
#if UNITY_EDITOR
    UsingMouse = true;
#endif
#if UNITY_WEBPLAYER
    UsingMouse = true;
#endif
 
  }
  /************************************************************************************************
  * NAME   : ProcessAuthentication
  * ACTION : This function gets called when Authenticate completes. Note that if the operation is successful,
  *          Social.localUser will contain data from the server
  * INPUT  : bool success, 
  * OUTPUT : 
  *************************************************************************************************/
  void ProcessAuthentication(bool success)
  {
    if(success)
    {
      D.logx("GameCentre Authenticated");
      ServerFound = Server.GAMECENTRE;
      state = State.IDLE;
      DoLeaderboard();
    }
    else
    {
      PleaseWait = false;
      ServerFound = Server.MISSING;
      D.logx("GameCentre Failed to authenticate");
      ServerWanted = Server.OURSERVER;                                 /* Switch to using our server then */
      ServerFound = Server.NONE;
      StartCoroutine("GetScoreFromServer");                            /* Get scores from our server */
      if(currentscore > 0)
        state = State.SHOWKEYBOARD;
      else
        state = State.IDLE;
    }
    if(ServerWanted != Server.NONE)
    {
      D.logx("Server Wanted {0}",ServerWanted);
    }
  }
  /************************************************************************************************
  * NAME   : DoLeaderboard
  * ACTION : Called from DoLeaderboard when game center has been authenticated
  * INPUT  : none
  * OUTPUT : none
  *************************************************************************************************/
  void DoLeaderboard()
  {
    Leaderboard = Social.CreateLeaderboard();
    Leaderboard.id = "TorpedoRun_L7";                                                    /* your game centre leaderboard ID goes here */
    D.logx("Loading Leaderboard {0}",Leaderboard.id);
    Leaderboard.range = new Range(1,100);
    Leaderboard.LoadScores(result => DidLoadScores(result));
  }

  /************************************************************************************************
  * NAME   : DidLoadScores
  * ACTION : Load scores from game center into local scores
  * INPUT  : bool result, 
  * OUTPUT : none
  *************************************************************************************************/
  void DidLoadScores(bool result)
  {
    int i = 0;

    NumScores = Leaderboard.scores.Length;
  
    foreach(IScore score in Leaderboard.scores)
    {
      userIDs[i] = score.userID;
      LocalScores[i].SetScore((int)score.value);
      i++;
      if(i >= MAXENTRIES) /* Saftey check to make sure we don't cause an exception */
        break;
    }

    Social.LoadUsers(userIDs,DidLoadProfiles);
  }
  /************************************************************************************************
  * NAME   : DidLoadProfiles
  * ACTION : 
  * INPUT  : result, 
  * OUTPUT : none
  *************************************************************************************************/
  void DidLoadProfiles(IUserProfile[] result)
  {
    int i = 0;

    foreach(IUserProfile profile in result)
    {
      LocalScores[i].UserName = profile.userName;
      i++;
      if((i >= MAXENTRIES) || (i >= NumScores)) /* Saftey check to make sure we don't cause an exception */
        break;
    }
    PleaseWait = false;
    if(currentscore > 0)
    {
      Player.UserName = Social.localUser.userName;
      AddHighScore(Player);
    }
    SaveLocalScores();   /* save to the local store just incase we ever play without an internet connection */
  }

  /************************************************************************************************
  * NAME   : do_keyhit
  * ACTION : process what key hit on keyboard
  * INPUT  : the raycast hit object
  * OUTPUT : none
  *************************************************************************************************/
  void do_keyhit(RaycastHit hit)
  {
    int i;

    GetComponent<AudioSource>().PlayOneShot(Sfx_keyclick);
    for(i = 0; keys[i].Index != 99; i++)
    {
      if(keys[i].name_txt == hit.transform.name)
      {
        hit.transform.gameObject.BroadcastMessage("HitKey",hit.normal);
        if(hit.transform.name == "Key done")
        {
          GetComponent<AudioSource>().PlayOneShot(Sfx_keyboardoff);
          Dir = 0 - KeyboardAnimSpeed;
          AddHighScore(Player);
          break;
        }

        if(hit.transform.name == "Key del" && (Player.UserName.Length > 0))
        {
          Player.UserName = Player.UserName.Remove(Player.UserName.Length - 1);
          name_textmesh.text = Player.UserName;
          break;
        }

        if(Player.UserName.Length < MAXNAME)
        {
          Player.UserName += keys[i].character;
          name_textmesh.text = Player.UserName;
          break;
        }
      }
    }

  }

  /************************************************************************************************
  * NAME   : Update
  * ACTION : called every frame
  * INPUT  : none
  * OUTPUT : none
  *************************************************************************************************/
  void Update()
  {
    switch(state)
    {
      case State.IDLE:
        break;
      case State.BLANK:
        break;
      case State.SHOWKEYBOARD:
        GetComponent<AudioSource>().PlayOneShot(Sfx_keyboardon);
        TargetRot = KBUpRotation;
        TargetPos = KBUpPosition;
        StartRot = KBDownRotation;
        StartPos = KBDownPosition;
        TimePassed = 0;
        Dir = KeyboardAnimSpeed;
        state = State.SHOWINGKEYBOARD;
        break;

      case State.SHOWINGKEYBOARD:
        TimePassed += Dir * Time.deltaTime;
        TimePassed = Mathf.Clamp01(TimePassed);
        keyboard.transform.rotation = Quaternion.Lerp(StartRot,TargetRot,TimePassed);
        keyboard.transform.position = Vector3.Lerp(StartPos,TargetPos,TimePassed);
        if((Dir < 0) && TimePassed == 0)
        {
          state = State.IDLE;
        }

        if(UsingMouse)
        {
          if(Input.GetMouseButtonDown(0))
          {
            Vector2 pos;
            pos.x = Input.mousePosition.x;
            pos.y = Input.mousePosition.y;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit;

            if(Physics.Raycast(ray,out hit,50))
            {
              do_keyhit(hit);
            }

          }
        }
        else
        {
          if(Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Began))
          {
            Vector2 pos;
            pos.x = Input.touches[0].position.x;
            pos.y = Input.touches[0].position.y;
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit;

            if(Physics.Raycast(ray,out hit,50))
            {
              do_keyhit(hit);
            }
          }
        }
        break;
    };


  }

  /************************************************************************************************
  * NAME   : OnGUI
  * ACTION : called every frame to overlay GUI objects
  * INPUT  : none
  * OUTPUT : none
  *************************************************************************************************/
  void OnGUI()
  {
    GUI.skin = customskin;

    GUI.matrix = Matrix4x4.TRS(new Vector3(0,0,0),Quaternion.identity,new Vector3(Screen.width / NaturalHorizontalRes,Screen.height / NaturalVerticalRes,1));


    if(GUI.Button(MenuButtonPos,"MENU"))
    {
      StartCoroutine(LoadScene("TestScene",1.0f));                                       /* add your return scene here */
    }
 
    if(state == State.IDLE)
    {
      if(ServerFound == Server.GAMECENTRE)
      {
        if(GUI.Button(GameCenterButtonPos,"Center"))
        {
          Social.ShowLeaderboardUI();
        }
        if(GUI.Button(RankButtonPos,"Rank"))
        {
          Social.ShowAchievementsUI();
        }
      }
      
      int i;
      Rect rc = col1;
      int idx = page * 10;

      for(i = 0; i < numrows; i++,idx++)
      {
        if(idx >= MAXENTRIES)
          break;
        rc = col1;
        rc.y += i * RowHeight;
        if(idx >= 99)
          rc.width += 15;
        if((idx == highlightidx) && !PleaseWait)
          GUI.Label(rc,(idx + 1).ToString(),customskin.GetStyle("Highlight"));
        else
          GUI.Label(rc,(idx + 1).ToString(),customskin.GetStyle("HighScoreBox"));


        rc = col2;
        rc.y += i * RowHeight;
        if(PleaseWait && (i == 0))
          GUI.Label(rc,"Loading, Please Wait...",customskin.GetStyle("HighlightLoading"));
        else
        {
          if(idx == highlightidx)
            GUI.Label(rc,LocalScores[idx].UserName,customskin.GetStyle("Highlight"));
          else
            GUI.Label(rc,LocalScores[idx].UserName,customskin.GetStyle("HighScoreBox"));
        }
        rc = col3;
        rc.y += i * RowHeight;

        if((idx == highlightidx) && !PleaseWait)
          GUI.Label(rc,LocalScores[idx].ScoreTxt,customskin.GetStyle("Highlight"));
        else
          GUI.Label(rc,LocalScores[idx].ScoreTxt,customskin.GetStyle("HighScoreBox"));
      }

      if(GUI.Button(NextButtonPos,">>"))
      {
        if(page < 9)
          page++;
        else
          page = 0;
      }

    }
  }
  /************************************************************************************************
  * NAME   : LoadScene
  * ACTION : load scene with delay to allow you to hear the sound effect
  * INPUT  : scene name
  * OUTPUT : none
  *************************************************************************************************/
  IEnumerator LoadScene(string scene,float delay)
  {
    GetComponent<AudioSource>().PlayOneShot(SFX_PressedMenu);
    yield return new WaitForSeconds(delay);
    Application.LoadLevel(scene);
  }

  /************************************************************************************************
  * NAME   : LoadLocalScores
  * ACTION : load local scores stored in playerprefs and initialises playerprefs if not yet done
  * INPUT  : none
  * OUTPUT : none
  *************************************************************************************************/
  void LoadLocalScores()
  {
    int i;

    for(i = 0; i < MAXENTRIES; i++)
    {
      if(PlayerPrefs.HasKey(i + "HScore"))
      {
        LocalScores[i].SetScore(PlayerPrefs.GetInt(i + "HScore"));
        LocalScores[i].UserName = PlayerPrefs.GetString(i + "HScoreName");
      }
      else
      {
        PlayerPrefs.SetInt(i + "HScore",0);
        PlayerPrefs.SetString(i + "HScoreName","- Empty -");
        LocalScores[i].SetScore(PlayerPrefs.GetInt(i + "HScore"));
        LocalScores[i].UserName = PlayerPrefs.GetString(i + "HScoreName");
      }
    }
  }
  /************************************************************************************************
  * NAME   : SaveLocalScores
  * ACTION : Save local scores in playerprefs
  * INPUT  : none
  * OUTPUT : none
  *************************************************************************************************/
  void SaveLocalScores()
  {
    int i;

    for(i = 0; i < MAXENTRIES; i++)
    {
      PlayerPrefs.SetInt(i + "HScore",LocalScores[i].Score);
      PlayerPrefs.SetString(i + "HScoreName",LocalScores[i].UserName);
    }
  }

  /************************************************************************************************
  * NAME   : AddHighScore
  * ACTION : add a player and its score to the local highscore table. Sends to Gamecenter if available
  * INPUT  : player struct with name and score 
  * OUTPUT : none
  *************************************************************************************************/
  public void AddHighScore(Highscores_struct player)
  {
    int i;
    int x;

    int PrevHighest = PlayerPrefs.GetInt("Highest");
    if(player.Score > PrevHighest)
      PlayerPrefs.SetInt("Highest",player.Score);

    switch(ServerFound)
    {
      case Server.NONE:
        break;

      case Server.OURSERVER:
       StartCoroutine(PostScoreToServer(player.UserName,player.Score));
       break;

      case Server.GAMECENTRE:
        Social.ReportScore((long)player.Score,Leaderboard.id,success => { Debug.Log(success ? "Reported score successfully" : "Failed to report score"); });
        break;
    }
     
    for(i = 0; i < MAXENTRIES; i++)
    {
      if(player.Score >= LocalScores[i].Score)                                           /* new score is higher than the stored score */
      {
        for(x = (MAXENTRIES - 1); x > i; x--)                                            /* ripple move the rest down the list */
          LocalScores[x] = LocalScores[x - 1];

        LocalScores[i] = player;
        highlightidx = i;
        page = (highlightidx / 10);
        break;
      }
    }
    SaveLocalScores();                                                                   /* and save them */
  }
  /************************************************************************************************
  * NAME   : GetScoreFromServer
  * ACTION : get scores from our own QED server
  * INPUT  : 
  * OUTPUT : 
  *************************************************************************************************/
  IEnumerator GetScoreFromServer()
  {
    ServerScoreString = "";

    PleaseWait = true;

    WWWForm form = new WWWForm();
    form.AddField("limit",100);

    WWW www = new WWW(GetHighscoreUrl,form);
    yield return www;

    if(www.text == "")
    {
      D.logx("There was an error getting the high score: {0}",www.error);
      ServerFound = Server.MISSING;
    }
    else
    {
      ServerFound = Server.OURSERVER;
      PleaseWait = false;
      ServerScoreString = www.text;
      if(DebugServer)
        D.logx("from server: {0}",ServerScoreString);

      string[] data;

      if(ServerScoreString.Length > 0)
      {
        data = Regex.Split(ServerScoreString,@"\t|\n");
        int len = data.Length;
        if((len & 0x01) > 0)                                      /* if odd length */
          len--;
        int f = 0;
        int t = 0;

        do
        {
          LocalScores[t].UserName = data[f];
          LocalScores[t].SetScore(int.Parse(data[f + 1]));
          if(LocalScores[t].UserName.Length <= 0)
            LocalScores[t].UserName = "Unknown";
          f += 2;
          t++;
          if(t >= MAXENTRIES) /* Saftey check to make sure we don't cause an exception */
            break;
          if(f >= len)
            break;
        } while(true);

        if(t < MAXENTRIES)
        {
          do
          {
            LocalScores[t].UserName = "- Empty -";
            LocalScores[t].SetScore(0);
            t++;
            if(t >= MAXENTRIES) /* Saftey check to make sure we don't cause an exception */
              break;

          } while(true);
        }

      }
    }
    SaveLocalScores();                                               /* and save them */
  }

  /************************************************************************************************
  * NAME   : PostScoreToServer
  * ACTION : post score to our own QED server
  * INPUT  : string name, int score, 
  * OUTPUT : 
  *************************************************************************************************/
  IEnumerator PostScoreToServer(string name,int score)
  {
    string _name = name;
    int _score = score;

    string hash = CreateHash(_name + _score + secretKey).ToLower();

    WWWForm form = new WWWForm();
    form.AddField("name",_name);
    form.AddField("score",_score);
    form.AddField("hash",hash);

    WWW www = new WWW(PostScoreUrl,form);
    PleaseWait = true;
    yield return www;

    if(www.text == "done")
    {
      StartCoroutine("GetScoreFromServer");
    }
    else
    {
      D.logx("There was an error posting the high score: {0}",www.error);
    }
  }
  /************************************************************************************************
  * NAME   : CreateHash
  * ACTION : 
  * INPUT  : string input, 
  * OUTPUT : string 
  *************************************************************************************************/
  public string CreateHash(string input)
  {
#if UNITY_STANDALONE_OSX || UNITY_ANDROID || UNITY_IPHONE || UNITY_WEBPLAYER
    System.Security.Cryptography.MD5 enc = System.Security.Cryptography.MD5.Create();
    byte[] Bytes = System.Text.Encoding.UTF8.GetBytes(input);
    byte[] hash = enc.ComputeHash(Bytes);
#elif UNITY_STANDALONE_WIN || UNITY_WP8 || UNITY_WINRT
    System.Security.Cryptography.MD5 enc = System.Security.Cryptography.MD5.Create();
    byte[] Bytes = System.Text.Encoding.UTF8.GetBytes(input);
    byte[] hash = enc.ComputeHash(Bytes);
#endif
    StringBuilder sb = new StringBuilder();
    for(int i = 0; i < hash.Length; i++)
    {
      sb.Append(hash[i].ToString("X2"));
    }
    return sb.ToString();
  }

  /************************************************************************************************
 * NAME   : CreateBackground
 * ACTION : Create a resoloution independent plane to fully fit the screen as a background image
 * INPUT  : none
 * OUTPUT : none
 *************************************************************************************************/
  void CreateBackground()
  {
    GameObject go = new GameObject("Background");
    go.transform.localPosition = Vector3.zero;
    go.transform.localRotation = Quaternion.identity;
    go.transform.parent = transform;
    go.AddComponent<MeshFilter>();
    go.AddComponent<MeshRenderer>();
    Renderer go_Renderer = go.GetComponent<Renderer>();
    Mesh go_Mesh = go.GetComponent<MeshFilter>().mesh;
    go_Mesh.Clear();
    go_Mesh.vertices = new Vector3[] 
    {
      Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)),
      Camera.main.ScreenToWorldPoint(new Vector3(0, Camera.main.pixelHeight, 10)),
      Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 10)),
      Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, 10))
    };

    go_Mesh.uv = new Vector2[] 
    {
      new Vector2(0, 0), new Vector2(0, 1),
      new Vector2(1, 1), new Vector2(1, 0)
    };

    go_Mesh.triangles = new int[] { 0,1,2,0,2,3 };
    go_Renderer.material = BackgroundMaterial;

  }

}
