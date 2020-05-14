using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContoroller : MonoBehaviour{

  int[,] map;

    void Start(){

      //マップ情報を取得
      map = DungeonManager.GetMap();

    }

    void Update(){

      // 座標を取得
      Transform myTransform = this.transform;
      Vector3 pos = myTransform.position;

      if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
          if (map[(int)pos.x, (int)pos.z + 1] != 0) pos.z += 1.0f;
      }
      if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
        if (map[(int)pos.x - 1, (int)pos.z] != 0) pos.x -= 1.0f;
      }
      if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
        if (map[(int)pos.x, (int)pos.z - 1] != 0) pos.z -= 1.0f;
      }
      if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
        if (map[(int)pos.x + 1, (int)pos.z] != 0) pos.x += 1.0f;
      }

      myTransform.position = pos;  // 座標を設定


    }

}
