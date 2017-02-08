using UnityEngine;
using System.Collections;
using MonobitEngine;
using System.Collections.Generic;

/// <summary>
/// チャットや接続を管理するクラス
/// </summary>
public class ChatScript : Origin{

    /** ルーム名. */
    private string roomName = "";

    // マッチングルームの最大人数
    private byte maxPlayers = 4;

    // マッチングルームを公開するかどうかのフラグ
    private bool isVisible = true;

    /** チャット発言文. */
    private string chatWord = "";

    /** チャット発言ログ. */
    List<string> chatLog = new List<string>();

    /**
     * RPC 受信関数.
     */
    [MunRPC]
    void RecvChat(string senderName, string senderWord)
    {
        chatLog.Add(senderName + " : " + senderWord);
        if (chatLog.Count > 10)
        {
            chatLog.RemoveAt(0);
        }
    }
    [MunRPC]
    /// ゲームを開始する
    void GameStart() {

        // プレイヤー名をグローバルデータに登録
        for (int i = 0; i < maxPlayers; i++){
            // 各プレイヤーの名前をグローバルデータに保存
            //GrobalData.Instance._plrName[i] = MonobitNetwork.playerList[i].name;
            GrobalData.Instance._plrName[i] = MonobitPlayer.Find(i + 1).name;
        }
        // 最大プレイ人数をグローバルデータに保存
        GrobalData.Instance._plrCount = maxPlayers;

        // キャラクターセレクトに移動
        FadeManager.Instance.MonobitLoadLevel(CHARA_SELECT_SCENE, 2.5f);
        
    }

    /**
     * GUI制御.
     */
    void OnGUI()
    {
        // MUNサーバに接続している場合
        if (MonobitNetwork.isConnect)
        {
            // ルームに入室している場合
            if (MonobitNetwork.inRoom)
            {
                // ルーム内のプレイヤー一覧の表示
                GUILayout.BeginHorizontal();
                GUILayout.Label("PlayerList : ");
                foreach (MonobitPlayer player in MonobitNetwork.playerList)
                {
                    GUILayout.Label(player.ID+":"+player.name + " ");
                }
                GUILayout.EndHorizontal();

                // ルームからの退室
                if (GUILayout.Button("Leave Room", GUILayout.Width(150)))
                {
                    MonobitNetwork.LeaveRoom();
                    chatLog.Clear();
                    AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
                }

                // チャット発言文の入力
                GUILayout.BeginHorizontal();
                GUILayout.Label("Message : ");
                chatWord = GUILayout.TextField(chatWord, GUILayout.Width(400));
                GUILayout.EndHorizontal();

                // チャット発言文を送信する
                if (GUILayout.Button("Send", GUILayout.Width(100)) && chatWord != "")
                {
                    monobitView.RPC("RecvChat", MonobitTargets.All, MonobitNetwork.playerName, chatWord);
                    chatWord = "";
                    AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
                }

                // 最大プレイ人数に達し、ホストならゲームをスタートする
                if (MonobitNetwork.playerCountInRoom == maxPlayers && MonobitEngine.MonobitNetwork.isHost) {

                    if (GUILayout.Button("Game Start", GUILayout.Width(100))){

                        monobitView.RPC("RecvChat", MonobitTargets.All, MonobitNetwork.playerName, "ゲームを開始します");
                        monobitView.RPC("RecvChat", MonobitTargets.All, MonobitNetwork.playerName, "キャラセレクトに移動します。");
                        monobitView.RPC("GameStart", MonobitTargets.All, null);
                        AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);

                    }
                }

                // チャットログを表示する
                string msg = "";
                for (int i = 0; i < 10; ++i)
                {
                    msg += ((i < chatLog.Count) ? chatLog[i] : "") + "\r\n";
                }
                GUILayout.TextArea(msg);
            }
            // ルームに入室していない場合
            else
            {
                // ルーム名の入力
                GUILayout.BeginHorizontal();
                GUILayout.Label("RoomName : ");
                roomName = GUILayout.TextField(roomName, GUILayout.Width(200));
                GUILayout.EndHorizontal();

                // 最大プレイ人数を入力
                GUILayout.BeginHorizontal();
                GUILayout.Label("Max Player(2～4) : ");
                string tmpInput = GUILayout.TextField(this.maxPlayers.ToString(), GUILayout.Width(50));
                byte.TryParse(tmpInput, out this.maxPlayers);
                GUILayout.EndHorizontal();

                // 自分の作成するルーム名を公開設定にするかどうかのフラグ
                GUILayout.BeginHorizontal();
                this.isVisible = GUILayout.Toggle(this.isVisible, "Visible room");
                GUILayout.EndHorizontal();

                // ルームを作成して入室する
                if (GUILayout.Button("Create Room", GUILayout.Width(150)))
                {
                    RoomSettings roomSettings = new RoomSettings()
                    {
                        isVisible = this.isVisible,
                        isOpen = true,
                        maxPlayers = this.maxPlayers,

                    };
                    
                    MonobitNetwork.CreateRoom(roomName,roomSettings,null);
                    AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
                }

                // ルーム一覧を検索
                foreach (RoomData room in MonobitNetwork.GetRoomData())
                {
                    // ルームを選択して入室する
                    if (GUILayout.Button("Enter Room : " + room.name + "(" + room.playerCount + "/" + ((room.maxPlayers == 0) ? "-" : room.maxPlayers.ToString()) + ")"))
                    {
                        MonobitNetwork.JoinRoom(room.name);
                        AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
                        maxPlayers = room.maxPlayers;
                    }
                }

                // プレイヤー名入力画面に戻る
                if (GUILayout.Button("Back PlayerName", GUILayout.Width(150)))
                {
                    MonobitNetwork.DisconnectServer();
                    AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
                }
            }
        }
        // MUNサーバに接続していない場合
        else
        {
            // プレイヤー名の入力
            GUILayout.BeginHorizontal();
            GUILayout.Label("PlayerName : ");
            MonobitNetwork.playerName = GUILayout.TextField((MonobitNetwork.playerName == null) ? "" : MonobitNetwork.playerName, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            // デフォルトロビーへの自動入室を許可する
            MonobitNetwork.autoJoinLobby = true;

            // MUNサーバに接続する
            if (GUILayout.Button("Connect Server", GUILayout.Width(150)) && MonobitNetwork.playerName != "")
            {
                MonobitNetwork.ConnectServer("UnityBomber_v1.5Beta");
                AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
               
            }

            // モードセレクトに戻る
            if (GUILayout.Button("Back ModeSelect", GUILayout.Width(150)))
            {
                FadeManager.Instance.LoadLevel(MODESELECT_SCENE, 1.0f);
                AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
            }
        }
    }
}