using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MonobitEngine;

/// <summary>
/// 対戦結果を表示するクラス
/// </summary>
public class ResultManager : Origin {

    //========================================================
    // 定数
    //========================================================
    // RPC
    private const string RPC_RECVGAMECONTINUE = "RecvGameContinue";
    private const string RPC_CHECK = "Check";
    // コルーチン
    // 再戦オブジェクトを表示
    private const string COROUTINE_ACTIVE_REMATCH_OBJECT = "ActiveRematchObject";
    // 2秒待つ
    private const string COROUTINE_CONTINUE = "Continue";
    // 3秒待つ
    private const string COROUTINE_END = "End";

    // マーカーポジション
    private Vector2 MARKER_POSITION0 = new Vector2(0, -335);
    private Vector2 MARKER_POSITION1 = new Vector2(256, -335);

    //========================================================
    // UI関連
    //========================================================
    // プレイヤーUIオブジェクト
    public GameObject[] m_playerUIObject;
    // 継続UIオブジェクト
    public GameObject m_rematchUI;
    // 選択マーカー
    public GameObject m_selectMarker;
    // プレイヤー顔画像
    public Image[] m_playerFaceUI;
    // プレイヤー名UIテキスト
    public Text[] m_playerNameTextUI;
    // プレイヤーキルテキスト
    public Text[] m_playerKillTextUI;
    // プレイヤーデステキスト
    public Text[] m_playerDeathTextUI;
    // プレイヤー順位テキスト
    public Text[] m_playerRankTextUI;
    // 待機テキスト
    public Text m_waitTextUI;
    // カウントダウンテキスト
    public Text m_countDownTextUI;
    //========================================================
    // リテラル
    //========================================================
    // プレイヤー数
    private int m_maxPlayer;
    // プレイヤー使用キャラ
    private string[] m_useCharaName;
    // プレイヤー名
    private string[] m_playerName;
    // プレイヤーキル数
    private int[] m_playerKill;
    // プレイヤーデス数
    private int[] m_playerDeath;
    // プレイヤーの順位
    private int[] m_playerRank;
    // プレイヤーの継続フラグ
    private bool[] m_playerGameContinue;
    // 各プレイヤーはキャラを決定したか true..決定済み false..選択中
    private bool[] m_plrDecided;
    // 勝利フラグ
    private bool[] m_playerWinFlag;
    // タイマー
    private Timer m_timer;
    private int m_selectNum;
    private bool m_canPush;
    private bool m_enter;
    //========================================================
    // 初期化処理
    //========================================================
    void Start(){
        // グローバルデータから各プレイヤーの情報を取得
        m_maxPlayer = GrobalData.Instance._plrCount;
        m_useCharaName = GrobalData.Instance._useCharaName;
        m_playerName = GrobalData.Instance._plrName;
        m_playerKill = GrobalData.Instance._plrKillScore;
        m_playerDeath = GrobalData.Instance._plrDeathScore;
        m_playerRank = GrobalData.Instance._plrRank;
        m_playerWinFlag = GrobalData.Instance._plrWinFlag;
        m_playerGameContinue = new bool[] { false,false,false,false};
        m_plrDecided = new bool[] { false, false, false, false };
        m_canPush = false;
        m_enter = false;
        // UIの初期化
        this.InitUI(m_maxPlayer);
        AudioManager.Instance.PlayBGM(AUDIO.BGM_RESULT);
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
        // 
        DecidedCheck();
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
        Debug.Log("初期化開始");
        //========================================================
        // プレイヤーUIの初期化
        //========================================================
        for(int i = 0; i < maxPlayer ; i++) {

            SetFace sf = new SetFace();

            m_playerUIObject[i].SetActive(true);
            m_playerFaceUI[i].sprite = sf.SetFaceSprite(m_useCharaName[i], m_playerWinFlag[i]);
            m_playerNameTextUI[i].text = m_playerName[i];
            m_playerDeathTextUI[i].text = m_playerDeath[i].ToString();
            m_playerKillTextUI[i].text = m_playerKill[i].ToString();
            if (m_playerWinFlag[i]) { m_playerRankTextUI[i].text = "<b>WIN</b>"; }
            else { m_playerRankTextUI[i].text = "<b>LOSE</b>"; }

        }

        //========================================================
        // プレイヤーUIの初期化はここまで
        //========================================================

        //========================================================
        // 再戦UIの初期化
        //========================================================
        // 非表示にする
        m_rematchUI.SetActive(false);
        m_waitTextUI.color = TRANSPARENT_COLOR;
        m_countDownTextUI.color = TRANSPARENT_COLOR;
        m_selectMarker.SetActive(false);
        //========================================================
        // 再戦UIの初期化ここまで
        //========================================================

        //========================================================
        // タイマーUIの初期化
        //========================================================
        m_timer = new Timer();
        // 時間を設定
        m_timer.LimitTime = 10;
        // 終了処理を設定
        //m_timer.FireDelegate =
        // まだ更新しない
        m_timer.IsEnable = false;

        //========================================================
        // タイマーUIの初期化はここまで
        //========================================================
        StartCoroutine(COROUTINE_END);
        m_waitTextUI.color = OPAQUE_COLOR;
        Debug.Log("初期化終了");
    }
    /// <summary>
    /// UIの更新
    /// </summary>
    void UpdateUI() {

        // タイマークラスの更新
        if (m_timer.Update()){

        }
        // タイマーの更新
        UpdateTimer();

        // セレクトUIの更新
        //SelectUI();
    }
    /// <summary>
    /// タイマーの更新
    /// </summary>
    void UpdateTimer() {
        // 秒を設定
        int second = (int)m_timer.RemainingTime % 60;
        // タイマーテキストに設定
         m_countDownTextUI.text = "<b>" +second.ToString("D2") + "</b>";
    }
    /// <summary>
    /// セレクトUIの更新
    /// </summary>
    /*
    void SelectUI() {

        switch (SelectNum()) {

            case 0:
                m_selectMarker.GetComponent<RectTransform>().anchoredPosition = MARKER_POSITION0;
                if (Input.GetButtonDown(FIRE1_BUTTON)){
                    this.GameContinue();
                }
                break;
            case 1:
                m_selectMarker.GetComponent<RectTransform>().anchoredPosition = MARKER_POSITION1;
                if (Input.GetButtonDown(FIRE1_BUTTON)){
                    this.GameEnd();
                }
                break;
            default:
                break;
        }

    }*/

