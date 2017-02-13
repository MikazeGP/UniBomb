using UnityEngine;
using System.Collections;
using System;

namespace FixedPhrase {

    public abstract class FixedPhrase {

        public abstract string Phrase();

        // 定型文
        protected const string  KONNICHIHA= "こんにちは";
        protected const string  YOROSIKU = "よろしくお願いします";
        protected const string  OTUKARE = "お疲れさまでした";
        protected const string  OK = "準備OK";
        protected const string  ARE_YOU_READY = "準備はよろしいですか？";
        protected const string  HAZIMEMASHITE = "はじめまして";
        protected const string  THANK = "助かる";
        protected const string  REMATCH = "再戦いいですか？";
        protected const string  FUN = "たのしませてくれよ";
        protected const string  KUSA = "www";
        protected const string  SORRY = "ごめんなさい";
        protected const string  AORI1 = "かかってこい";
        protected const string  AORI2 = "で？";
        protected const string  AORI3 = "ふーん";
        protected const string  AORI4 = "何を言ってるんだ？";
        protected const string  AORI5 = "素人は黙っとれ";
        protected const string  AORI6 = "おことわりいたす";
        protected const string  AORI7 = "何も見なかったことにしよう";
        protected const string  AORI8 = "ウソつくのやめてもらっていいですか？";
        protected const string  AORI9 = "なんかそういうデータあるんですか？";
        protected const string  AORI10 = "？？？";
        protected const string  AORI11 = "諦めてどうぞ";
        protected const string  AORI12 = "だが断る";

        protected string[] m_AORI = new string[] {AORI1,AORI2,AORI3,AORI4,
                                                                                    AORI5,AORI6,AORI7,AORI8,
                                                                                        AORI9,AORI10,AORI11,AORI12};
    }

    public class Greeting1 : FixedPhrase {

        public override string Phrase(){

            return KONNICHIHA;
        }
    }
    public class Greeting2 : FixedPhrase{

        public override string Phrase(){

            return YOROSIKU;
        }
    }
    public class Greeting3 : FixedPhrase{

        public override string Phrase(){

            return OTUKARE;
        }
    }
    public class Greeting4 : FixedPhrase{

        public override string Phrase(){

            return OK;
        }
    }
    public class Greeting5 : FixedPhrase{

        public override string Phrase(){

            return ARE_YOU_READY;
        }
    }
    public class Greeting6 : FixedPhrase {

        public override string Phrase(){

            return HAZIMEMASHITE;
        }
    }
    public class Greeting7 : FixedPhrase{

        public override string Phrase(){

            return THANK;
        }
    }
    public class Greeting8 : FixedPhrase{

        public override string Phrase(){

            return REMATCH;
        }
    }
    public class Greeting9: FixedPhrase{

        public override string Phrase(){

            return FUN;
        }
    }
    public class Greeting10 : FixedPhrase{

        public override string Phrase(){

            return KUSA;
        }
    }
    public class Greeting11 : FixedPhrase{

        public override string Phrase(){

            return SORRY;
        }
    }
    public class Aori : FixedPhrase{

        public override string Phrase(){

            System.Random r = new System.Random();

            int i = r.Next(0, 12);

            return m_AORI[i];
        }
    }
}