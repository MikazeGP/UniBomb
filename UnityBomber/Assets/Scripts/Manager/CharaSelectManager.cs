using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Serialization;
using MonobitEngine;

/// <summary>
/// キャラ選択を管理するクラス
/// </summary>
public class CharaSelectManager : Origin {

    //========================================================
    // 定数
    //========================================================

    // RPC
    // ステージセレクトに移動
    private const string LOAD_STAGE_SELECT = "LoadStageSelect";
    // キャラの選択or未選択を受け取る
    private const string RECEIVE_CHARACTER_SELECT = "RecvCharaSelected";

    // アニメータのパラメータ名
    private const string ANIPARAM_PLRNUM = "PlrNum";

    //========================================================
    // UI関連
    //========================================================

    // 使用するキャラの画像
    public RawImage r_kohaku, r_misaki, r_yuko;

    // 二つ名、キャラ名、キャラの解説テキスト
    public Text t_anotherName, t_charaName, t_kaisetsu;

    // 各パラメータのスライダー
    public Slider s_bombStock, s_power, s_speed;

    // 各パラメータのテキスト
    public Text t_bombStockParam, t_powerParam, t_speedParam;

    // 各プレイヤのキャラ選択チェック
    public Text[] t_decideCheck;

    //========================================================
    // アニメータ
    //========================================================
    // アニメータ
    public Animator a_selectAni;

    //========================================================
    // リテラル
    //========================================================

    // プレイヤーのステータス
    // ボムの残機数
    private int m_bombStock;
    // ボムの火力
    private int m_power;
    // 移動速度
    private int m_speed;
    // キャラの名前
    private string m_charaName;
    // キャラの番号
    public int m_charaNum;

    // UI処理関連
    // 更新をストップするか true..停止 false..更新
    private bool stop;
    // ボタンを押すことができるか true..可能 false..不可能
    private  bool m_canPush;

    // RPC関連
    // プレイヤー数
    [SerializeField]
    private int maxPlayer;

    // キャラクターを決定したか true..決定済み false..選択中
    private bool m_decided;
    // 各プレイヤーはキャラを決定したか true..決定済み false..選択中
    [SerializeField]
    private bool[] m_plrDecided;
    // 各プレイヤーの使用キャラ
    [SerializeField]
    private string[] m_plrUseChara;

    // シーン移動フラグ
    private bool m_loadScene;


    //========================================================
    // 初期化処理
    //========================================================

    void Start() {

        // 初期は１にする
        m_charaNum = 1;
        // フレームカウントを0にする
        FrameCount = 0;
        // 更新する
        stop = false;
        // キャラを選択中
        m_decided = false;
        // 各プレイヤーはキャラを選択中
        m_plrDecided = new bool[] {false,false,false,false };
        // 使用キャラは選択中
        m_plrUseChara = new string[] {"","","","" };

        // プレイヤー数をグローバルデータから取得
        maxPlayer = GrobalData.Instance._plrCount;

        // シーンは読み込んでいない
        m_loadScene = false;

        // サーバーに接続していない場合
        // 以下の処理をしない
        if (!MonobitNetwork.isConnect)
        {
            return;
        }

        // UIにも反映
        for (int i = 0; i < maxPlayer; i++) {

            t_decideCheck[i].text = (i + 1) + "P:✖";
        }

    }

    //========================================================
    // 初期化処理はここまで
    //========================================================

    //========================================================
    // 更新処理
    //========================================================
    void Update() {

        // パラメータを反映
        ChangeParamater(CharaNum());

        // フレームカウントを更新
        FrameCount++;

        // フェードの関係上,
        // 60フレームになってから操作できるようにする
        if(FrameCount >= 60) {
            
            // 入力情報を取得
            CheckState();

            // UIを更新
            UIState();
        }
    }

    //========================================================
    // 更新処理はここまで
    //========================================================

