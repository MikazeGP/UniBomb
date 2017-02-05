using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpsionManager : Origin {

    public GameObject SelectObj;
    public Slider s_BGM, s_SE,s_VOICE;
    public Button button;
    private int m_selectNum;
    private bool stop;
    private bool m_canPush;
    private  string[] m_voiceList = new string[3] { AUDIO.VOICE_V0032, AUDIO.VOICE_V2032, AUDIO.VOICE_V1032 };
    private int m_voiceNum;

    // Use this for initialization
    void Start () {

        FrameCount = 0;
        m_selectNum = 0;
        m_voiceNum = 0;
        stop = false;

    }
	
	// Update is called once per frame
	void Update () {

        FrameCount++;

        if (FrameCount >= 30){

           

            if (stop) { return; }
            this.State();
        }
	}

    void State() {


        if (Input.GetButtonDown(FIRE2_BUTTON))
        {

            // ここで再生
            AudioManager.Instance.PlaySE(AUDIO.SE_CANSEL);

            stop = true;

            FadeManager.Instance.LoadLevel(MODESELECT_SCENE, 1.0f);
        }

        switch (SelectNum())
        {

            case 0:
                SelectObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-370f, 80f);

                if (Input.GetAxisRaw(AXIS_HORIZONTAL) != 0){

                    s_BGM.value += Input.GetAxisRaw(AXIS_HORIZONTAL) / 100;
                    AudioManager.Instance.ChangeVolume(s_BGM.value, 0);
                }
                break;

            case 1:
                SelectObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-370f, -20f);

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

                SelectObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-370f, -120f);

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

                SelectObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-210f, -270);

                if (Input.GetButton(FIRE1_BUTTON)){


                    FadeManager.Instance.LoadLevel(MODESELECT_SCENE, 1.0f);
                    stop = true;
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
