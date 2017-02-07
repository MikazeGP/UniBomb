using UnityEngine;
using System.Collections;
using MonobitEngine;
using UnityEngine.UI;

public class StageSelect : Origin {

    private const string LOAD_STAGE = "LoadStage";

    public Image m_image;
    public Sprite m_stg1, m_stg2;

    public Text t_stageName, t_commentary;

    private int m_stageNum;
    private bool m_canPush;
    private bool m_stop;
    private bool m_decided;

    void Start() {

        m_stageNum = 1;
        FrameCount = 0;
        m_stop = false;

    }

    void Update() {

        // ホスト以外は処理をしない
        if (!MonobitEngine.MonobitNetwork.isHost && MonobitNetwork.inRoom)
        {
            return;
        }

        ChangeStage(StageNum());

        FrameCount++;

        if(FrameCount >= 65) {

            if(m_stop == true) {

                return;
            }

            CheckState();
        }
    }

    void ChangeStage(int num) {

        switch (num) {

            case 1:

                m_image.sprite = m_stg1;
                t_stageName.text = "<b>学園広場</b>";
                t_commentary.text = "学園前の噴水広場です。";

                break;

            case 2:

                m_image.sprite = m_stg2;
                t_stageName.text = "<b>デパート屋上</b>";
                t_commentary.text = "";

                break;

            default:

                break;
        }
    }

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

            if (m_stageNum > 3) { m_stageNum = 1; }
            else if (m_stageNum < 1) { m_stageNum = 2; }

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

            AudioManager.Instance.FadeOutBGM(AudioManager.BGM_FADE_SPEED_RATE_HIGH);
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

    [MunRPC]
    void LoadStage() {

        switch (StageNum()) {

            case 1:

                FadeManager.Instance.MonobitLoadLevel(STAGE1_SCENE, 1.0f);
                break;

            case 2:

                FadeManager.Instance.MonobitLoadLevel(STAGE2_SCENE, 1.0f);
                break;
        }
    }
}
