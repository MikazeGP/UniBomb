using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// ランダムな背景を表示するクラス
/// </summary>
public class RandomBG : Origin {
    //========================================================
    // 定数
    //========================================================
    // スプライトファイルパス
    private const string SPRITE_FILE_PASS = "Sprite/BG";

    //========================================================
    // リテラル
    //========================================================
    // BGスプライトデータ
    private Sprite[] m_bgSprite;
    // BGスプライトを表示するイメージ
    public Image m_bgImage;

    // 乱数
    private int m_randomInt;
    // 解像度　横と高さ
    private int m_screenWidth, m_screenHeigh;
    //========================================================
    // 初期化処理
    //========================================================
    void Start() {
        // Resourcesからスプライトを読み込む
        m_bgSprite = Resources.LoadAll<Sprite>(SPRITE_FILE_PASS);
        // 1~5の乱数を取得
        m_randomInt = (int)Random.Range(0f, 100f) % 6;
        // BGを設定
        m_bgImage.sprite = m_bgSprite[m_randomInt];

        // 解像度を取得
        m_screenWidth = Screen.width;
        m_screenHeigh = Screen.height;
        // BGを解像度に合わせる
        m_bgImage.rectTransform.sizeDelta = new Vector2(m_screenWidth,m_screenHeigh);
    }
    //========================================================
    // 初期化処理はここまで
    //========================================================
}
