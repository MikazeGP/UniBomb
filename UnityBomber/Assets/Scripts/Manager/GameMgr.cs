using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MonobitEngine;

/// <summary>
/// ゲームメインで各プレイヤーの情報を
/// 管理するクラス
/// </summary>
public class GameMgr : Origin {

    public enum GAME_STATE  : int{
        INIT = 0,
        UPDATE ,
        FINISH 
    };

    //========================================================
    // 定数
    //========================================================
    // 制限時間(現時点)
    private const int LIMIT_TIME = 300;

    // RPC
    private const string RPC_RECV_LOAD_RESULTSCENE = "RecvLoadResultScene";
    // コルーチン
    // カウントダウンコルーチン
    private const string COUNTDOWN_COROUTINE = "CountDown";
    // リザルトコルーチン
    private const string LOADRESULT_COROUTINE = "LoadResult";

   


    //========================================================
    // UI関連
    //========================================================
    // 各プレイヤーの顔UI
    public Image[] m_plrFaceUI;

    // キャラクターの顔のスプライト
    // ユニティちゃん
    public Sprite m_utcFaceN_UI,m_utcFaceD_UI;
    // ミサキ
    public Sprite m_misakiFaceN_UI, m_misakiFaceD_UI;
    // ユウコ
    public Sprite m_yukoFaceN_UI, m_yukoFaceD_UI;

    // 各プレイヤー名UI
    public Text[] m_plrNameUI;

    // 各プレイヤーの残機UI
    public Text[] m_plrStockUI;

    // 各プレイヤーの残機BGUI
    public Image[] m_plrStockBgUI;

    // タイマーUI
    public Text m_timerUI;

    // カウントダウンUI
    public Image m_countDownUI;
    public Image m_GoUI;

    // カウントダウン用スプライト
    public Sprite[] m_countSprite;

    // UI更新フラグ
    public bool m_bUpdateUI;
   
    //========================================================
    // リテラル
    //========================================================
    // プレイヤー関連
    // 各プレイヤーの残機数
    public int[] m_plrStock;
    // 各プレイヤーの名前
    public string[] m_plrName;
    // 各プレイヤーのボムの残機数
    public int[] m_plrBombStock;
    // 各プレイヤーの使用キャラ名
    public string[] m_useCharaName;
    // 各プレイヤーの死亡フラグ
    public bool[] m_dieFlag;
    // プレイヤー数
    public int m_maxPlayer;
    // 現在のゲームの状態
    public GAME_STATE m_currentState;
    // タイマー
    private Timer m_timer = new Timer();
    // プレイヤー情報更新フラグ
    public bool m_bUpdatePlayerData;
    // 各プレイヤーのキル数
    public int[] m_plrKill;
    // 各プレイヤーのデス数
    public int[] m_plrDeath;
    // 各プレイヤーの順位
    public int[] m_plrRank;
    //========================================================
    // 初期化処理
    //========================================================
    void Start() {

        // グローバルデータから各プレイヤーの情報を取得
        m_plrStock = GrobalData.Instance._plrStock;
        m_plrName = GrobalData.Instance._plrName;
        m_plrBombStock = GrobalData.Instance._plrBombStock;
        m_useCharaName = GrobalData.Instance._useCharaName;
        m_maxPlayer = GrobalData.Instance._plrCount;
        // 各プレイヤーの情報を初期化
        m_plrKill = new int[] { 0,0,0,0};
        m_plrDeath = new int[] { 0,0,0,0};
        m_plrRank = new int[] { 1, 2, 3, 4 };
        // フレームカウントを0にする
        FrameCount = 0;
        // 各プレイヤーの志望フラグをfalseにする
        m_dieFlag = new bool[] { false,false,false,false};
        // UIはまだ更新しない
        m_bUpdateUI = false;
        // プレイヤーデータは更新しない
        m_bUpdatePlayerData = false;

        //UIの初期化
        InitUI(m_maxPlayer);

        // 現在は初期化
        m_currentState = GAME_STATE.INIT;
    }
    //========================================================
    // 初期化処理はここまで
    //========================================================

    //========================================================
    // 更新処理
    //========================================================
    void Update() {

        // UIの更新
        UpdateUI();

        // 入力処理の更新
        UpdateInput();

        // プレイヤーデータの更新
        UpdatePlayerData();
    }
    //========================================================
    // 更新処理はここまで
    //========================================================

