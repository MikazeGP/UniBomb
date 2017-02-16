using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 対戦結果を表示するクラス
/// </summary>
public class ResultManager : Origin {

    //========================================================
    // 定数
    //========================================================
    // コルーチン
    //private const string
    // 順位
    private const int RANK1 = 1;
    private const int RANK2 = 2;
    private const int RANK3 = 3;
    private const int RANK4 = 4;

    //========================================================
    // UI関連
    //========================================================
    // プレイヤーUIオブジェクト
    public GameObject[] m_playerUIObject;

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
    // 勝利フラグ
    private bool[] m_playerWinFlag;
    //========================================================
    // 初期化処理
    //========================================================
    void Start(){

        m_maxPlayer = GrobalData.Instance._plrCount;
        m_useCharaName = GrobalData.Instance._useCharaName;
        m_playerName = GrobalData.Instance._plrName;
        m_playerKill = GrobalData.Instance._plrKillScore;
        m_playerDeath = GrobalData.Instance._plrDeathScore;
        m_playerWinFlag = GrobalData.Instance._plrWinFlag;


        this.InitUI(m_maxPlayer);

    }
    //========================================================
    // 初期化処理はここまで
    //========================================================

    //========================================================
    // 更新処理
    //========================================================
    void Update(){

        
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

        }

        //========================================================
        // プレイヤーUIの初期化はここまで
        //========================================================

        //========================================================
        // 再戦UIの初期化
        //========================================================
        
        

        //========================================================
        // 再戦UIの初期化ここまで
        //========================================================

        //========================================================
        // タイマーUIの初期化
        //========================================================
        
        //========================================================
        // タイマーUIの初期化はここまで
        //========================================================
        Debug.Log("初期化終了");
    }
    /// <summary>
    /// UIの更新
    /// </summary>
    void UpdateUI() {

    }

    //========================================================
    // UI処理はここまで
    //========================================================
}
