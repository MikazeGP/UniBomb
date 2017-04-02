using UnityEngine;
using System.Collections;

public class LoadManager : Origin {

    // Use this for initialization
    void Start () {

        FrameCount = 0;

        // 音量の初期化
        AudioManager.Instance.ChangeVolume(
            GrobalData.Instance._currentBGMVolume,
            GrobalData.Instance._currentSEVolume,
            GrobalData.Instance._currentVoiceVolume
            );

	}
	
	// 更新処理
	void Update () {

        // フレームカウントの更新
        FrameCount++;

        if(FrameCount == 120) {

            FadeManager.Instance.LoadLevel(TITLE_SCENE,1.0f);
        }
	}
}
