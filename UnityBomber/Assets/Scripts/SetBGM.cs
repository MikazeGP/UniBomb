using UnityEngine;
using System.Collections;

public class SetBGM : Origin {

    private const string COROUTINE_PLAY_BGM = "PlayBGM";

    public int m_bgmNum;

	// Use this for initialization
	void Start () {
        m_bgmNum = GrobalData.Instance._BGMNum;

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