    /// <summary>
    /// キャラのパラメータ更新処理
    /// </summary>
    /// <param name="num">キャラの番号</param>
    void ChangeParamater(int num) {


        switch (num) {

            // Unityちゃん
            case 1:

                t_anotherName.text = "<b>ユニティちゃん</b>";
                t_charaName.text = "<b>大鳥 こはく</b>";
                t_kaisetsu.text = "バランス型\n弱点がなく、柔軟な立ち回りが可能\n初心者におすすめ";
                m_charaName = "UnityChan";
                a_selectAni.SetInteger(ANIPARAM_PLRNUM, 1);
                t_bombStockParam.text = "<b>3</b>";
                t_powerParam.text = "<b>3</b>";
                t_speedParam.text = "<b>3</b>";
                r_kohaku.rectTransform.position = new Vector3(225, 334, 0);
                r_misaki.rectTransform.position = new Vector3(-230, 334, 0);
                r_yuko.rectTransform.position = new Vector3(-260, 334, 0);
                m_bombStock = 3;

                break;
            // みさき
            case 2:
                t_anotherName.text = "<b>生徒会長</b>";
                t_charaName.text = "<b>藤原 みさき</b>";
                t_kaisetsu.text = "高速移動型\n移動速度は全キャラ中最速\nただしボムは２つしか持てないため\n回避がメイン。中級者向け";
                m_charaName = "Misaki";
                a_selectAni.SetInteger(ANIPARAM_PLRNUM, 2);
                t_bombStockParam.text = "<b>2</b>";
                t_powerParam.text = "<b>3</b>";
                t_speedParam.text = "<b>4</b>";
                r_kohaku.rectTransform.position = new Vector3(-225, 334, 0);
                r_misaki.rectTransform.position = new Vector3(230, 334, 0);
                r_yuko.rectTransform.position = new Vector3(-260, 334, 0);
                m_bombStock = 2;
                break;
            // ゆうこ
            case 3:
                t_anotherName.text = "<b>マイペースなゲーマー</b>";
                t_charaName.text = "<b>神林 ゆうこ</b>";
                t_kaisetsu.text = "高火力型\nボムの火力、所持数は最高だが\n移動速度は最遅。上級者向け";
                m_charaName = "Yuko";
                a_selectAni.SetInteger(ANIPARAM_PLRNUM, 3);
                t_bombStockParam.text = "<b>4</b>";
                t_powerParam.text = "<b>4</b>";
                t_speedParam.text = "<b>1</b>";
                r_kohaku.rectTransform.position = new Vector3(-225, 334, 0);
                r_misaki.rectTransform.position = new Vector3(-230, 334, 0);
                r_yuko.rectTransform.position = new Vector3(260, 334, 0);
                m_bombStock = 4;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 入力に応じてキャラ番号を変更
    /// 1～3の値が入る
    /// </summary>
    /// <returns>キャラの番号</returns>
    int CharaNum() {

        // trueなら番号を変更せず
        // 下記の処理は行わない
        if (stop == true){

            return m_charaNum;
        }


        // X軸方向にキーが押されたとき
        if (Mathf.Abs(Input.GetAxis(AXIS_HORIZONTAL)) < 0.1f) {

            // 押すことができる
            m_canPush = true;
        }

        // X軸方向に押され、 canPush が true なら
        if(Mathf.Abs(Input.GetAxis(AXIS_HORIZONTAL)) > 0.2f && m_canPush) {

            // 押されたキーが正なら
            if (Input.GetAxis(AXIS_HORIZONTAL) > 0) {

                // キャラの番号を１増やす
                m_charaNum++;

             // 押されたキーが負なら
            } else {

                // キャラの番号を１減らす
                m_charaNum--;
            }

            // 3より大きいとき、1にする
            if (m_charaNum > 3) { m_charaNum = 1; }
            // 1より小さいとき、3にする
            else if(m_charaNum < 1) { m_charaNum = 3; }

            // SEを再生
            AudioManager.Instance.PlaySE(AUDIO.SE_SELECT_SE);

            // 押せなくする
            m_canPush = false;

            // キャラの番号を返す
            return m_charaNum;
        }

        // 何も押されてないなら
        // 現在のキャラの番号をそのまま返す
        return m_charaNum;
    }

    /// <summary>
    /// ボタンの状態を更新する
    /// </summary>
    void CheckState() {

        // 使用キャラ未選択で決定キーが押されたとき
        if (Input.GetButtonDown(FIRE1_BUTTON) && m_decided == false) {

            // trueなら下記の処理は行わない
            if (stop == true){

                return;
            }

            // SEを再生
            //AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);

            // グローバルデータにキャラの名前を保存
            GrobalData.Instance._plrCharaName = m_charaName;

            // BGMをフェードアウトする
            //AudioManager.Instance.FadeOutBGM(AudioManager.BGM_FADE_SPEED_RATE_HIGH);

            // 更新を止める
            stop = true;

            // キャラは選択済み
            m_decided = true;

            if (MonobitNetwork.inRoom) {

                // 現在選択しているキャラを送信してキャラを選択済みであることを送信する
                monobitView.RPC(RECEIVE_CHARACTER_SELECT, MonobitTargets.All, MonobitNetwork.player.ID, m_charaName,m_bombStock,true);
            }

        }
        // キャンセルボタンが押されたとき
        if (Input.GetButtonDown(FIRE2_BUTTON)) {

            // ルームに接続している時
            if (MonobitNetwork.inRoom && m_decided == true){

                // SEを再生
                AudioManager.Instance.PlaySE(AUDIO.SE_CANSEL);

                // BGMをフェードアウトする
                //AudioManager.Instance.FadeOutBGM(AudioManager.BGM_FADE_SPEED_RATE_HIGH);

                // 更新を開始する
                stop = false;

                // キャラは未選択にする
                m_decided = false;

                // 現在選択しているキャラを送信してキャラを選択済みであることを送信する
                monobitView.RPC(RECEIVE_CHARACTER_SELECT, MonobitTargets.All, MonobitNetwork.player.ID, "",0,false);

            }

            // ルームに未接続の時
            if (!MonobitNetwork.inRoom) {

                // SEを再生
                AudioManager.Instance.PlaySE(AUDIO.SE_CANSEL);
                // 更新を止める
                stop = true;
                // モードセレクトに戻る
                FadeManager.Instance.LoadLevel(MODESELECT_SCENE, 1.0f);
                // BGMを変更し、フェードアウトする
                AudioManager.Instance.PlayBGM(AUDIO.BGM_SELECT_BGM, AudioManager.BGM_FADE_SPEED_RATE_HIGH);
            }
        }
        // 全員がキャラクターを選択し、ホストなら
        if (CheckDecided() && MonobitEngine.MonobitNetwork.isHost && m_loadScene == false)
        {
            
            monobitView.RPC(LOAD_STAGE_SELECT, MonobitTargets.All, null);

            GrobalData.Instance._useCharaName = m_plrUseChara;

            m_loadScene = true;
        }
    }
    /// <summary>
    ///   各プレイヤのキャラ選択チェック
    /// </summary>
    void UIState() {

        for(int i = 0; i < maxPlayer; i++){

            if(m_plrDecided[i] == true) {
                // キャラ選択済み
                t_decideCheck[i].text = (i + 1) + "P:OK";

            }else{
                //　キャラ未選択
                t_decideCheck[i].text = (i + 1) + "P:✖";
            }
        }
    }
    
    [MunRPC]
    /// <summary>
    /// ステージセレクトに移動
    /// </summary>
    private void LoadStageSelect() {

        FadeManager.Instance.MonobitLoadLevel(STAGE_SELECT_SCENE, 1.0f);
    }

    [MunRPC]
    /// キャラの選択or未選択を受け取る。
    private void RecvCharaSelected(int plrID,string charaName,int bombstock,bool decided) {

        
        m_plrDecided[plrID - 1] = decided;

        m_plrUseChara[plrID - 1] = charaName;

        // グローバルデータにボム最大所持を代入
        GrobalData.Instance._plrBombStock[plrID - 1] = bombstock;

        // SEを再生
        AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);


    }

    /// 全員がキャラを選択したか
    /// true..選択済み false..未選択
    bool CheckDecided() {

        for (int i = 0; i < maxPlayer; i++) {

            if(m_plrDecided[i] == false) {

                return false;
            }

        }
        return true;
    }

}
