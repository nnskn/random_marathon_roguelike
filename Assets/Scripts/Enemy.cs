using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private CharacterStatus status;//CharacterStatusを変数に取る
    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<CharacterStatus>();//クラス内の変数（ステータス）を使えるようにする
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
