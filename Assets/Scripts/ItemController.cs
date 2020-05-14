using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{

    void Start()
    {

    }


    void Update()
    {

      //アイテムとプレイヤーの座標を取得
      Vector3 item_pos = this.transform.position;
      Vector3 me_pos = GameObject.Find("me(Clone)").transform.position;
      float me_x = me_pos.x;
      float me_z = me_pos.z;
      float item_x = item_pos.x;
      float item_z = item_pos.z;


      //触れると消滅する
      if ((me_x <= item_x + 0.5f && me_x >= item_x - 0.5f) && (me_z <= item_z + 0.5f && me_z >= item_z - 0.5f))
      {
          Destroy(gameObject);
      }
    }
}
