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
 *  *      SYSTEM QVIDS - APPLE - MYAPPS - ASSAULT - ASSAULT - ASSETS
 *  **************************************************************************************************
 *  * 
 *  * Filename     : HighScoreKey.cs
 *  * Country      : AU
 *  * Market       : ALL AU
 *  * Technology   : QXI200
 *  * Date created : Tuesday 01 May 2012 -- 11:52:42 
 *  * Author       : P. Smith 
 *  * Purpose      : 
 *  * 
 *  **************************************************************************************************/
using UnityEngine;
using System.Collections;

public class HighScoreKey : MonoBehaviour
{
  Vector3 StartPos;
  float uptime;
  bool hit;
  /************************************************************************************************
  * NAME   : Start
  * ACTION : 
  * INPUT  : 
  * OUTPUT : 
  *************************************************************************************************/
  void Start()
  {
    StartPos = transform.localPosition;
    uptime = Time.time;
    hit=false;
  }
  /************************************************************************************************
  * NAME   : HitKey
  * ACTION : 
  * INPUT  : 
  * OUTPUT : 
  *************************************************************************************************/
  public void HitKey(Vector3 normal)
  {
    Vector3 pos = StartPos;
    pos.y -=0.015f;
    transform.localPosition = pos;
    uptime = Time.time+0.1f;
    hit = true;
    
  }
  /************************************************************************************************
  * NAME   : Update
  * ACTION : 
  * INPUT  : 
  * OUTPUT : 
  *************************************************************************************************/
  void Update()
  {
    if(hit==false)
      return;
    if(Time.time > uptime)
      transform.localPosition = StartPos;
  }

  
}

