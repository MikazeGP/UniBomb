using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// タイトルを管理するクラス
/// </summary>
public class TitleManager : Origin {

    public GameObject textObj;
    
    private float nextTime;
    public float interval = 0.8f; //点滅周期
    // 更新停止フラグ
    public bool stop;

    // 初期化処理
    void Start () {

        // BGMの再生
        AudioManager.Instance.PlayBGM(AUDIO.BGM_TITLE, AudioManager.BGM_FADE_SPEED_RATE_HIGH);

        nextTime = Time.time;

        // 更新させる
        stop = false;

        FrameCount = 0;
	}
	
	// 更新処理
	void Update () {

        // フレームをインクリメントする
        FrameCount++;

        //　点滅処理
        this.Invincible();

        if (FrameCount >= 60)
        {

            if (stop) {  return; }

            this.State();
        }
	}
    // 点滅処理
    void Invincible() {

        
        if(Time.time > nextTime) {

            float alpha = textObj.GetComponent<CanvasRenderer>().GetAlpha();

            if(alpha == 1.0f) {

                textObj.GetComponent<CanvasRenderer>().SetAlpha(0.0f);

            }
            else {

                textObj.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
            }

            nextTime += interval;

           
        }
    }

    void State() {

        if (Input.anyKey) {

            // ここで再生
            AudioManager.Instance.PlaySE(AUDIO.SE_ENTER);
            AudioManager.Instance.PlayBGM(AUDIO.BGM_SELECT_BGM, AudioManager.BGM_FADE_SPEED_RATE_HIGH);
            

            // シーンの移動
            FadeManager.Instance.LoadLevel(MODESELECT_SCENE, 1.0f);

            stop = true;

        }
    }
}
