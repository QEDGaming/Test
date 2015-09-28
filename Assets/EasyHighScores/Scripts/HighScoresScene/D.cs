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
 *  *      SYSTEM QVIDS - APPLE - MYAPPS - ASSAULT - ASSETS - SCRIPTS
 *  **************************************************************************************************
 *  * 
 *  * Filename     : D.cs
 *  * Country      : AU
 *  * Market       : ALL AU
 *  * Technology   : QXI200
 *  * Date created : Wednesday 07 Mar 2012 -- 18:11:41 
 *  * Author       : P. Smith 
 *  * Purpose      : 
 *  * 
 *  **************************************************************************************************/
#define DEBUG_LEVEL_LOG
#define DEBUG_LEVEL_WARN
#define DEBUG_LEVEL_ERROR


using UnityEngine;
using System.Collections;


public class D
{
  static int cnt = 0;
  

  /************************************************************************************************
  * NAME   : log
  * ACTION : 
  * INPUT  : string format, params object[] paramList, 
  * OUTPUT : static void 
  *************************************************************************************************/
  [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
  [System.Diagnostics.Conditional("DEBUG_LEVEL_WARN")]
  [System.Diagnostics.Conditional("DEBUG_LEVEL_ERROR")]
  public static void log(string format, params object[] paramList)
  {
    Debug.Log(string.Format(format, paramList));
  }

  /************************************************************************************************
  * NAME   : logx
  * ACTION : 
  * INPUT  : string format, params object[] paramList, 
  * OUTPUT : static void 
  *************************************************************************************************/
  [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
  [System.Diagnostics.Conditional("DEBUG_LEVEL_WARN")]
  [System.Diagnostics.Conditional("DEBUG_LEVEL_ERROR")]
  public static void logx(string format, params object[] paramList)
  {
    string str = cnt + " " + string.Format(format, paramList);

    if(Application.isEditor)
      Debug.Log(str);
    else
    {
#if UNITY_STANDALONE_OSX || UNITY_ANDROID || UNITY_IPHONE
      System.Console.WriteLine(str);
#else
      Debug.Log(str);
#endif
    }
    
    cnt++;
  }
  /************************************************************************************************
  * NAME   : logxx
  * ACTION : 
  * INPUT  : string format, params object[] paramList, 
  * OUTPUT : static void 
  *************************************************************************************************/
  [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
  [System.Diagnostics.Conditional("DEBUG_LEVEL_WARN")]
  [System.Diagnostics.Conditional("DEBUG_LEVEL_ERROR")]
  public static void logxx(string format, params object[] paramList)
  {
    if((cnt % 60) == 0)
    {
      string str = cnt + " " + string.Format(format, paramList);
      Debug.Log(str);
    }
    cnt++;
  }


  /************************************************************************************************
  * NAME   : warn
  * ACTION : 
  * INPUT  : string format, params object[] paramList, 
  * OUTPUT : static void 
  *************************************************************************************************/
  [System.Diagnostics.Conditional("DEBUG_LEVEL_WARN")]
  [System.Diagnostics.Conditional("DEBUG_LEVEL_ERROR")]
  public static void warn(string format, params object[] paramList)
  {
    Debug.LogWarning(string.Format(format, paramList));
  }


  /************************************************************************************************
  * NAME   : error
  * ACTION : 
  * INPUT  : string format, params object[] paramList, 
  * OUTPUT : static void 
  *************************************************************************************************/
  [System.Diagnostics.Conditional("DEBUG_LEVEL_ERROR")]
  public static void error(string format, params object[] paramList)
  {
    Debug.LogError(string.Format(format, paramList));
  }


  /************************************************************************************************
  * NAME   : assert
  * ACTION : 
  * INPUT  : bool condition, 
  * OUTPUT : static void 
  *************************************************************************************************/
  [System.Diagnostics.Conditional("UNITY_EDITOR")]
  [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
  public static void assert(bool condition)
  {
    assert(condition, string.Empty, true);
  }


  /************************************************************************************************
  * NAME   : assert
  * ACTION : 
  * INPUT  : bool condition, string assertString, 
  * OUTPUT : static void 
  *************************************************************************************************/
  [System.Diagnostics.Conditional("UNITY_EDITOR")]
  [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
  public static void assert(bool condition, string assertString)
  {
    assert(condition, assertString, false);
  }


  /************************************************************************************************
  * NAME   : assert
  * ACTION : 
  * INPUT  : bool condition, string assertString, bool pauseOnFail, 
  * OUTPUT : static void 
  *************************************************************************************************/
  [System.Diagnostics.Conditional("UNITY_EDITOR")]
  [System.Diagnostics.Conditional("DEBUG_LEVEL_LOG")]
  public static void assert(bool condition, string assertString, bool pauseOnFail)
  {
    if(!condition)
    {
      Debug.LogError("assert failed! " + assertString);

      if(pauseOnFail)
        Debug.Break();
    }
  }

}