    // 再戦オブジェクトを表示
    IEnumerator ActiveRematchObject() {
        // 2秒待つ
        yield return new WaitForSeconds(2.0f);
        // UIを表示
        //m_rematchUI.SetActive(true);
        //m_countDownTextUI.color = OPAQUE_COLOR;
        //m_selectMarker.SetActive(true);
        // タイマーを起動
        //m_timer.IsEnable = true;
    }

    //========================================================
    // UI処理はここまで
    //========================================================

    //========================================================
    // 入力処理
    //========================================================
    /// <summary>
    /// 入力処理の更新
    /// </summary>
    void UpdateInput() {
       
    }
    /*
    int SelectNum() {

        if (Mathf.Abs(Input.GetAxis(AXIS_HORIZONTAL)) < 0.1f){

            m_canPush = true;
        }
        if (Mathf.Abs(Input.GetAxis(AXIS_HORIZONTAL)) > 0.2f && m_canPush) {

            if (Input.GetAxis(AXIS_HORIZONTAL) > 0){

                m_selectNum++;

            } else{

                m_selectNum--;
            }

            m_selectNum = Mathf.Clamp(m_selectNum, 0, 1);

            //　ここで再生
            AudioManager.Instance.PlaySE(AUDIO.SE_SELECT_SE);

            m_canPush = false;

            return m_selectNum;
        }
        return m_selectNum;
    }*/

