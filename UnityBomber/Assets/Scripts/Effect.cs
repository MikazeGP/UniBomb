using UnityEngine;
using System.Collections;
using MonobitEngine;

public class Effect : Origin {

    //========================================================
    // 定数
    //========================================================
    // ゲームマネージャーオブジェクトの名前
    private const string GAME_MGR = "GameMgr";
    // RPC
    // SEを再生
    private const string RPC_PLAYBOMBSE = "PlayBombSE";
    // キル情報送受信処理
    private const string RPC_RECV_KILLSCORE = "RecvKillScore";

    //========================================================
    // リテラル
    //========================================================
    // タイマー
    private Timer m_timer;
    // プレイヤー
    public Player m_plr;
    // ゲームマネージャー
    private GameMgr m_gameMgr;
    // プレイヤーの名前
    private string m_playerName;
    // 生成したプレイヤーID
    private int m_createId;

    // 更新処理
    void Start () {

        m_gameMgr = GameObject.Find(GAME_MGR).GetComponent<GameMgr>();

        this.SetTimer();

        if (!monobitView.isMine) {

            return;
        }

        m_playerName = GrobalData.Instance._plrCharaName;
        
        ChangeScale(m_playerName);

        monobitView.RPC(RPC_PLAYBOMBSE, MonobitTargets.All, m_playerName);
    }

    // Update is called once per frame
    void Update () {

        if (m_timer.Update()) {

        }
	}
    
    // 終了処理
    void End() {

        //Vanish();

        MonobitEngine.MonobitNetwork.Destroy(gameObject);
        //Destroy(this);
    }

    void OnTriggerEnter(Collider col) {

        if(col.tag == PLAYER_TAG) {
            if (col.GetComponent<Player>() != null && col.GetComponent<Player>().m_deathFlag == true){
                return;
            }
            if(col.GetComponent<Player>().m_invincible == true) { return; }
            if ((monobitView.isMine)) { return; }

            monobitView.RPC(RPC_RECV_KILLSCORE, MonobitTargets.All, m_createId);
        }
    }

    // タイマーを設定
    void SetTimer() {

        m_timer = new Timer();
        m_timer.LimitTime = 1.05f;
        m_timer.FireDelegate = End;
    }

    // キャラに応じてエフェクトの大きさを変更
    void ChangeScale(string name) {

        // キャラの名前で変更
        switch (name)
        {

            case "UnityChan":
                float utc_Size = 1.3f * m_plr.m_bombPower;
                SetScale(utc_Size, utc_Size, utc_Size);
                break;

            case "Misaki":
                float misaki_Size = 1.1f * m_plr.m_bombPower;
                SetScale(misaki_Size, misaki_Size, misaki_Size);
                break;

            case "Yuko":
                float yuko_Size = 1.7f * m_plr.m_bombPower;
                SetScale(yuko_Size, yuko_Size, yuko_Size);
                break;

            default:

                SetScale(1, 1, 1);
                break;
        }
    }
    //========================================================
    // UPC処理  
    //========================================================
    [MunRPC]
    /// <summary>
    /// 爆発音を再生
    /// </summary>
    /// <param name="plrName"></param>
    void PlayBombSE(string plrName) {

        // キャラに応じて再生する爆発音を変更する
        switch (m_playerName)
        {

            case "UnityChan":

                // ここで音を再生
                AudioManager.Instance.PlaySE(AUDIO.SE_UTC_RUPTURE);

                break;

            case "Misaki":

                // ここで音を再生
                AudioManager.Instance.PlaySE(AUDIO.SE_MISAK_RUPTURE);

                break;

            case "Yuko":

                // ここで音を再生
                AudioManager.Instance.PlaySE(AUDIO.SE_YUKO_RUPTURE);

                break;

            default:

                AudioManager.Instance.PlaySE(AUDIO.SE_UTC_RUPTURE);

                break;
        }
    }
    /// <summary>
    /// キル情報受信処理
    /// </summary>
    /// <param name="id"></param>
    [MunRPC]
    void RecvKillScore(int id) {

        print("死亡処理受信");
        m_gameMgr.m_plrKill[id]++;
    }
    //========================================================
    // UPC処理はここまで
    //========================================================

   void OnMonobitInstantiate(MonobitEngine.MonobitMessageInfo info) {

        Debug.Log("OnMonobitInstantiate : creator name = " + info.sender.ID);

        m_createId = info.sender.ID - 1;
    }
}
