using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvUP : MonoBehaviour
{
    //主人公につけるスクリプト
    private CharacterStatus status;//CharacterStatusを変数に取る
    public int m_nextExpBase; // 次のレベルまでに必要な経験値の基本値
    public int m_nextExpInterval; // 次のレベルまでに必要な経験値の増加値
    public int m_prevNeedExp; // 前のレベルに必要だった経験値
    public int m_needExp; // 次のレベルに必要な経験値
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        status = GetComponent<CharacterStatus>();//クラス内の変数（ステータス）を使えるようにする
        m_needExp = GetNeedExp(1); // 次のレベルに必要な経験値
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))//実際は敵を倒した時に
        {
            AddExp(1);//()内に敵の持つ経験値
        }
    }
    
    // 経験値を増やす関数
    // 敵を倒した時に呼び出される
    public void AddExp(int getexp)
    {
        // 経験値を増やす
        status.exp += getexp;

        // まだレベルアップに必要な経験値に足りていない場合、ここで処理を終える
        if (status.exp < m_needExp) return;

        // レベルアップする
        //ログに表示している部分は画面上にメッセージとして表示したい
        //ステータスの増減も要調整
        status.level++;
        Debug.Log("レベルが" + status.level + "になった！");
        status.maxhp += status.level * 5;
        status.hp = status.maxhp;
        status.pow += status.level * 1;
        status.vit += status.level * 1;
        Debug.Log("最大HPが" + status.maxhp + "になった！");
        Debug.Log("攻撃力が" + status.pow + "になった！");
        Debug.Log("守備力が" + status.vit + "になった！");

        // 今回のレベルアップに必要だった経験値を記憶しておく
        // （経験値ゲージの表示に使用するため）
        m_prevNeedExp = m_needExp;

        // 次のレベルアップに必要な経験値を計算する
        m_needExp = GetNeedExp(status.level);


    }
    private int GetNeedExp(int level)
    {
        /*
         * 例えば、m_nextExpBase が 16、m_nextExpInterval が 18 の場合、
         *
         * レベル 1：16 + 18 * 0 = 16
         * レベル 2：16 + 18 * 1 = 34
         * レベル 3：16 + 18 * 4 = 88
         * レベル 4：16 + 18 * 9 = 178
         *
         * このような計算式になり、レベルが上がるほど必要な経験値が増えていく
         */
        return m_nextExpBase +
            m_nextExpInterval * ((level - 1) * (level - 1));
    }
}
