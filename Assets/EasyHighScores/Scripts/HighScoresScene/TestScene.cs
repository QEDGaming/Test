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
 *  *      SYSTEM QVIDS - APPLE - MYAPPS - ASSETSTORE - HIGHSCORES - ASSETS
 *  **************************************************************************************************
 *  * 
 *  * Filename     : TestScene.cs
 *  * Country      : AU
 *  * Market       : ALL AU
 *  * Technology   : Unity3d
 *  * Date created : Monday 16 Jul 2012 -- 10:22:20 
 *  * Author       : P. Smith 
 *  * Purpose      : 
 *  * 
 *  **************************************************************************************************/
using UnityEngine;
using System.Collections;

public class TestScene : MonoBehaviour
{
  public float NaturalHorizontalRes = 1024.0f;
  public float NaturalVerticalRes = 768.0f;
  /************************************************************************************************
  * NAME   : Start
  * ACTION : 
  * INPUT  : 
  * OUTPUT : 
  *************************************************************************************************/
  void Start()
  {
  }
  /************************************************************************************************
  * NAME   : OnGUI
  * ACTION : 
  * INPUT  : 
  * OUTPUT : 
  *************************************************************************************************/
  void OnGUI()
  {
    GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(Screen.width / NaturalHorizontalRes, Screen.height / NaturalVerticalRes, 1));

    GUI.Label(new Rect(20, 20, 800, 40), "Erno Version 1,  2 Cubes version");

    if(GUI.Button(new Rect((NaturalHorizontalRes / 2) - 200, 300, 400, 60), "End Game Submit Score"))
    {
      PlayerPrefs.SetInt("CurrentScore", 1000 * Random.Range(1, 100));
      Application.LoadLevel("HighScoresScene");
    }
    if(GUI.Button(new Rect((NaturalHorizontalRes / 2) - 200, 400, 400, 60), "Just Show Highscores"))
    {
      PlayerPrefs.SetInt("CurrentScore", 0);
      Application.LoadLevel("HighScoresScene");
    }
  }

  /************************************************************************************************
  * NAME   : Update
  * ACTION : 
  *************************************************************************************************/
  void Update()
  {

  }
}

