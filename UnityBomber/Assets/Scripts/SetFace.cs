using UnityEngine;
using System.Collections;

public class SetFace{

    //========================================================
    // 定数
    //========================================================
    // スプライトファイルパス
    private const string SPRITE_FILE_PASS = "Sprite/Face";

    // 顔画像名
    // ユニティちゃん
    private const string SPRITE_UTC_G = "utcFaceG";
    private const string SPRITE_UTC_B = "utcFaceD";
    // みさき
    private const string SPRITE_MISAKI_G = "misakiFaceG";
    private const string SPRITE_MISAKI_B = "misakiFaceB";
    // ゆうこ
    private const string SPRITE_YUKO_G = "yukoFaceG";
    private const string SPRITE_YUKO_B = "yukoFaceB";

    public Sprite SetFaceSprite(string charaName,bool win) {

        Sprite spr = null;
        Sprite[] facespr = Resources.LoadAll<Sprite>(SPRITE_FILE_PASS);

        switch (charaName) {
            // Unityちゃん
            case "UnityChan":

                if (win) { spr = System.Array.Find<Sprite>(facespr, (sprite) => sprite.name.Equals(SPRITE_UTC_G)) ; }
                else { spr = System.Array.Find<Sprite>(facespr, (sprite) => sprite.name.Equals(SPRITE_UTC_B)); }
                break;
            // ミサキ
            case "Misaki":
                if (win) { spr = System.Array.Find<Sprite>(facespr, (sprite) => sprite.name.Equals(SPRITE_MISAKI_G)); }
                else { spr = System.Array.Find<Sprite>(facespr, (sprite) => sprite.name.Equals(SPRITE_MISAKI_B)); }
                break;
            // ユウコ
            case "Yuko":
                if (win) { spr = System.Array.Find<Sprite>(facespr, (sprite) => sprite.name.Equals(SPRITE_YUKO_G)); }
                else { spr = System.Array.Find<Sprite>(facespr, (sprite) => sprite.name.Equals(SPRITE_YUKO_B)); }
                break;
            default:
                break;
        }

        return spr;
    }
}
