using UnityEngine;
using System.Collections;
using System;

namespace FixedPhrase
{

    public abstract class FixedPhrase
    {

        public abstract string Phrase();

        // 定型文
        protected const string KONNICHIHA = "こんにちは";
        protected const string YOROSIKU = "よろしくお願いします";
        protected const string OTUKARE = "お疲れさまでした";
        protected const string OK = "準備OK";
        protected const string ARE_YOU_READY = "準備はよろしいですか？";
        protected const string HAZIMEMASHITE = "はじめまして";
        protected const string THANK = "ええっ";
        protected const string REMATCH = "再戦いいですか？";
        protected const string FUN = "ありがとう";
        protected const string KUSA = "www";
        protected const string SORRY = "ごめんなさい";
    }

    public class Greeting1 : FixedPhrase
    {

        public override string Phrase()
        {

            return KONNICHIHA;
        }
    }
    public class Greeting2 : FixedPhrase
    {

        public override string Phrase()
        {

            return YOROSIKU;
        }
    }
    public class Greeting3 : FixedPhrase
    {

        public override string Phrase()
        {

            return OTUKARE;
        }
    }
    public class Greeting4 : FixedPhrase
    {

        public override string Phrase()
        {

            return OK;
        }
    }
    public class Greeting5 : FixedPhrase
    {

        public override string Phrase()
        {

            return ARE_YOU_READY;
        }
    }
    public class Greeting6 : FixedPhrase
    {

        public override string Phrase()
        {

            return HAZIMEMASHITE;
        }
    }
    public class Greeting7 : FixedPhrase
    {

        public override string Phrase()
        {

            return THANK;
        }
    }
    public class Greeting8 : FixedPhrase
    {

        public override string Phrase()
        {

            return REMATCH;
        }
    }
    public class Greeting9 : FixedPhrase
    {

        public override string Phrase()
        {

            return FUN;
        }
    }
    public class Greeting10 : FixedPhrase
    {

        public override string Phrase()
        {

            return KUSA;
        }
    }
    public class Greeting11 : FixedPhrase{

        public override string Phrase(){

            return SORRY;
        }
    }
    public class Voice : FixedPhrase {

        public override string Phrase(){

            return "ボイス";
        }
    }
}