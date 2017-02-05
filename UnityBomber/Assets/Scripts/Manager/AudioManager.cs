using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// BGMとSEの管理をするマネージャ。シングルトン。
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    //ボリューム保存用のkeyとデフォルト値
    private const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
    private const string SE_VOLUME_KEY = "SE_VOLUME_KEY";
    private const string VOICE_VOLUME_KEY = "VOICE_VOLUME_KEY";
    private const float BGM_VOLUME_DEFULT = 0.5f;
    private const float SE_VOLUME_DEFULT = 0.5f;
    private const float VOICE_VOLUME_DEFULT = 0.5f;

    //オーディオファイルのパス
    private const string BGM_PATH = "Audio/BGM";
    private const string SE_PATH = "Audio/SE";
    private const string VOICE_PATH = "Audio/VOICE";

    //BGMがフェードするのにかかる時間
    public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
    public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
    private float _bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

    //次流すBGM名、SE名、VOICE名
    private string _nextBGMName;
    private string _nextSEName;
    private string _nextVOICEName;

    //BGMをフェードアウト中か
    private bool _isFadeOut = false;

    //BGM用、SE用、VOICE用に分けてオーディオソースを持つ
    public AudioSource _bgmSource;
    private List<AudioSource> _seSourceList;
    private List<AudioSource> _voiceSourceList;
    private const int SE_SOURCE_NUM = 12;
    private const int VOICE_SOURCE_NUM = 100;

    //全AudioClipを保持
    private Dictionary<string, AudioClip> _bgmDic, _seDic,_voiceDic;

    //=================================================================================
    //初期化
    //=================================================================================

    private void Awake(){

        if (this != Instance){

            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        //オーディオリスナーおよびオーディオソースをSE+VOICE+1(BGMの分)作成
        gameObject.AddComponent<AudioListener>();
        for (int i = 0; i < SE_SOURCE_NUM +VOICE_SOURCE_NUM + 1; i++){

            gameObject.AddComponent<AudioSource>();
        }

        //作成したオーディオソースを取得して各変数に設定、ボリュームも設定
        AudioSource[] audioSourceArray = GetComponents<AudioSource>();
        _seSourceList = new List<AudioSource>();
        _voiceSourceList = new List<AudioSource>();

        //print(SE_SOURCE_NUM + VOICE_SOURCE_NUM + 1);

        for (int i = 0; i < audioSourceArray.Length; i++){

            audioSourceArray[i].playOnAwake = false;

            if (i == 0){

                audioSourceArray[i].loop = true;
                _bgmSource = audioSourceArray[i];
                _bgmSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);

            }else if (i >0 && i < 14) {

                _seSourceList.Add(audioSourceArray[i]);
                audioSourceArray[i].volume = PlayerPrefs.GetFloat(SE_VOLUME_KEY, SE_VOLUME_DEFULT);

            }
            else{
                
                _voiceSourceList.Add(audioSourceArray[i]);
                audioSourceArray[i].volume = PlayerPrefs.GetFloat(VOICE_VOLUME_KEY, VOICE_VOLUME_DEFULT);
            }

        }

        //リソースフォルダから全SE&BGMのファイルを読み込みセット
        _bgmDic = new Dictionary<string, AudioClip>();
        _seDic = new Dictionary<string, AudioClip>();
        _voiceDic = new Dictionary<string, AudioClip>();

        object[] bgmList = Resources.LoadAll(BGM_PATH);
        object[] seList = Resources.LoadAll(SE_PATH);
        object[] voiceList = Resources.LoadAll(VOICE_PATH);

        foreach (AudioClip bgm in bgmList){

            _bgmDic[bgm.name] = bgm;
        }

        foreach (AudioClip se in seList){

            _seDic[se.name] = se;
        }

        foreach(AudioClip voice in voiceList) {

            _voiceDic[voice.name] = voice;
        }

    }

    //=================================================================================
    //SE
    //=================================================================================

    /// <summary>
    /// 指定したファイル名のSEを流す。第二引数のdelayに指定した時間だけ再生までの間隔を空ける
    /// </summary>
    public void PlaySE(string seName, float delay = 0.0f){

        if (!_seDic.ContainsKey(seName)){

            Debug.Log(seName + "という名前のSEがありません");
            return;
        }

        _nextSEName = seName;
        Invoke("DelayPlaySE", delay);
    }

    private void DelayPlaySE(){

        foreach (AudioSource seSource in _seSourceList){

            if (!seSource.isPlaying){

                seSource.PlayOneShot(_seDic[_nextSEName] as AudioClip);
                return;
            }
        }
    }

    //=================================================================================
    // VOICE
    //=================================================================================

    /// <summary>
    /// 指定したファイル名のSEを流す。第二引数のdelayに指定した時間だけ再生までの間隔を空ける
    /// </summary>
    public void PlayVoice(string voiceName, float delay = 0.0f){

        if (!_voiceDic.ContainsKey(voiceName)){

            Debug.Log(voiceName + "という名前のVOICEがありません");
            return;
        }

        _nextVOICEName = voiceName;
        Invoke("DelayPlayVoice", delay);
    }

    private void DelayPlayVoice(){

        foreach (AudioSource voiceSource in _voiceSourceList){

            if (!voiceSource.isPlaying){

                voiceSource.PlayOneShot(_voiceDic[_nextVOICEName] as AudioClip);
                return;
            }
        }
    }

    //=================================================================================
    //BGM
    //=================================================================================

    /// <summary>
    /// 指定したファイル名のBGMを流す。ただし既に流れている場合は前の曲をフェードアウトさせてから。
    /// 第二引数のfadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
    /// </summary>
    public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH){

        if (!_bgmDic.ContainsKey(bgmName)){

            Debug.Log(bgmName + "という名前のBGMがありません");
            return;
        }

        //現在BGMが流れていない時はそのまま流す
        if (!_bgmSource.isPlaying){

            _nextBGMName = "";
            _bgmSource.clip = _bgmDic[bgmName] as AudioClip;
            _bgmSource.Play();
        }
        //違うBGMが流れている時は、流れているBGMをフェードアウトさせてから次を流す。同じBGMが流れている時はスルー
        else if (_bgmSource.clip.name != bgmName){

            _nextBGMName = bgmName;
            FadeOutBGM(fadeSpeedRate);
        }
    }

    /// <summary>
    /// BGMをすぐに止める
    /// </summary>
    public void StopBGM(){

        _bgmSource.Stop();
    }

    /// <summary>
    /// 現在流れている曲をフェードアウトさせる
    /// fadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
    /// </summary>
    public void FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW){

        _bgmFadeSpeedRate = fadeSpeedRate;
        _isFadeOut = true;
    }

    private void Update(){

        if (!_isFadeOut){

            return;
        }

        //徐々にボリュームを下げていき、ボリュームが0になったらボリュームを戻し次の曲を流す
        _bgmSource.volume -= Time.deltaTime * _bgmFadeSpeedRate;
        if (_bgmSource.volume <= 0){

            _bgmSource.Stop();
            _bgmSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
            _isFadeOut = false;

            if (!string.IsNullOrEmpty(_nextBGMName)){

                PlayBGM(_nextBGMName);
            }
        }

    }

    //=================================================================================
    //音量変更
    //=================================================================================

    /// <summary>
    /// BGMとSEのボリュームを別々に変更&保存
    /// </summary>
    public void ChangeVolume(float BGMVolume, float SEVolume,float VOICEVolume){

        _bgmSource.volume = BGMVolume;
        foreach (AudioSource seSource in _seSourceList){

            seSource.volume = SEVolume;
        }

        foreach(AudioSource voiceSource in _voiceSourceList) {

            voiceSource.volume = VOICEVolume;
        }

        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVolume);
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, SEVolume);
        PlayerPrefs.SetFloat(VOICE_VOLUME_KEY, VOICEVolume);
    }

    /// <summary>
    /// BGM、SE、Voiceのどれかのボリュームを変更
    /// 0はBGM,1はSE、2はVOICE
    /// </summary>
    public void ChangeVolume(float volume,int num) {

        switch (num) {

            case 0:

                _bgmSource.volume = volume;
                PlayerPrefs.SetFloat(BGM_VOLUME_KEY, volume);

                break;

            case 1:

                foreach (AudioSource seSource in _seSourceList){

                    seSource.volume = volume;
                }

                PlayerPrefs.SetFloat(SE_VOLUME_KEY, volume);

                break;

            case 2:

                foreach (AudioSource voiceSource in _voiceSourceList){

                    voiceSource.volume = volume;
                }

                PlayerPrefs.SetFloat(VOICE_VOLUME_KEY, volume);

                break;
        }
    }
}