using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// オプションを管理するクラス
/// </summary>
public class OpsionManager : Origin {

    // セレクトマーカー
    public GameObject SelectObj;
    // 各項目のスライダー
    public Slider s_BGM, s_SE,s_VOICE;
    // ボタン
    public Button button;
    // 解説テキスト
    public Text m_commentaryText;
    // 現在のセレクト番号
    private int m_selectNum;
    // 更新を止めるフラグ true..止める false..止めない
    private bool m_stop;
    // ボタンを押せるフラグ true..押せる false..押せない
    private bool m_canPush;
    // ボイスの音量確認ボイス
    private  string[] m_voiceList = new string[3] { AUDIO.VOICE_V0032, AUDIO.VOICE_V2032, AUDIO.VOICE_V1032 };
    // ボイス番号
    private int m_voiceNum;

    // Use this for initialization
    void Start () {
        // 0で初期化
        FrameCount = 0;
        m_selectNum = 0;
        m_voiceNum = 0;
        // UIの初期化
        s_BGM.value = GrobalData.Instance._currentBGMVolume;
        s_SE.value = GrobalData.Instance._currentSEVolume;
        s_VOICE.value = GrobalData.Instance._currentVoiceVolume;
      // 更新する
      m_stop = false;

    }
	
	// Update is called once per frame
	void Update () {

        FrameCount++;

        if (FrameCount >= 30){

           

            if (m_stop) { return; }
            this.State();
        }
	}

    void State() {


        if (Input.GetButtonDown(FIRE2_BUTTON))
        {

            // ここで再生
            AudioManager.Instance.PlaySE(AUDIO.SE_CANSEL);
            // 更新を止める
            m_stop = true;

            FadeManager.Instance.LoadLevel(MODESELECT_SCENE, 1.0f);
        }

        switch (SelectNum())
        {
            
            case 0:
                SelectObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350f, 80f);
                m_commentaryText.text = "<b>BGMの音量を変更します</b>";
                if (Input.GetAxisRaw(AXIS_HORIZONTAL) != 0){

                    s_BGM.value += Input.GetAxisRaw(AXIS_HORIZONTAL) / 100;
                    AudioManager.Instance.ChangeVolume(s_BGM.value, 0);
                }
                break;

            case 1:
                SelectObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350f, -20f);
                m_commentaryText.text = "<b>SEの音量を変更します</b>";
                if (Input.GetAxisRaw(AXIS_HORIZONTAL) != 0)
                {
                    s_SE.value += Input.GetAxisRaw(AXIS_HORIZONTAL) / 100;
                    AudioManager.Instance.ChangeVolume(s_SE.value, 1);

                }
                if (Input.GetButtonDown(FIRE1_BUTTON)) {

                    // ここで再生
                    AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
                }

                break;

            case 2:

                SelectObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350f, -120f);
                m_commentaryText.text = "<b>VOICEの音量を変更します</b>";
                if (Input.GetAxisRaw(AXIS_HORIZONTAL) != 0){

                    s_VOICE.value += Input.GetAxisRaw(AXIS_HORIZONTAL) / 100;
                    AudioManager.Instance.ChangeVolume(s_VOICE.value, 2);

                }

                if (Input.GetButtonDown(FIRE1_BUTTON)){

                    // ここで再生
                    AudioManager.Instance.PlayVoice(m_voiceList[m_voiceNum]);

                    m_voiceNum++;

                    if(m_voiceNum > 2) { m_voiceNum = 0; }

                   
                }

                break;

            case 3:

                SelectObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150f, -220f);
                m_commentaryText.text = "<b>モードセレクトに戻ります</b>";
                if (Input.GetButton(FIRE1_BUTTON)){
                    // グローバルデータに保存
                    GrobalData.Instance._currentBGMVolume = s_BGM.value;
                    GrobalData.Instance._currentSEVolume = s_SE.value;
                    GrobalData.Instance._currentVoiceVolume = s_VOICE.value;

                    FadeManager.Instance.LoadLevel(MODESELECT_SCENE, 1.0f);
                    m_stop = true;
                    // ここで再生
                    AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
                }
                break;
        }

    }
    int SelectNum()
    {


        if (Mathf.Abs(Input.GetAxis(AXIS_VERTICAL)) < 0.1f)
        {

            m_canPush = true;
        }

        if (Mathf.Abs(Input.GetAxis(AXIS_VERTICAL)) > 0.2f && m_canPush)
        {


            if (Input.GetAxis(AXIS_VERTICAL) > 0)
            {

                m_selectNum--;

            }
            else
            {

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

}
