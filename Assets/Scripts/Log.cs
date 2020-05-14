using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Log : MonoBehaviour
{
    [SerializeField]
    private CharacterParameter parameter;
    
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {

                Debug.Log(parameter.Parameter[0].name + "は" + parameter.Parameter[1].name + "を攻撃した！");
                Debug.Log(parameter.Parameter[1].name + "に" + (parameter.Parameter[0].pow - parameter.Parameter[1].vit) + "のダメージを与えた！");

            }
            if (Input.GetKeyDown(KeyCode.B))
            {

                Debug.Log("Bが押されました");
            }
        }       
    }
    
}
