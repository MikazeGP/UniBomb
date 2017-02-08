using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 対戦結果を表示するクラス
/// </summary>
public class ResultManager : Origin {

    private enum GAME_STATE : int {

        INIT = 0,
        UPDATE,
        FINISH
    };

    //========================================================
    // 定数
    //========================================================
    // 選択時間
    private const int LIMIT_TIME = 10;
    // コルーチン
    private const string WAIT_TIME_COROUTINE = "WaitTime";

    //========================================================
    // UI関連
    //========================================================
    // 各プレイヤーの顔UI
    public Image[] m_plrFaceUI;

    // キャラクターの顔のスプライト
    // ユニティちゃん
    public Sprite m_utcFaceG_UI, m_utcFaceB_UI;
    // ミサキ
    public Sprite m_misakiFaceG_UI, m_misakiFaceB_UI;
    // ユウコ
    public Sprite m_yukoFaceG_UI, m_yukoFaceB_UI;

    // 各プレイヤー名UI
    public Text[] m_plrNameUI;
    // 再戦UI
    public GameObject m_rematchUI;
    // タイマーUI
    public Text m_timerUI;
    //========================================================
    // リテラル
    //========================================================
    // 各プレイヤーの名前
    private string[] m_plrName;
    // 各プレイヤーの使用キャラ名
    private string[] m_useCharaName;
    // プレイヤー数
    public int m_maxPlayer;
    // 勝利フラグ
    public bool m_winFlag;
    // 再戦フラグ
    public bool[] m_rematchFlag;
    // タイマー
    private Timer m_timer = new Timer();

    //========================================================
    // 初期化処理
    //========================================================
    void Start(){

        // グローバルデータから各プレイヤーの情報を取得
        m_plrName = GrobalData.Instance._plrName;
        m_useCharaName = GrobalData.Instance._useCharaName;
        m_maxPlayer = GrobalData.Instance._plrCount;
        m_winFlag = GrobalData.Instance._plrWinFlag[PlayerId];

        m_rematchFlag = new bool[] { false,false,false,false};
        //UIの初期化
        InitUI(m_maxPlayer);
    }
    //========================================================
    // 初期化処理はここまで
    //========================================================

    //========================================================
    // 更新処理
    //========================================================
    void Update(){

        // UIの更新
        UpdateUI();
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
        for (int i = 0; i < maxPlayer; i++) {

            switch (m_useCharaName[i]){

                // ユニティちゃん
                case "UnityChan":
                    if (m_winFlag) { m_plrFaceUI[i].sprite = m_utcFaceG_UI;  return; }
                    m_plrFaceUI[i].sprite = m_utcFaceB_UI; 
                    break;
                // ミサキ
                case "Misaki":
                    if (m_winFlag) { m_plrFaceUI[i].sprite = m_misakiFaceG_UI; return; }
                    m_plrFaceUI[i].sprite = m_misakiFaceB_UI;
                    break;
                // ユウコ
                case "Yuko":
                    if (m_winFlag) { m_plrFaceUI[i].sprite = m_yukoFaceG_UI; return; }
                    m_plrFaceUI[i].sprite = m_yukoFaceB_UI;
                    break;
                default:
                    break;
            }

            // もし名前がなければ
            if (m_plrName[i] == ""){

                // プレイヤー(i+1)と表示する
                int j = i + 1;
                m_plrNameUI[i].text = "<b>Player" + j + "</b>";

            }else {

                // 名前をそのまま表示する
                m_plrNameUI[i].text = "<b>" + m_plrName[i] + "</b>";
            }
            //UIを Active にする
            m_plrFaceUI[i].color = Color.white;
            m_plrNameUI[i].color = new Color(1.0f, 0.4f, 0.1f, 1.0f);
        }
        //========================================================
        // プレイヤーUIの初期化はここまで
        //========================================================

        //========================================================
        // 再戦UIの初期化
        //========================================================
        
        // 非表示にする
        m_rematchUI.SetActive(false);

        //========================================================
        // 再戦UIの初期化ここまで
        //========================================================

        //========================================================
        // タイマーUIの初期化
        //========================================================
        // 制限時間を設定
        m_timer.LimitTime = LIMIT_TIME;

        m_timer.IsEnable = false;

        StartCoroutine(WAIT_TIME_COROUTINE);
        //========================================================
        // タイマーUIの初期化はここまで
        //========================================================
    }
    /// <summary>
    /// UIの更新
    /// </summary>
    void UpdateUI() {

        if (m_timer.Update()){

        }

        // タイマーの更新
        UpdateTimer();

    }

    /// <summary>
    /// タイマーの更新
    /// </summary>
    void UpdateTimer(){

        //分と秒を設定 
        int second = (int)m_timer.RemainingTime % 60;

        // タイマーテキストに設定
        m_timerUI.text = "<b>" + second+ "</b>";
    }
    /// <summary>
    /// 3秒待つ
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitTime() {

        yield return new WaitForSeconds(3.0f);

        //表示にする
        m_rematchUI.SetActive(true);
        // タイマーを起動
        m_timer.IsEnable = true;
    }

    //========================================================
    // UI処理はここまで
    //========================================================
}
