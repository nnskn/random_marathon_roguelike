//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

  int[,] map;

    void Start()
    {
      //マップ情報を取得
      map = DungeonManager.GetMap();
    }


    void Update()
    {
      //敵とプレイヤーの座標を取得
      Transform myTransform = this.transform;
      Vector3 my_pos = myTransform.position;
      Vector3 me_pos = GameObject.Find("me(Clone)").transform.position;


      //プレイヤーとのx,z座標の差を求める
      float div_x = my_pos.x - me_pos.x;
      float div_z = my_pos.z - me_pos.z;
      float div_x_abs = div_x;
      float div_z_abs = div_z;
      if (div_x <= 0) div_x_abs = -div_x_abs;
      if (div_z <= 0) div_z_abs = -div_z_abs;


      //近くにいるかどうか（周囲６マス以内かどうか）
      if (div_x_abs <= 6.0f && div_z_abs <= 6.0f) {   //近い場合
        //近くにいるかどうか（周囲1マス以内かどうか）
        if (div_x_abs <= 1.0f && div_z_abs <= 1.0f) {   //戦闘状態

          //戦闘方法どうしようかな







        }else {   //接近してくる
          if (Input.anyKeyDown){
            if (div_x_abs != 0) {
              if (div_x >= 0) {
                if (map[(int)my_pos.x - 1, (int)my_pos.z] != 0) my_pos.x -= 1.0f;
                else {
                  if (div_z >= 0) {
                    if (map[(int)my_pos.x, (int)my_pos.z - 1] != 0) my_pos.z -= 1.0f;
                  }else {
                    if (map[(int)my_pos.x, (int)my_pos.z + 1] != 0) my_pos.z += 1.0f;
                  }
                }
              }else {
                if (map[(int)my_pos.x + 1, (int)my_pos.z] != 0) my_pos.x += 1.0f;
                else {
                  if (div_z >= 0) {
                    if (map[(int)my_pos.x, (int)my_pos.z - 1] != 0) my_pos.z -= 1.0f;
                  }else {
                    if (map[(int)my_pos.x, (int)my_pos.z + 1] != 0) my_pos.z += 1.0f;
                  }
                }
              }
            }else{
              if (div_z >= 0) {
                if (map[(int)my_pos.x, (int)my_pos.z - 1] != 0) my_pos.z -= 1.0f;
                else {
                  if (div_x >= 0) {
                    if (map[(int)my_pos.x - 1, (int)my_pos.z] != 0) my_pos.x -= 1.0f;
                  }else {
                    if (map[(int)my_pos.x + 1, (int)my_pos.z] != 0) my_pos.x += 1.0f;
                  }
                }
              }else {
                if (map[(int)my_pos.x, (int)my_pos.z + 1] != 0) my_pos.z += 1.0f;
                else {
                  if (div_x >= 0) {
                    if (map[(int)my_pos.x - 1, (int)my_pos.z] != 0) my_pos.x -= 1.0f;
                  }else {
                    if (map[(int)my_pos.x + 1, (int)my_pos.z] != 0) my_pos.x += 1.0f;
                  }
                }
              }
            }
          }
        }
      }else{    //遠い場合（ランダム行動）
        if (Input.anyKeyDown){
            int xxx = Random.Range(0, 100);
            if (xxx <= 25) {
              if (map[(int)my_pos.x, (int)my_pos.z + 1] != 0) my_pos.z += 1.0f;
              else {
                if (map[(int)my_pos.x - 1, (int)my_pos.z] != 0) my_pos.x -= 1.0f;
              }
            }else if (xxx <= 50) {
              if (map[(int)my_pos.x - 1, (int)my_pos.z] != 0) my_pos.x -= 1.0f;
              else {
                if (map[(int)my_pos.x, (int)my_pos.z + 1] != 0) my_pos.z += 1.0f;
              }
            }else if (xxx <= 75) {
              if (map[(int)my_pos.x, (int)my_pos.z - 1] != 0) my_pos.z -= 1.0f;
              else {
                if (map[(int)my_pos.x + 1, (int)my_pos.z] != 0) my_pos.x += 1.0f;
              }
            }else{
              if (map[(int)my_pos.x + 1, (int)my_pos.z] != 0) my_pos.x += 1.0f;
              else {
                if (map[(int)my_pos.x, (int)my_pos.z - 1] != 0) my_pos.z -= 1.0f;
              }
            }
        }
      }
      myTransform.position = my_pos;  // 座標を設定
    }
}
