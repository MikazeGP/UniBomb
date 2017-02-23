using UnityEngine;
using System.Collections;

public class GrobalData : MonoBehaviour {

    //======================================
    // ここから Singleton 設定
    //======================================

    // 共有インスタンス
    public static GrobalData Instance;

    // ゾーンを移動した際にオブジェクトが生成された際に呼び出される
    void Awake()
    {

        // 共有インスタンスが存在しているかどうかのチェック
        if (Instance == null)
        {

            // ロードされた際にゲームオブジェクトを破棄しない
            DontDestroyOnLoad(gameObject);

            // 共有インスタンスに自身を設定
            Instance = this;

        }
        else if (Instance != this)
        {

            // 共有インスタンスが自身でなければゲームオブジェクトを破棄
            Destroy(gameObject);
        }
    }

    //======================================
    // ここまで Singleton 設定
    //======================================

    //======================================
    // ここからゲーム全体で共有したいメンバの宣言
    //======================================

    // ゲーム開始かどうか true...ゲームが開始している
    bool m_initFlag = false;

    // プレイヤーのステータス

    // 自機のステータス

    // 使用するプレイヤーの名前
    public string _plrCharaName = "";

    // プレイヤー数
    public int _plrCount = 4;

    // 各プレイヤーの残機
    public int[] _plrStock = new int[] { 3, 3, 3, 3 };

    // 各プレイヤーの名前
    public string[] _plrName = new string[] {"","","","" };

    // 各プレイヤーの使用キャラ名
    public string[] _useCharaName = new string[] { "", "", "", "" };

    // 各プレイヤーのボムの残機
    public int[] _plrBombStock = new int[] {0,0,0,0 };

    // 各プレイヤーのスコア
    public int[] _plrScore = new int[] { 0, 0, 0, 0};

    // 各プレイヤーのキル
    public int[] _plrKillScore = new int[] { 0, 0, 0, 0 };

    // 各プレイヤーのデス
    public int[] _plrDeathScore = new int[] { 0, 0, 0, 0 };

    // 各プレイヤーの勝利フラグ
    public bool[] _plrWinFlag = new bool[] { false, false, false, false };

    // 各プレイヤーの順位
    public int[] _plrRank = new int[] { 1, 2, 3, 4 };

    // 引き分けフラグ
    public bool _drawMatch;

    // オーディオ関連
    // 音量
    // BGM
    public float _currentBGMVolume = 0.5f;
    // SE
    public float _currentSEVolume = 0.5f;
    // Voice
    public float _currentVoiceVolume = 0.5f;
}
