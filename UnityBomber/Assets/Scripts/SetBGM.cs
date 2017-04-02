using UnityEngine;
using System.Collections;

/// <summary>
/// BGMを設定するクラス
/// </summary>
public class SetBGM : Origin {

    //========================================================
    // 定数
    //========================================================
    private const string COROUTINE_PLAY_BGM = "PlayBGM";

    //========================================================
    // リテラル
    //========================================================
    // BGMの番号
    public int m_bgmNum;

	// 初期化処理
	void Start () {
        //　グローバルデータからBGMの番号を取得
        m_bgmNum = GrobalData.Instance._BGMNum;

        //　コルーチンの開始
        StartCoroutine(COROUTINE_PLAY_BGM);
	}
	private IEnumerator PlayBGM() {

        yield return new WaitForSeconds(3.5f);

        switch (m_bgmNum) {

            case 0:
                AudioManager.Instance.PlayBGM(AUDIO.BGM_BATTLE1);
                break;
            case 1:
                AudioManager.Instance.PlayBGM(AUDIO.BGM_BATTLE2);
                break;
            case 2:
                AudioManager.Instance.PlayBGM(AUDIO.BGM_BATTLE3);
                break;
            case 3:
                AudioManager.Instance.PlayBGM(AUDIO.BGM_BATTLE4);
                break;
            case 4:
                AudioManager.Instance.PlayBGM(AUDIO.BGM_BATTLE5);
                break;
        }
    }
}
