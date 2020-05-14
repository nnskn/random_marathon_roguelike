using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//シーンマネジメントを有効にする

public class ToNext : MonoBehaviour
{

    void Update()
    {

      //プレイヤーとゴールの座標を取得
      Vector3 me_pos = GameObject.Find("me(Clone)").transform.position;
      Vector3 goal_pos = GameObject.Find("Tree(Clone)").transform.position;
      float me_x = me_pos.x;
      float me_z = me_pos.z;
      float goal_x = goal_pos.x;
      float goal_z = goal_pos.z;


      //プレイヤーが触ると
      if ((me_x <= goal_x + 0.5f && me_x >= goal_x - 0.5f) && (me_z <= goal_z + 0.5f && me_z >= goal_z - 0.5f))
      {
          SceneManager.LoadScene("DungeonScene"); //Sceneをロードする
      }


    }
}
