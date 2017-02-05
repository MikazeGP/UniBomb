using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MonobitEngine;

/// <summary>
/// ゲームメインで各プレイヤーの情報を
/// 管理するクラス
/// </summary>
public class GameMgr : Origin {

    
    public Text _timerText,_stockText;
    public Timer time = new Timer();
    public Camera _camera;
    public int plrnum;
    private bool startAudio;
    //public bool gameStart;
    // 各プレイヤーの残機数
    public int[] m_plrBombStock;
    // 各プレイヤーの名前
    public string[] m_plrName;
    

    // 初期化処理
    void Start () {


        m_plrBombStock = GrobalData.Instance._plrBombStock;
        m_plrName = GrobalData.Instance._plrName;

        // 制限時間は300秒に設定する。
        time.LimitTime = 300.0f;
        // 終了関数を指定する。
        time.FireDelegate = Finish;
        time.IsEnable = false;
        
        startAudio = false;
        //gameStart = false;
    }
	
	// 更新処理
	void Update () {

        //_stockText.text = "<b>残機　" + new string('★', GrobalData.Instance._plrStock[PlayerId]) + "</b>";

        if (time.Update()) {
            
        }

        PlayAudio();

        this.SetTimer();

        if (Input.GetKey(KeyCode.Escape)) { FadeManager.Instance.LoadLevel(TITLE_SCENE, 1.0f); }

        if (!MonobitEngine.MonobitNetwork.isHost){
            return;
        }

        if (Input.GetButtonDown(FIRE3_BUTTON) || Input.GetKey(KeyCode.S) && time.IsEnable == false) {

            time.IsEnable = true;
            //gameStart = true;
        }
	}

    // タイマー設定
    void SetTimer(){

        // 分と秒を設定
        int minute = (int)time.RemainingTime / 60;
        int second = (int)time.RemainingTime % 60;

        // テキストに代入
        _timerText.text = "<b>"+minute.ToString("D2") + ":" + second.ToString("D2")+"</b>";
    }

    void PlayAudio() {

        if(time.IsEnable == true && startAudio == false) {

            print("In");

            // ここで音を再生
            AudioManager.Instance.PlayBGM(AUDIO.BGM_BATTLE1, AudioManager.BGM_FADE_SPEED_RATE_HIGH);
            startAudio = true;
        }
    }

    void Finish() {

        // GrobalDataに結果を反映

        // リザルトシーンに転移
    }

}
