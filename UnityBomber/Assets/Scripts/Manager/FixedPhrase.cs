using UnityEngine;
using System.Collections;
using System;


namespace FixedPhrase
{
    /// <summary>
    /// 定型文基底クラス
    /// </summary>
    public abstract class FixedPhrase
    {
        // 定型文を呼び出す
        public abstract string Phrase();

        // 定型文
        protected const string KONNICHIHA = "こんにちは";
        protected const string YOROSIKU = "よろしくお願いします";
        protected const string OTUKARE = "お疲れさまでした";
        protected const string OK = "OK";
        protected const string ARE_YOU_READY = "準備はよろしいですか？";
        protected const string HAZIMEMASHITE = "はじめまして";
        protected const string EE = "ええっ";
        protected const string REMATCH = "再戦いいですか？";
        protected const string THX = "ありがとう";
        protected const string KUSA = "www";
        protected const string SORRY = "ごめんなさい";

        // ボイス番号
        protected int m_voiceNum1 = UnityEngine.Random.Range(0,3);
        protected int m_voiceNum2 = UnityEngine.Random.Range(1, 34);
    }
    /// <summary>
    /// こんにちは
    /// </summary>
    public class Greeting1 : FixedPhrase{

        public override string Phrase(){

            return KONNICHIHA;
        }
    }
    /// <summary>
    /// よろしく
    /// </summary>
    public class Greeting2 : FixedPhrase
    {

        public override string Phrase()
        {

            return YOROSIKU;
        }
    }
    /// <summary>
    /// おつ
    /// </summary>
    public class Greeting3 : FixedPhrase
    {

        public override string Phrase()
        {

            return OTUKARE;
        }
    }
    /// <summary>
    /// OK
    /// </summary>
    public class Greeting4 : FixedPhrase
    {

        public override string Phrase()
        {

            return OK;
        }
    }
    /// <summary>
    /// 準備はいい？
    /// </summary>
    public class Greeting5 : FixedPhrase
    {

        public override string Phrase()
        {

            return ARE_YOU_READY;
        }
    }
    /// <summary>
    /// はじめまして
    /// </summary>
    public class Greeting6 : FixedPhrase
    {

        public override string Phrase()
        {

            return HAZIMEMASHITE;
        }
    }
    /// <summary>
    /// ええっ
    /// </summary>
    public class Greeting7 : FixedPhrase
    {

        public override string Phrase()
        {

            return EE;
        }
    }
    /// <summary>
    /// 再戦
    /// </summary>
    public class Greeting8 : FixedPhrase
    {

        public override string Phrase()
        {

            return REMATCH;
        }
    }
    /// <summary>
    /// ありがとう
    /// </summary>
    public class Greeting9 : FixedPhrase
    {

        public override string Phrase()
        {

            return THX;
        }
    }
    /// <summary>
    /// www
    /// </summary>
    public class Greeting10 : FixedPhrase
    {

        public override string Phrase()
        {

            return KUSA;
        }
    }
    /// <summary>
    /// ごめんなさい
    /// </summary>
    public class Greeting11 : FixedPhrase{

        public override string Phrase(){

            return SORRY;
        }
    }
    //　ボイス(未実装)
    public class Voice : FixedPhrase {

        public override string Phrase(){

            AudioManager.Instance.PlayVoice("V"+m_voiceNum1.ToString() + "0" + m_voiceNum2.ToString("D2"));

            return "隠しボイス";
        }
    }
}