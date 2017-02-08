using UnityEngine;
using System.Collections;
using MonobitEngine;

/// <summary>
/// プレイヤークラス
/// </summary>
public class Player : Origin{

    //========================================================
    // 定数
    //========================================================

    // ゲームマネージャーオブジェクトの名前
    private const string GAME_MGR = "GameMgr";

    // RPC
    // 死亡受信処理名
    private const string RPC_RECV_DIE = "RecvDie";
    // ボム生成受信処理名
    private const string RPC_RECV_SHOTBOMB = "RecShotBomb";
    

    //========================================================
    // リテラル
    //========================================================
    // 現在所持しているボムの数
    public int m_stockBomb;
    // 移動速度
    public float m_moveSpeed;

    // プレイヤーが移動できる最大位置
    private float EndPosxMin = -4.8f;
    private float EndPosxMax = 4.8f;
    private float EndPoszMin = -4.3f;
    private float EndPoszMax = 4.3f;

    // 初期化フラグ
    private bool m_init = false;
    // ボム発射フラグ true..発射可能 false..発射不可能
    public bool m_shotBomb;
    // 死亡フラグ true..死亡 false..生存
    public bool m_deathFlag; 
    // タイマークラス
    private Timer m_timer;
    // プレイヤー残機
    private int m_plrStock;
    // ゲームマネージャー
    private GameMgr m_gamemgr;

    //========================================================
    // 初期化処理
    //========================================================
    void Start () {

        // 発射可能にする
        m_shotBomb = true;

        // 最初は生きているので false にする
        m_deathFlag = false;

        // ゲームマネージャーをシーンから探す
        m_gamemgr = GameObject.Find(GAME_MGR).GetComponent<GameMgr>();

        // グローバルデータから残機の数を取得
        m_plrStock = m_gamemgr.m_plrStock[PlayerId];
        // レイヤーを設定
        gameObject.layer = 8 + PlayerId;
    }
    //========================================================
    // 初期化処理はここまで
    //========================================================

    //========================================================
    // 更新処理
    //========================================================
    void Update () {

        // オブジェクト所有権を所持しなければ実行しない
        if (!monobitView.isMine) {

            // キャラクターコントローラーを無効にする
            GetComponent<CharacterController>().enabled = false;

            return;
        }
        // 死亡したときはタイマーのみ動かす
        if(m_deathFlag == true) {

            if (m_timer.Update()){}

            return;
        }
        
        if(m_gamemgr.m_currentState == GameMgr.GAME_STATE.UPDATE) {

            // 入力処理の更新
            InputUpdate();

        }
	}
    //========================================================
    // 更新処理はここまで
    //========================================================

    //========================================================
    // 入力処理
    //========================================================
    /// <summary>
    /// 入力処理の更新
    /// </summary>
    void InputUpdate() {

        this.ShotBomb();

        this.MoveFunc();
    } 
   /// <summary>
   /// ボム生成処理
   /// </summary>
    void ShotBomb() {

        // Fire1を押したとき
        if (Input.GetButton(FIRE1_BUTTON)) {

            // ボムの残機があり、ボムを発射できるとき
            if (m_shotBomb == true && m_stockBomb > 0) {

                // ボムを生成
                GameObject bombobj = MonobitEngine.MonobitNetwork.Instantiate("Prefabs/Pbomb", new Vector3(X, Y + 0.2f, Z), Quaternion.identity, 0, null);

                // ボムを置けなくする
                m_shotBomb = false;

                // 残機を１減らす
                m_stockBomb--;

                // ボムに生成主の情報を送る
                bombobj.GetComponent<Bomb>().m_plr = this;
                // レイヤーを変更
                bombobj.layer = gameObject.layer;
            }
        }
    }
    /// <summary>
    /// プレイヤーの移動処理
    /// </summary>
    void MoveFunc() {

        // 自分の位置を取得
        Vector3 p = new Vector3(X, 0f, Z);

        // 入力した方向分足す
        p.x = 0.05f*m_moveSpeed*Input.GetAxisRaw(AXIS_HORIZONTAL);
        p.z = 0.05f*m_moveSpeed*Input.GetAxisRaw(AXIS_VERTICAL);

        // 移動する向きに回転
        gameObject.transform.LookAt(p + transform.position);

        // 位置を代入して移動
        gameObject.GetComponent<CharacterController>().Move(p);

        // 移動している時...移動アニメーション 移動していない時...待機処理
        if(Input.GetAxisRaw(AXIS_HORIZONTAL) != 0 || Input.GetAxisRaw(AXIS_VERTICAL) != 0) {

            // 移動アニメーション
            Animator.SetFloat("Speed", 2.0f);

        }else {

            // 待機アニメーション
            Animator.SetFloat("Speed", 0);
        }
        // 移動範囲制限処理
        this.Clamp();
    }

