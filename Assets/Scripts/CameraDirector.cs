using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDirector : MonoBehaviour
{
  
    private GameObject player;   //プレイヤー情報格納用
    private Vector3 ppp = new Vector3(0, 10, 0);    //10のところを変えるとカメラの視野が変わる


    void Update()
    {

        //プレイヤーの座標を取得
        this.player = GameObject.Find("me(Clone)");

        //新しいトランスフォームの値を代入する
        transform.position = player.transform.position + ppp;
    }

}
