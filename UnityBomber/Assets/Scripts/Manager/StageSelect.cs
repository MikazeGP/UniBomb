using UnityEngine;
using System.Collections;
using MonobitEngine;
using UnityEngine.UI;

/// <summary>
/// ステージ選択を管理するクラス
/// </summary>
public class StageSelect : Origin {

    //========================================================
    // 定数
    //========================================================
    private const string LOAD_STAGE = "LoadStage";

    //========================================================
    // UI関連
    //========================================================
    public Image m_image;
    public Sprite m_stg10, m_stg11,m_stg12,m_stg20;

    public Text t_stageName, t_commentary;

    //========================================================
    // リテラル
    //========================================================
    private int m_stageNum;
    private bool m_canPush;
    private bool m_stop;
    private bool m_decided;

    //========================================================
    // 初期化処理
    //========================================================
    void Start() {

        m_stageNum = 1;
        FrameCount = 0;
        m_stop = false;

    }
    //========================================================
    // 初期化処理はここまで
    //========================================================

    //========================================================
    // 更新処理
    //========================================================
    void Update() {

        // ホスト以外は処理をしない
        if (!MonobitEngine.MonobitNetwork.isHost && MonobitNetwork.inRoom)
        {
            return;
        }
        FrameCount++;

        ChangeStage(StageNum());

        if(FrameCount >= 65) {

            if(m_stop == true) {

                return;
            }

            CheckState();
        }
    }
    //========================================================
    // 更新処理はここまで
    //========================================================
    //========================================================
    // UI更新処理
    //========================================================
    void ChangeStage(int num) {

        switch (num) {

            case 1:

                m_image.sprite = m_stg10;
                t_stageName.text = "<b>学園広場その1</b>";
                t_commentary.text = "<b>障害物のない\nシンプルなステージ</b>";

                break;

            case 2:
                m_image.sprite = m_stg11;
                t_stageName.text = "<b>学園広場その2</b>";
                t_commentary.text = "<b>障害物ありの\nシンプルなステージ</b>";
                break;
            case 3:
                m_image.sprite = m_stg12;
                t_stageName.text = "<b>学園広場その3</b>";
                t_commentary.text = "<b>すこし複雑な\nテクニカルステージ</b>";
                break;
            case 4:
                m_image.sprite = m_stg20;
                t_stageName.text = "<b>ビル屋上その１</b>";
                t_commentary.text = "<b>障害物のない\nシンプルなステージ</b>";
                break;
            case 5:
                m_image.sprite = m_stg20;
                t_stageName.text = "<b>ビル屋上その２</b>";
                t_commentary.text = "<b>隠しステージ</b>";
                break;
            default:

                break;
        }
    }

    //========================================================
    // UI更新処理はここまで
    //========================================================
    //========================================================
    // 入力処理
    //========================================================
    int StageNum() {

        if (Mathf.Abs(Input.GetAxis(AXIS_HORIZONTAL)) < 0.1f){

            m_canPush = true;
        }

        if (Mathf.Abs(Input.GetAxis(AXIS_HORIZONTAL)) > 0.2f && m_canPush){


            if (Input.GetAxis(AXIS_HORIZONTAL) > 0){

                m_stageNum++;
            }
            else{

                m_stageNum--;
            }

            if (m_stageNum > 5) { m_stageNum = 1; }
            else if (m_stageNum < 1) { m_stageNum = 4; }

            // ここで音を再生
            AudioManager.Instance.PlaySE(AUDIO.SE_SELECT_SE);

            m_canPush = false;

            return m_stageNum;
        }
        return m_stageNum;
    }

    void CheckState() {

        // 決定キーが押されたとき
        if (Input.GetButtonDown(FIRE1_BUTTON)) {

            // SEを再生
            AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);

            monobitView.RPC(LOAD_STAGE, MonobitTargets.All, null);

            m_stop = true;
        }

        // キャンセルキーが押されたとき
        if (Input.GetButtonDown(FIRE2_BUTTON)) {

            // 部屋に入ってないときはキャラセレクトに戻る
            if (!MonobitNetwork.inRoom) {

                // SEを再生
                AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);

                FadeManager.Instance.LoadLevel(CHARA_SELECT_SCENE, 1.0f);
            }

            
        }
    }
    //========================================================
    // 入力処理はここまで
    //========================================================

    //========================================================
    // RPC処理
    //========================================================
    [MunRPC]
    void LoadStage() {

        switch (StageNum()) {

            case 1:

                FadeManager.Instance.MonobitLoadLevel(STAGE1_SCENE, 1.0f);
                break;

            case 2:

                FadeManager.Instance.MonobitLoadLevel(STAGE1_1_SCENE, 1.0f);
                break;
            case 3:
                FadeManager.Instance.MonobitLoadLevel(STAGE1_2_SCENE, 1.0f);
                break;
            case 4:
                FadeManager.Instance.MonobitLoadLevel(STAGE2_SCENE, 1.0f);
                break;
            case 5:
                FadeManager.Instance.MonobitLoadLevel(STAGE2_2_SCENE, 1.0f);
                break;
            
        }
        // BGMを止める
        AudioManager.Instance.FadeOutBGM();
    }
    //========================================================
    // RPC処理はここまで
    //========================================================
}