    //========================================================
    // UI処理
    //========================================================
    /// <summary>
    /// UIの初期化
    /// </summary>
    /// <param name="maxPlayer"> プレイヤー数</param>
    void InitUI(int maxPlayer) {

        //========================================================
        // プレイヤーUIの初期化
        //========================================================
        for (int i = 0 ; i < maxPlayer ; i++ ) {

            // 使用キャラの画像を選択し表示する
            switch (m_useCharaName[i]) {
                // ユニティちゃん
                case "UnityChan":
                    m_plrFaceUI[i].sprite = m_utcFaceN_UI;
                    break;
                // ミサキ
                case "Misaki":
                    m_plrFaceUI[i].sprite = m_misakiFaceN_UI;
                    break;
                // ユウコ
                case "Yuko":
                    m_plrFaceUI[i].sprite = m_yukoFaceN_UI;
                    break;
                default:
                    break;
            }

            // もし名前がなければ
            if(m_plrName[i] == "") {
                // プレイヤー(i+1)と表示する
                int j = i + 1;
                m_plrNameUI[i].text = "<b>Player" + j+"</b>";
                Debug.Log("名前なし");
            }else {
                // 名前をそのまま表示する
                m_plrNameUI[i].text =  "<b>"+m_plrName[i]+"</b>";
                Debug.Log(m_plrName[i]);
            }
            // 残機数を表示
            m_plrStockUI[i].text = "<b>" +new string('★',m_plrStock[i])+ "</b>";

            //UIを Active にする
            m_plrFaceUI[i].color = Color.white;
            m_plrNameUI[i].color = new Color(1.0f, 0.4f, 0.1f, 1.0f);
            m_plrStockUI[i].color = Color.yellow;
            m_plrStockBgUI[i].color = Color.white;
            print(m_plrNameUI[i].text);
        }
        //========================================================
        // プレイヤーUIの初期化はここまで
        //========================================================

        //========================================================
        // タイマーUIの初期化
        //========================================================
        // 制限時間を設定
        m_timer.LimitTime = LIMIT_TIME;
        // 終了関数を設定
        m_timer.FireDelegate = Finishi;
        m_timer.IsEnable = false;

        //========================================================
        // タイマーUIの初期化はここまで
        //========================================================

        //========================================================
        // カウントダウンUIの初期化
        //========================================================

        // 透明にする
        m_GoUI.color = TRANSPARENT_COLOR;
        // 不透明にする
        m_countDownUI.color = OPAQUE_COLOR;

        // カウントダウンコルーチン
        StartCoroutine(COUNTDOWN_COROUTINE);
        //========================================================
        // カウントダウンUIの初期化はここまで
        //========================================================
    }
    /// <summary>
    /// UIの更新
    /// </summary>
    void UpdateUI() {

        // タイマークラスの更新
        if (m_timer.Update()){

        }
        // タイマーUIの更新
        UpdateTimer();

        // UI更新フラグにtrueが入ったとき
        // 1度しか更新しないようにする
        if (m_bUpdateUI) {

            // 顔UIの更新
            UpdateFaceUI(m_maxPlayer);
            // プレイヤー残機UIの更新
            UpdatePlayerStockUI(m_maxPlayer);
            // falseを入れて2回以上更新しないようにする
            m_bUpdateUI = false;
        }
    }

    /// <summary>
    /// タイマーの更新
    /// </summary>
    void UpdateTimer() {
        
        //分と秒を設定 
        int minute = (int)m_timer.RemainingTime / 60;
        int second = (int)m_timer.RemainingTime % 60;
        
        // タイマーテキストに設定
        m_timerUI.text = "<b>" +minute.ToString("D1")+":"+second.ToString("D2")+ "</b>";
    }