    public void GameContinue() {
        // SEを再生
        AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
        // ゲームの継続意思を送信
        monobitView.RPC(RPC_RECVGAMECONTINUE, MonobitTargets.All, PlayerId, true);
        // 非表示にする
        m_rematchUI.SetActive(false);
        m_waitTextUI.color = TRANSPARENT_COLOR;
        m_selectMarker.SetActive(false);
        // 表示にする
        m_waitTextUI.color = Color.black;
        //　タイマーをストップ
        m_timer.IsEnable = false;
    }
    public void GameEnd() {
        // SEを再生
        AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
        // ゲームの継続意思を送信
        monobitView.RPC(RPC_RECVGAMECONTINUE, MonobitTargets.All, PlayerId, false);
        // 非表示にする
        m_rematchUI.SetActive(false);
        m_waitTextUI.color = TRANSPARENT_COLOR;
        m_selectMarker.SetActive(false);
        // 表示にする
        m_waitTextUI.color = Color.black;
        //　タイマーをストップ
        m_timer.IsEnable = false;
    }

    //========================================================
    // 入力処理はここまで
    //========================================================

    //========================================================
    // UPC処理
    //========================================================
    [MunRPC]
    /// <summary>
    /// プレイヤーのゲームの継続意思
    /// </summary>
    /// <param name="id">プレイヤーID</param>
    /// <param name="con">継続するかしないか</param>
    void RecvGameContinue(int id,bool con) {

        m_playerGameContinue[id] = con;
        m_plrDecided[id] = true;
    }
    /// <summary>
    /// 再戦するかチェックする
    /// </summary>
    [MunRPC]
    void Check() {

        for(int j = 0; j < m_maxPlayer; j++) {

            if(m_playerGameContinue[j] == false) {

                StartCoroutine(COROUTINE_END);
                return;

            }else {

                StartCoroutine(COROUTINE_CONTINUE);
                return;
            }
        }
    }
    /// <summary>
    /// ゲーム継続処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Continue() {

        // テキストを変更
        m_waitTextUI.text = "<b>再戦が決定しました。</b>";
        // 2秒待つ
        yield return new WaitForSeconds(2.0f);
        // テキストを変更
        m_waitTextUI.text = "<b>3秒後ロビーに戻ります。</b>";
        // 3秒待つ
        yield return new WaitForSeconds(3.0f);
        // タイトル画面に戻る
        FadeManager.Instance.MonobitLoadLevel(CHATROOM_SCENE, 1.0f);


    }
    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    /// <returns></returns>
    IEnumerator End() {

        // テキストを変更
        m_waitTextUI.text = "<b>プレイしてくれてありがとう！</b>";
        // 4秒待つ
        yield return new WaitForSeconds(4.0f);
        // テキストを変更
        m_waitTextUI.text = "<b>10秒後タイトル画面に戻ります</b>";
        // 5秒待つ
        yield return new WaitForSeconds(5.0f);
        m_waitTextUI.text = "<b>再戦したいときは、再度部屋を作ってください</b>";
        // 5秒待つ
        yield return new WaitForSeconds(5.0f);
        // サーバーから切断する
        MonobitNetwork.DisconnectServer();
        // タイトル画面に戻る
        FadeManager.Instance.MonobitLoadLevel(TITLE_SCENE, 1.0f);
    }
    //========================================================
    // UPC処理はここまで
    //========================================================
    //========================================================
    // 状態更新
    //========================================================
    void DecidedCheck() {

        if (!MonobitEngine.MonobitNetwork.isHost || m_enter) { return; }

        for (int i = 0; i < m_maxPlayer; i++)
        {
            if (m_plrDecided[i] == false) { return; }
        }
        monobitView.RPC(RPC_CHECK, MonobitTargets.All, null);
        m_enter = true;
    }

    //========================================================
    // 状態更新はここまで
    //========================================================
}
