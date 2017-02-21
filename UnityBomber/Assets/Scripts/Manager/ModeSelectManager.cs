using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// モード選択を管理するクラス
/// </summary>
public class ModeSelectManager : Origin {
    // セレクトマーカー
    public GameObject SelectMark;
    // 各ボタン
    public Button RANDOM_BUTTON, ROOM_BUTTON,OPSION_BUTTON, BACK_BUTTON;
    private bool m_stop;
    private bool m_canPush;
    [SerializeField]
    private int m_selectNum;

	// Use this for initialization
	void Start () {

        AudioManager.Instance.PlayBGM(AUDIO.BGM_SELECT_BGM, AudioManager.BGM_FADE_SPEED_RATE_HIGH);
        // UIの初期化
        SelectMark.GetComponent<RectTransform>().anchoredPosition = new Vector2(-395f, 460f);
        RANDOM_BUTTON.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 480);
        ROOM_BUTTON.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 370f);
        OPSION_BUTTON.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 260f);
        BACK_BUTTON.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 150f);

        m_selectNum = 0;
        FrameCount = 0;
        m_stop = false;
        
    }
	
	// Update is called once per frame
	void Update () {
        FrameCount++;

        if(FrameCount >= 60) {

            if (m_stop) {

              
                return;
            }
            this.State(SelectNum());
        }   
	}

    public void BackTitle() {

        FadeManager.Instance.LoadLevel(TITLE_SCENE, 1.0f);
    }

    // 状態を更新
    void State(int num) {

        if (Input.GetButtonDown(FIRE2_BUTTON)) {

            // SEとBGMを設定し再生
            AudioManager.Instance.PlaySE(AUDIO.SE_CANSEL);
            AudioManager.Instance.PlayBGM(AUDIO.BGM_TITLE, AudioManager.BGM_FADE_SPEED_RATE_HIGH);

            m_stop = true;

            FadeManager.Instance.LoadLevel(TITLE_SCENE, 1.0f);
        }

        switch (num) {

            // ロームマッチ
            case 0:

                SelectMark.GetComponent<RectTransform>().anchoredPosition = new Vector2(-395f, 460f);

                if (Input.GetButtonDown(FIRE1_BUTTON)) {

                    this.LoadChatScene();
                }
                break;
            // ランダムマッチ
            case 1:

                SelectMark.GetComponent<RectTransform>().anchoredPosition = new Vector2(-395f, 340f);

                
                if (Input.GetButtonDown(FIRE1_BUTTON))
                {
                    this.LoadCharacterSelectScene();
                   
                }

                break;
            
            // オプション
            case 2:

                SelectMark.GetComponent<RectTransform>().anchoredPosition = new Vector2(-395f, 240f);
                if (Input.GetButtonDown(FIRE1_BUTTON)){

                    this.LoadOpsionScene();
                }
                break;

            // タイトル
            case 3:
                SelectMark.GetComponent<RectTransform>().anchoredPosition = new Vector2(-395f, 150f);
                if (Input.GetButton(FIRE1_BUTTON))
                {

                    this.GameEnd();
                }
                break;
        }
    }
    int SelectNum(){


        if (Mathf.Abs(Input.GetAxis(AXIS_VERTICAL)) < 0.1f){

            m_canPush = true;
        }

        if (Mathf.Abs(Input.GetAxis(AXIS_VERTICAL)) > 0.2f && m_canPush){


            if (Input.GetAxis(AXIS_VERTICAL) > 0){

                m_selectNum--;

            }else{

                m_selectNum++;
            }

            m_selectNum = Mathf.Clamp(m_selectNum, 0, 3);

            //　ここで再生
            AudioManager.Instance.PlaySE(AUDIO.SE_SELECT_SE);

            m_canPush = false;

            return m_selectNum;
        }
        return m_selectNum;
    }
    // マウスでボタンのクリックして入力したときの処理
    
    // チャットチーンをロードする
    public void LoadChatScene() {

        FadeManager.Instance.LoadLevel(CHATROOM_SCENE, 1.0f);
        m_stop = true;
        // SE再生
        AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
    }

    // キャラセレクトシーンをロードする
    public void LoadCharacterSelectScene() {

        FadeManager.Instance.LoadLevel(CHARA_SELECT_SCENE, 1.0f);
        m_stop = true;
        // SE再生
        AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
    }

    // オプションシーンをロードする
    public void LoadOpsionScene() {

        FadeManager.Instance.LoadLevel(OPSION_SCENE, 1.0f);
        m_stop = true;
        // SE再生
        AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
    }

    // タイトルシーンをロードする
    public void GameEnd() {


        Application.Quit();
        m_stop = true;
        // SE再生
        AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
    }
}
