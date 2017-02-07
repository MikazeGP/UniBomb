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

    // コルーチン
    private const string COUNTDOWN_COROUTINE = "CountDown";

    // カラー
    // 透明
    private Color TRANSPARENT_COLOR = new Color(1, 1, 1, 0);
    // 不透明
    private Color OPAQUE_COLOR = new Color(1, 1, 1, 1);


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

        // フレームカウントを0にする
        FrameCount = 0;
        // 各プレイヤーの志望フラグをfalseにする
        m_dieFlag = new bool[] { false,false,false,false};
        // UIはまだ更新しない
        m_bUpdateUI = false;

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

            }else {
                // 名前をそのまま表示する
                m_plrNameUI[i].text =  "<b>"+m_plrName[i]+"</b>";
            }
            // 残機数を表示
            m_plrStockUI[i].text = "<b>" +new string('★',m_plrStock[i])+ "</b>";

            //UIを Active にする
            m_plrFaceUI[i].color = Color.white;
            m_plrNameUI[i].color = new Color(1.0f, 0.4f, 0.1f, 1.0f);
            m_plrStockUI[i].color = Color.yellow;
            m_plrStockBgUI[i].color = Color.white;
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
        // m_timer.FireDelegate = ;
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

        for(int i = 0; i < maxPlayer; i++) {

            if(m_plrStock[i] < 1) {

                m_plrStockUI[i].text ="<b>     KO</b>";

            }else {

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

        m_currentState = GAME_STATE.UPDATE;
    }

    //========================================================
    // UI処理はここまで
    //========================================================

    //========================================================
    // UPC処理
    //========================================================

    //========================================================
    // UPC処理はここまで
    //========================================================

    //========================================================
    // 入力処理
    //========================================================
    /// <summary>
    /// 入力処理の更新
    /// </summary>
    void UpdateInput() {

        BackTitle();
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
    void PlayerDataUpdate() {

    }

    void DieCheck() {


    }

    //========================================================
    // プレイヤー情報更新処理はここまで
    //========================================================


}