    /// <summary>
    /// 顔UIの更新
    /// </summary>
    /// <param name="maxPlayer">プレイヤー数</param>
    void UpdateFaceUI(int maxPlayer) {

        // プレイヤー数分まわす
        for (int i = 0; i < maxPlayer ; i++) {

            // 死亡フラグがtrueなら
            if(m_dieFlag[i] == true) {

                // 使用キャラの画像をDieに変更し表示
                switch (m_useCharaName[i])
                {
                    // ユニティちゃん
                    case "UnityChan":
                        m_plrFaceUI[i].sprite = m_utcFaceD_UI;
                        break;
                    // ミサキ
                    case "Misaki":
                        m_plrFaceUI[i].sprite = m_misakiFaceD_UI;
                        break;
                    // ユウコ
                    case "Yuko":
                        m_plrFaceUI[i].sprite = m_yukoFaceD_UI;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    /// <summary>
    /// プレイヤー残機UIの更新
    /// </summary>
    /// <param name="maxPlayer"></param>
    void UpdatePlayerStockUI(int maxPlayer) {
        // プレイヤーの数だけ更新する
        for(int i = 0; i < maxPlayer; i++) {
            // 残機が１より少ないとき
            if(m_plrStock[i] < 1) {
                // 「KO」と表示する
                m_plrStockUI[i].text ="<b>     KO</b>";

            }else {
                // 残機の数だけ★を表示
                m_plrStockUI[i].text = "<b>" + new string('★', m_plrStock[i]) + "</b>";
            }
        }
    }
    /// <summary>
    /// // カウントダウンコルーチン
    /// </summary>
    IEnumerator CountDown() {

        for(int i = 0; i < 3; i++) {
            // 画像を切り替える
            m_countDownUI.sprite = m_countSprite[i];
            // １秒待つ
            yield return new WaitForSeconds(1.0f);
        }
        // 透明にする
        m_countDownUI.color = TRANSPARENT_COLOR;
        // 不透明にする
        m_GoUI.color = OPAQUE_COLOR;

        // 0.5秒待つ
        yield return new WaitForSeconds(0.5f);

        // 透明にする
        m_GoUI.color = TRANSPARENT_COLOR;

        // Updadeに移行
        m_currentState = GAME_STATE.UPDATE;

        // タイマーをスタートする
        m_timer.IsEnable = true;
    }

    //========================================================
    // UI処理はここまで
    //========================================================

    //========================================================
    // RPC処理
    //========================================================
    /// <summary>
    /// リザルトシーンを読み込む受信処理
    /// </summary>
    [MunRPC]
    void RecvLoadResultScene() {
        int count = 0;

        for(int i = 0 ; i < m_maxPlayer; i++) {
            if(m_dieFlag[i] == false) {

                GrobalData.Instance._plrWinFlag[i] = true;
                count++;
            }
        }
        if(count > 1) {
            for (int i = 0; i < m_maxPlayer; i++){

                if (GrobalData.Instance._plrWinFlag[i] == true){

                    GrobalData.Instance._plrWinFlag[i] = false;
                }
            }
            GrobalData.Instance._drawMatch = true;
        }
        // グローバルデータに保存
        GrobalData.Instance._plrKillScore = m_plrKill;
        GrobalData.Instance._plrDeathScore = m_plrDeath;
        GrobalData.Instance._plrRank = m_plrRank;
        if(m_maxPlayer == 1) { FadeManager.Instance.MonobitLoadLevel(TITLE_SCENE, 2.0f); }
        // リザルト画面に移動
        FadeManager.Instance.MonobitLoadLevel(RESULT_SCENE, 2.0f);
    }
    //========================================================
    // RPC処理はここまで
    //========================================================

    //========================================================
    // 入力処理
    //========================================================
    /// <summary>
    /// 入力処理の更新
    /// </summary>
    void UpdateInput() {
        // タイトルに戻る
        //BackTitle();
    }

    // デバッグ用
    /// <summary>
    /// タイトルに戻る
    /// </summary>
    void BackTitle() {
        // FIRE3ボタンが押されたとき
        if (Input.GetButtonDown(FIRE3_BUTTON)) {
            
            // サーバーに接続しているとき
            if (MonobitEngine.MonobitNetwork.isConnect){
                // サーバーから切断
                MonobitNetwork.DisconnectServer();
            }
            // タイトルシーンに戻る
            FadeManager.Instance.LoadLevel(TITLE_SCENE, 1.0f);
        }
    }

    //========================================================
    // 入力処理はここまで
    //========================================================

    //========================================================
    // プレイヤー情報更新処理
    //========================================================
    /// <summary>
    /// プレイヤー情報更新処理
    /// </summary>
    void UpdatePlayerData() {

        if (m_bUpdatePlayerData == true)  {

            // 生存しているプレイヤーの確認
            CheckPlayerStock();
            // falseを入れて2回以上更新しないようにする
            m_bUpdatePlayerData = false;
        }
    }
    /// <summary>
    /// 生存しているプレイヤーの確認
    /// </summary>
    void CheckPlayerStock() {
        int i = 0;
        for(int j = 0; j < m_maxPlayer ; j++ ) {

           if(m_dieFlag[j] == true) { i++; }
        }
        // 場にいるプレイヤーが１人以下のとき
        if( m_maxPlayer - i  < 2) {
            // ゲームを終了する。
            Finishi();
        }
    }

    //========================================================
    // プレイヤー情報更新処理はここまで
    //========================================================

    //========================================================
    // ゲーム終了処理
    //========================================================
    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    void Finishi() {

        if(m_currentState == GAME_STATE.FINISH) { return; }
        // タイマーを止める
        m_timer.IsEnable = false;
        // ゲームを終了する
        m_currentState = GAME_STATE.FINISH;
        // 音を再生
        AudioManager.Instance.PlaySE(AUDIO.SE_FINISH);
        // BGMをフェードアウトする
        AudioManager.Instance.FadeOutBGM();

        if (MonobitEngine.MonobitNetwork.isHost) {
            // コルーチンを開始
            StartCoroutine(LOADRESULT_COROUTINE);
        }
    }
    /// <summary>
    /// リザルトシーンを読み込む受信処理
    /// (ホストのみ)
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadResult() {

        // 2秒待つ
        yield return new WaitForSeconds(2.0f);
        if(m_maxPlayer == 1) {
            FadeManager.Instance.LoadLevel(TITLE_SCENE, 2.0f);
            // サーバーから切断する
            MonobitNetwork.DisconnectServer();
            yield break;
        }
        // リザルトシーンを読み込む送信処理
        monobitView.RPC(RPC_RECV_LOAD_RESULTSCENE, MonobitTargets.All, null);
    }
    //========================================================
    // ゲーム終了処理はここまで
    //========================================================

}
