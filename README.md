# random_marathon_roguelike
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Roguelike3D
{using UnityEngine.UI; //必須

public class NewBehaviourScript : MonoBehaviour
{
    Text MessageDialog1; //メッセージダイアログの表示
    Text MessageDialog2; //メッセージダイアログの表示
    Text MessageDialog3; //メッセージダイアログの表示
    Text MessageDialog4; //メッセージダイアログの表示
    Text MessageDialog5; //メッセージダイアログの表示
    Queue<string> Message = new Queue<string>(); //メッセージダイアログの入れ物（キュー）
    int MaxMessegeCount = 30; //メッセージの最大ログ数
    string MessegeOut;

    //各クラスから受けとったメッセージを入れるメソッド
    public void MessageDialogIn(string InMessege)
    {
        //受け取ったメッセージを格納
        Message.Enqueue(InMessege);
        //メッセージログが最大になったら一つ消す
        if (Message.Count >= MaxMessegeCount)
        {
            Message.Dequeue();
        }
        MessageDialogOut();
    }

    //メッセージ表示用
    void MessageDialogOut()
    {
        //メッセージダイアログのテキストを取得（表示用）
        MessageDialog1 = GameObject.Find("MessageText (1)").GetComponent<Text>();
        MessageDialog2 = GameObject.Find("MessageText (2)").GetComponent<Text>();
        MessageDialog3 = GameObject.Find("MessageText (3)").GetComponent<Text>();
        MessageDialog4 = GameObject.Find("MessageText (4)").GetComponent<Text>();
        MessageDialog5 = GameObject.Find("MessageText (5)").GetComponent<Text>();

        //メッセージログを下から上にコピーしていき、最後に最下段はDequeueしたものを入れる
        MessageDialog1.text = MessageDialog2.text;
        MessageDialog2.text = MessageDialog3.text;
        MessageDialog3.text = MessageDialog4.text;
        MessageDialog4.text = MessageDialog5.text;
        MessageDialog5.text = Message.Dequeue(); ;
    } }

}
