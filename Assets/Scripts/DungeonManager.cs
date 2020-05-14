using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
		[Header("床のオブジェクト - Floor Object -")]
		[SerializeField]
		private GameObject floor;

    [Header("壁のオブジェクト - Wall Object -")]
    [SerializeField]
    private GameObject wall;

    [Header("プレイヤーのオブジェクト - Player Object -")]
    [SerializeField]
    private GameObject player;

    [Header("敵のオブジェクト - Enemy Object -")]
    [SerializeField]
    private GameObject enemy;

    [Header("アイテムのオブジェクト - Item Object -")]
    [SerializeField]
    private GameObject item;

    [Header("ゴールのオブジェクト - Goal Object -")]
    [SerializeField]
    private GameObject goal;


    [Header("マップ全体の大きさ")]
    [SerializeField]
    [Range(20, 100)]
    public int MapWidth = 50;
    [SerializeField]
    [Range(20, 100)]
    public int MapHeight = 30;


    [Header("敵やアイテムの生成数")]
    [SerializeField]
    [Range(1, 10)]
    public int max_enemy = 4;
    [SerializeField]
    [Range(1, 10)]
    public int max_item = 3;

    [Header("壁の高さ")]
    [SerializeField]
    int WallHeght = 1;


    [HideInInspector]
    public static int[,] Map;     //マップ情報

    [Header("あくまで目安です！最小値を下回る場合もございます！!")]
    [Header("部屋の数 Min,Max（最小,最大）-- Number of rooms Min, Max --")]
    [SerializeField]
    [Range(1, 10)]
    int MinRooms = 1;
    [SerializeField]
    [Range(1, 20)]
    int MaxRooms = 13;
    int roomNum;


    [Header("部屋の一辺の最小の大きさ")]
    [SerializeField]
    [Range(4, 16)]
    int roomMinSize = 6;

    List<SpritRoomInfomation> roomSprit = new List<SpritRoomInfomation>();


    private void Start()
    {
      Create();
    }



    public void Create()
    {
      MapResetData();
      MapSprit();
      CreateRoom();
      CreateRoad();
      CreateDungeon();
    }




    void MapResetData()
    {
      Map = new int[MapWidth, MapHeight]; //Mapデータ[横,縦]

      // 壁しかないMapデータを作る -------------------
      for (int i = 0; i < MapWidth; i++)
      {
        for (int j = 0; j < MapHeight; j++)
        {
          Map[i, j] = Roguelike3DGroval.wallID;
        }
      }
    }




    void MapSprit()
    {
      roomSprit.Add(new SpritRoomInfomation());
      roomNum = Random.Range(MinRooms, MaxRooms); //部屋数を決める
      											// 大部屋作成
      roomSprit[0].Top = 0;
      roomSprit[0].Left = 0;
      roomSprit[0].Bottom = MapHeight - 1;
      roomSprit[0].Right = MapWidth - 1;
      roomSprit[0].areaRank = roomSprit[0].Bottom * roomSprit[0].Right; //部屋の大きさ

      for (int i = 1; i < roomNum; i++)
      {
        roomSprit.Add(new SpritRoomInfomation());
        int Target = 0; //分割する部屋
        int AreaMax = 0;    //最大面積だった部屋の面積

        // 最大の面積を持つ部屋を求める
        for (int j = 0; j < i; j++)
        {
          if (roomSprit[j].areaRank >= AreaMax)
          {
            AreaMax = roomSprit[j].areaRank;
            Target = j;
          }
        }
        // 分割点を求める
        if (roomSprit[Target].Right - roomSprit[Target].Left >= roomSprit[Target].Bottom - roomSprit[Target].Top)
        {
          //横分割
          // 縦横の大きさが最小値*2より大きい場合は実行
          if (roomSprit[Target].Right - roomSprit[Target].Left >= roomMinSize * 2 && roomSprit[Target].Bottom - roomSprit[Target].Top >= roomMinSize * 2)
          {

            // 分割点を求めて左座標へ入力する
            roomSprit[i].Left = roomSprit[Target].Left + Random.Range(roomMinSize, roomSprit[Target].Right - roomSprit[Target].Left - roomMinSize);
            roomSprit[i].Right = roomSprit[Target].Right;
            roomSprit[Target].Right = roomSprit[i].Left - 1;
            roomSprit[i].Top = roomSprit[Target].Top;
            roomSprit[i].Bottom = roomSprit[Target].Bottom;

            for (int ID = roomSprit[Target].childRoom.Count - 1; ID >= 0; ID--)
            {
              if (Target == roomSprit[roomSprit[Target].childRoom[ID]].parentRoom)
              {
                if (roomSprit[Target].Right < roomSprit[roomSprit[Target].childRoom[ID]].Left)
                {
                  roomSprit[roomSprit[Target].childRoom[ID]].parentRoom = i;
                  roomSprit[i].childRoom.Add(roomSprit[Target].childRoom[ID]);
                  roomSprit[i].isSpritX.Add(true);
                  roomSprit[i].childSpritPos.Add(roomSprit[roomSprit[Target].childRoom[ID]].Left);

                  roomSprit[Target].childRoom.RemoveAt(ID);
                  roomSprit[Target].isSpritX.RemoveAt(ID);
                  roomSprit[Target].childSpritPos.RemoveAt(ID);
                  break;

                }
              }
            }
            roomSprit[i].parentRoom = Target;
            roomSprit[Target].childRoom.Add(i);
            roomSprit[Target].isSpritX.Add(true);
            roomSprit[Target].childSpritPos.Add(roomSprit[i].Left);
          }
          else
          {
            roomNum = i;
            break;
          }
          roomSprit[i].areaRank = (roomSprit[i].Right - roomSprit[i].Left) * (roomSprit[i].Bottom - roomSprit[i].Top);
          roomSprit[Target].areaRank = (roomSprit[Target].Right - roomSprit[Target].Left) * (roomSprit[Target].Bottom - roomSprit[Target].Top);

        }
        else
        {
          //縦分割
          // 縦横の大きさが最小値*2より大きい場合は実行
          if (roomSprit[Target].Right - roomSprit[Target].Left >= roomMinSize * 2 && roomSprit[Target].Bottom - roomSprit[Target].Top >= roomMinSize * 2)
          {

            // 分割点を求めて左座標へ入力する
            roomSprit[i].Top = roomSprit[Target].Top + Random.Range(roomMinSize, roomSprit[Target].Bottom - roomSprit[Target].Top - roomMinSize);
            roomSprit[i].Bottom = roomSprit[Target].Bottom;
            roomSprit[Target].Bottom = roomSprit[i].Top - 1;
            roomSprit[i].Left = roomSprit[Target].Left;
            roomSprit[i].Right = roomSprit[Target].Right;

            for (int ID = roomSprit[Target].childRoom.Count - 1; ID >= 0; ID--)
            {
              if (Target == roomSprit[roomSprit[Target].childRoom[ID]].parentRoom)
              {
                if (roomSprit[Target].Bottom < roomSprit[roomSprit[Target].childRoom[ID]].Top)
                {
                  roomSprit[roomSprit[Target].childRoom[ID]].parentRoom = i;
                  roomSprit[i].childRoom.Add(roomSprit[Target].childRoom[ID]);
                  roomSprit[i].isSpritX.Add(false);
                  roomSprit[i].childSpritPos.Add(roomSprit[roomSprit[Target].childRoom[ID]].Top);

                  roomSprit[Target].childRoom.RemoveAt(ID);
                  roomSprit[Target].isSpritX.RemoveAt(ID);
                  roomSprit[Target].childSpritPos.RemoveAt(ID);
                  break;

                }
              }
            }
            roomSprit[i].parentRoom = Target;
            roomSprit[Target].childRoom.Add(i);
            roomSprit[Target].isSpritX.Add(false);
            roomSprit[Target].childSpritPos.Add(roomSprit[i].Top);
          }
          else
          {
            roomNum = i;
            break;
          }
          roomSprit[i].areaRank = (roomSprit[i].Right - roomSprit[i].Left) * (roomSprit[i].Bottom - roomSprit[i].Top);
          roomSprit[Target].areaRank = (roomSprit[Target].Right - roomSprit[Target].Left) * (roomSprit[Target].Bottom - roomSprit[Target].Top);

        }
      }
    }





    void CreateRoom()
    {
      int ratioX; //範囲を狭めた部屋の幅
      int ratioY; //範囲を狭めた部屋の高さ
      int moveX;  //範囲を狭めた時、範囲を動かす幅
      int moveY;  //範囲を狭めた時、範囲を動かす高さ
      for (int i = 0; i < roomNum; i++)   //作成した区画（部屋）数まで実行する
      {
        if (roomSprit[i] != null)
        {
          ratioY = roomSprit[i].Bottom - roomSprit[i].Top;    //部屋の高さを代入
          ratioY = Mathf.FloorToInt(Random.Range(0.60f, 0.80f) * ratioY);   //部屋の高さを乱数で調整
          ratioX = roomSprit[i].Right - roomSprit[i].Left;    //部屋の幅を代入
          ratioX = Mathf.FloorToInt(Random.Range(0.60f, 0.80f) * ratioX);   //部屋の幅を乱数で調整
          if (ratioY * 2 < ratioX)// 部屋が横長だった場合横の大きさを半分にする
          {
            ratioX /= 2;
          }
          else if (ratioX * 2 < ratioY)// 部屋が縦長だった場合縦の大きさを半分にする
          {
            ratioY /= 2;
          }


          moveY = (roomSprit[i].Bottom - roomSprit[i].Top - ratioY) / 2;  //上から下に動かす座標なので狭めた高さの半分を代入
          moveX = (roomSprit[i].Right - roomSprit[i].Left - ratioX) / 2;  //左から右に動かす座標なので狭めた幅の半分を代入
          roomSprit[i].Top += moveY;    // 区画の上座標に動かす高さの座標を足す
          roomSprit[i].Bottom = roomSprit[i].Top + ratioY;    // 区画の下座標に区画の上座標と部屋の高さを足す
          roomSprit[i].Left += moveX;   // 区画の左座標に動かす幅の座標を足す
          roomSprit[i].Right = roomSprit[i].Left + ratioX;    // 区画の右座標に区画の左座標と部屋の幅を足す

          // 部屋の範囲までMap配列に書き込む
          for (int j = 0; j < ratioY; j++)    //部屋の高さの範囲までループ
          {
            for (int k = 0; k < ratioX; k++)    //部屋の幅の範囲までループ
            {
              Map[roomSprit[i].Left + k + 1, roomSprit[i].Top + j + 1] = Roguelike3DGroval.roomID + i;  // 部屋の左側座標と増分をMapデータののXに、部屋の上側座標と増分をMapデータののYに設定する
            }
          }
        }
        else
        break;
      }
    }




    void CreateRoad()
    {
      int NowPos = 0;
      int NowDis = 0;
      int NextPos = 0;
      int NextDis = 0;
      int nowLines = 0;
      int nextLines = 0;

      for (int roomID = 0; roomID < roomNum; roomID++)
      {
        for (int childID = roomSprit[roomID].childRoom.Count - 1; childID >= 0; childID--)
        {
          if (roomSprit[roomID].isSpritX[childID])
          {
            // X分割された
            NowPos = roomSprit[roomID].Bottom - roomSprit[roomID].Top;
            NowPos = Random.Range(1, NowPos) + roomSprit[roomID].Top;
            NextPos = roomSprit[roomSprit[roomID].childRoom[childID]].Bottom - roomSprit[roomSprit[roomID].childRoom[childID]].Top;
            NextPos = Random.Range(1, NextPos) + roomSprit[roomSprit[roomID].childRoom[childID]].Top;

            NowDis = roomSprit[roomID].childSpritPos[childID] - roomSprit[roomID].Right + 1;
            NextDis = roomSprit[roomSprit[roomID].childRoom[childID]].Left - roomSprit[roomID].childSpritPos[childID] + 1;

            // ライン補正
            if (roomSprit[roomID].Right + 1 < MapWidth)
            {
              if (NowPos + 1 < MapHeight)
              {
                if (Map[roomSprit[roomID].Right + 1, NowPos + 1] == Roguelike3DGroval.roadID)
                NowPos++;
              }
              if (NowPos - 1 > 0)
              {
                if (Map[roomSprit[roomID].Right + 1, NowPos - 1] == Roguelike3DGroval.roadID)
                NowPos--;
              }
            }

            if (roomSprit[roomSprit[roomID].childRoom[childID]].Left - 1 > 0)
            {
              if (NextPos + 1 < MapHeight)
              {
                if (Map[roomSprit[roomSprit[roomID].childRoom[childID]].Left - 1, NextPos + 1] == Roguelike3DGroval.roadID)
                NextPos++;
              }
              if (NextPos - 1 > 0)
              {
                if (Map[roomSprit[roomSprit[roomID].childRoom[childID]].Left - 1, NextPos - 1] == Roguelike3DGroval.roadID)
                NextPos--;
              }
            }


            // 横ライン作成 -------------
            // →ライン作成
            for (nowLines = 0; nowLines < NowDis; nowLines++)
            {
              if (nowLines + roomSprit[roomID].Right < MapWidth)
              {
                if (Map[nowLines + roomSprit[roomID].Right, NowPos] == Roguelike3DGroval.wallID)
                {
                  Map[nowLines + roomSprit[roomID].Right, NowPos] = Roguelike3DGroval.roadID;

                }
              }
              else
              break;

            }

            // ←ライン作成
            for (nextLines = 0; nextLines < NextDis; nextLines++)
            {
              if (-nextLines + roomSprit[roomSprit[roomID].childRoom[childID]].Left > 0)
              {
                if (Map[-nextLines + roomSprit[roomSprit[roomID].childRoom[childID]].Left, NextPos] == Roguelike3DGroval.wallID)
                {
                  Map[-nextLines + roomSprit[roomSprit[roomID].childRoom[childID]].Left, NextPos] = Roguelike3DGroval.roadID;
                }

              }
              else
              break;


            }


            // 縦ライン作成
            for (int lines = 0; ; lines++)
            {
              // NOWとNEXT、どちらの方が高さが大きいか調べ、縦ラインを作成する
              if (NowPos >= NextPos)  //NOWの方が多い時（次の部屋の通路の方が上側）
              {
              if (NextPos + lines < NowPos)
              {
                if (Map[roomSprit[roomID].childSpritPos[childID], NextPos + lines] == Roguelike3DGroval.wallID)
                {
                  Map[roomSprit[roomID].childSpritPos[childID], NextPos + lines] = Roguelike3DGroval.roadID;
                }
              }
              else
              {
                RoadExtend(false, roomSprit[roomID].childSpritPos[childID], NextPos + lines);
                break;
              }


              }
              else    //NEXTの方が大きいとき（現在の部屋の通路の方が上側）
              {
                if (NowPos + lines < NextPos)
                {
                  if (Map[roomSprit[roomID].childSpritPos[childID], NowPos + lines] == Roguelike3DGroval.wallID)
                  {
                    Map[roomSprit[roomID].childSpritPos[childID], NowPos + lines] = Roguelike3DGroval.roadID;
                  }
                }
                else
                {
                  RoadExtend(false, roomSprit[roomID].childSpritPos[childID], NowPos + lines);
                  break;
                }

              }
            }

          }
          else
          {
            // Y分割された
            NowPos = roomSprit[roomID].Right - roomSprit[roomID].Left;
            NowPos = Random.Range(1, NowPos) + roomSprit[roomID].Left;
            NextPos = roomSprit[roomSprit[roomID].childRoom[childID]].Right - roomSprit[roomSprit[roomID].childRoom[childID]].Left;
            NextPos = Random.Range(1, NextPos) + roomSprit[roomSprit[roomID].childRoom[childID]].Left;

            NowDis = roomSprit[roomID].childSpritPos[childID] - roomSprit[roomID].Bottom + 1;
            NextDis = roomSprit[roomSprit[roomID].childRoom[childID]].Top - roomSprit[roomID].childSpritPos[childID] + 1;

            // ラインの修正

            // ライン補正
            if (roomSprit[roomID].Bottom + 1 < MapHeight)
            {
              if (NowPos + 1 < MapWidth)
              {
                if (Map[NowPos + 1, roomSprit[roomID].Bottom + 1] == Roguelike3DGroval.roadID)
                NowPos++;
              }
              if (NowPos - 1 > 0)
              {
                if (Map[NowPos - 1, roomSprit[roomID].Bottom + 1] == Roguelike3DGroval.roadID)
                NowPos--;
              }
            }

            if (roomSprit[roomSprit[roomID].childRoom[childID]].Top + 1 > 0)
            {
              if (NowPos + 1 < MapWidth)
              {
                if (Map[NowPos + 1, roomSprit[roomSprit[roomID].childRoom[childID]].Top + 1] == Roguelike3DGroval.roadID)
                NowPos++;
              }
              if (NowPos - 1 > 0)
              {
                if (Map[NowPos - 1, roomSprit[roomSprit[roomID].childRoom[childID]].Top + 1] == Roguelike3DGroval.roadID)
                NowPos--;
              }
            }


            // 縦ライン作成 -------------
            // ↓ライン作成
            for (nowLines = 0; nowLines < NowDis; nowLines++)
            {
              if (nowLines + roomSprit[roomID].Bottom < MapHeight)
              {
                if (Map[NowPos, nowLines + roomSprit[roomID].Bottom] == Roguelike3DGroval.wallID)
                {
                  Map[NowPos, nowLines + roomSprit[roomID].Bottom] = Roguelike3DGroval.roadID;
                }
              }


            }

            // ↑ライン作成
            for (nextLines = 0; nextLines < NextDis; nextLines++)
            {
              if (-nextLines + roomSprit[roomSprit[roomID].childRoom[childID]].Top > 0)
              {
                if (Map[NextPos, -nextLines + roomSprit[roomSprit[roomID].childRoom[childID]].Top] == Roguelike3DGroval.wallID)
                {
                  Map[NextPos, -nextLines + roomSprit[roomSprit[roomID].childRoom[childID]].Top] = Roguelike3DGroval.roadID;
                }
              }
            }

            // 横ライン作成
            for (int lines = 0; ; lines++)
            {
            // NOWとNEXT、どちらの方が高さが大きいか調べ、縦ラインを作成する
            if (NowPos >= NextPos)  //NOWの方が多い時（次の部屋の通路の方が上側）
            {
              if (NextPos + lines < NowPos)
              {
                if (Map[NextPos + lines, roomSprit[roomID].childSpritPos[childID]] == Roguelike3DGroval.wallID)  //読み込み元のIDが壁ならば（Y座標が変動）
                Map[NextPos + lines, roomSprit[roomID].childSpritPos[childID]] = Roguelike3DGroval.roadID;   //そのIDを通路にする
              }
              else
              {
                RoadExtend(true, NextPos + lines, roomSprit[roomID].childSpritPos[childID]);
                break;
              }
            }
            else    //NEXTの方が大きいとき（現在の部屋の通路の方が上側）
            {
              if (NextPos > NowPos + lines)
              {
                if (Map[NowPos + lines, roomSprit[roomID].childSpritPos[childID]] == Roguelike3DGroval.wallID)    //読み込み元のIDが壁ならば（Y座標が変動）
                Map[NowPos + lines, roomSprit[roomID].childSpritPos[childID]] = Roguelike3DGroval.roadID;    //そのIDを通路にする
              }
              else
              {
                RoadExtend(true, NowPos + lines, roomSprit[roomID].childSpritPos[childID]);
                break;
              }

            }
            }

          }
        }
      }
    }




    void RoadExtend(bool isX, int x, int y)
    {
      bool isHit = false;
      int extendLine = 0;
      if (isX)
      {
        x++;
        for (; x + extendLine < MapWidth; extendLine++)
        {
          if (Map[x + extendLine, y] == Roguelike3DGroval.roadID || Map[x + extendLine, y] >= Roguelike3DGroval.roomID)
          {
            isHit = true;
            break;
          }
          else if (Map[x + extendLine, y + 1] == Roguelike3DGroval.roadID || Map[x + extendLine, y + 1] >= Roguelike3DGroval.roomID ||
          	Map[x + extendLine, y - 1] == Roguelike3DGroval.roadID || Map[x + extendLine, y - 1] >= Roguelike3DGroval.roomID)
          {
            extendLine++;
            isHit = true;
            break;
          }
        }
        if (isHit)
        {
          for (int Line = 0; Line < extendLine; Line++)
          Map[x + Line, y] = Roguelike3DGroval.roadID;
        }
      }
      else
      {
        y++;
        for (; y + extendLine < MapHeight; extendLine++)
        {
          if (Map[x, y + extendLine] == Roguelike3DGroval.roadID || Map[x, y + extendLine] >= Roguelike3DGroval.roomID)
          {
            isHit = true;
            break;
          }
          else if (Map[x + 1, y + extendLine] == Roguelike3DGroval.roadID || Map[x + 1, y + extendLine] >= Roguelike3DGroval.roomID ||
          	Map[x - 1, y + extendLine] == Roguelike3DGroval.roadID || Map[x - 1, y + extendLine] >= Roguelike3DGroval.roomID)
          {
            isHit = true;
            extendLine++;
            break;
          }
        }
        if (isHit)
        {
          for (int Line = 0; Line < extendLine; Line++)
          Map[x, y + Line] = Roguelike3DGroval.roadID;
        }
      }
    }



    void CreateDungeon()
    {
      GameObject obj;

      bool flag_player = false;
      bool flag_goal = false;
      int now_enemy = 0;
      int now_item = 0;

      for (int i = 0; i < MapWidth; i++)
      {
        for (int j = 0; j < MapHeight; j++)
        {
          // 床を敷き詰める
          //obj = Instantiate(floor, new Vector3(i - MapWidth / 2, 0, j - MapHeight / 2), Quaternion.identity);
          obj = Instantiate(floor, new Vector3(i, 0, j), Quaternion.identity);
          obj.transform.parent = transform;
          // 壁だった場合壁にする
          if (Map[i, j] == Roguelike3DGroval.wallID)
          {
            for (int height = 0; height < WallHeght; height++)
            {
              //obj = Instantiate(wall, new Vector3(i - MapWidth / 2, height + 1, j - MapHeight / 2), Quaternion.identity);
              obj = Instantiate(wall, new Vector3(i, height + 1, j), Quaternion.identity);
              obj.transform.parent = transform;
            }
          }
        }
      }

      //敵やアイテムなどを生成し終えるまで
      while (now_enemy != max_enemy)
      {
        //ランダムな数字xxx,yyyを生成して、その座標が壁じゃないならプレイヤーやアイテムなどを生成
        int xxx = Random.Range(0, MapWidth);
        int yyy = Random.Range(0, MapHeight);
        if (Map[xxx, yyy] != Roguelike3DGroval.wallID)
        {
          if (flag_player == false)
          {
            obj = Instantiate(player, new Vector3(xxx, 1, yyy), Quaternion.identity);   //プレイヤーの生成
            obj.transform.parent = transform;
            flag_player = true;
          }
          else if (flag_goal == false)
          {
            obj = Instantiate(goal, new Vector3(xxx, 1, yyy), Quaternion.identity);     //ゴールの生成
            obj.transform.parent = transform;
            flag_goal = true;
          }
          else if (now_item < max_item)
          {
            obj = Instantiate(item, new Vector3(xxx, 1, yyy), Quaternion.identity);     //アイテムの生成
            obj.transform.parent = transform;
            now_item += 1;
          }else{
            obj = Instantiate(enemy, new Vector3(xxx, 1, yyy), Quaternion.identity);    //敵の生成
            obj.transform.parent = transform;
            now_enemy += 1;
          }
        }
      }

    }

    //マップ情報の呼び出し
    public static int[,] GetMap(){
        return Map;
    }

}


public class SpritRoomInfomation
{
  public int Top = 0;     //部屋の上側
	public int Left = 0;    //部屋の左側
	public int Bottom = 0;  //部屋の下側
	public int Right = 0;   //部屋の右側
	public int areaRank = 0;//部屋の面積
	public int parentRoom = 0;  //親の部屋のID
	public List<int> childRoom = new List<int>();    //子の部屋のID
	public List<bool> isSpritX = new List<bool>();   //子の部屋の分割座標軸　true...X軸　false...Y軸
	public List<int> childSpritPos = new List<int>(); //子の部屋の分割元座標　※Topなどの部屋の情報は変更するので変更する前の座標を保持するため
}



public class Roguelike3DGroval
{
   public const int wallID = 0;
	public const int roadID = 1;
	public const int roomID = 100;
}