    //========================================================
    // 入力処理はここまで
    //========================================================

   /// <summary>
   /// プレイヤーの移動範囲を制限する
   /// </summary>
    void Clamp()
    {
        // プレイヤーの座標を取得
        Vector3 pos = gameObject.transform.position;

        //プレイヤーの位置の制限
        pos.x = Mathf.Clamp(pos.x, EndPosxMin, EndPosxMax);
        pos.z = Mathf.Clamp(pos.z, EndPoszMin, EndPoszMax);

        //プレイヤーの制限位置を取得
        gameObject.transform.position = pos;
    }

    //========================================================
    // 衝突判定処理
    //========================================================
    void OnTriggerEnter(Collider col) {

        // 爆発エフェクトに触れたら倒れる
        if(col.tag == EXPLOSION_TAG && m_deathFlag == false) {

            // デバッグ用
            print("Col");
            
            // ダウンアニメーション
            Animator.SetBool("Down", true);

            // 死亡フラグをtrueにする
            m_deathFlag = true;

            // タイマーを設定
            SetTimer0();

            // 死亡処理を送信
            monobitView.RPC(RPC_RECV_DIE, MonobitTargets.All, PlayerId);
          
        }
    }
    //========================================================
    // 衝突判定処理はここまで
    //========================================================

    // タイマー
    void SetTimer0(){

        m_timer = new Timer();
        m_timer.LimitTime = 4.0f;
        m_timer.FireDelegate = Death;
    }

    void Death() {

        MonobitEngine.MonobitNetwork.Destroy(gameObject);
    }

    /// <summary>
    /// 死亡ボイスを設定
    /// </summary>
    /// <param name="stock"></param>
    /// <returns></returns>
    private string DieVoice(int stock) {

        string voice = "";

        switch (stock) {

            case 2:
                if(m_plrStock != 1) {

                    voice = AUDIO.VOICE_V2018;

                }else {

                    voice = AUDIO.VOICE_V2019;
                }

                break;

            case 3:

                if (m_plrStock != 1){

                    voice = AUDIO.VOICE_V0018;
                }else{

                    voice = AUDIO.VOICE_V0019;
                }

                break;

            case 4:

                if (m_plrStock != 1){

                    voice = AUDIO.VOICE_V1018;

                }else{

                    voice = AUDIO.VOICE_V1019;
                }

                break;

        }

        return voice;
    }

    //========================================================
    // UPC処理  
    //========================================================
    [MunRPC]
    /// <summary>
    /// 死亡処理を受信
    /// </summary>
    /// <param name="id">プレイヤーID</param>
    void RecvDie(int id) {

        int dieNum = m_gamemgr.m_plrBombStock[id];
        // SEを再生
        AudioManager.Instance.PlayVoice(DieVoice(dieNum));

        m_gamemgr.m_plrStock[id]--;
        m_gamemgr.m_bUpdateUI = true;
        if (m_gamemgr.m_plrStock[id] < 1) {
            m_gamemgr.m_dieFlag[id] = true;
        }
    }
    //========================================================
    // UPC処理はここまで 
    //========================================================
}
