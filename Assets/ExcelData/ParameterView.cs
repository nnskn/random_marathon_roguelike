using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParameterView : MonoBehaviour
{
    [SerializeField]
    private CharacterParameter param;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < param.Parameter.Count; i++)
        {
            Debug.Log(param.Parameter[i].name +
                "　属性：" + param.Parameter[i].element.ToString() +
                "　最大HP：" + param.Parameter[i].maxhp +
                "　攻撃力：" + param.Parameter[i].pow);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}