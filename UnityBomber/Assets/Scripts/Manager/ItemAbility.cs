using UnityEngine;
using System.Collections;
using System;

namespace ItemAbility {

    public abstract class BaseItemAbility {

        public abstract void SetAbility();

        System.Random m_random = new System.Random(10);
        //protected int m_randomInt 
    }

    public class UpAbility : BaseItemAbility {

        public override void SetAbility()
        {
            throw new NotImplementedException();
        }
    }

    public class DownAbility : BaseItemAbility
    {

        public override void SetAbility()
        {
            throw new NotImplementedException();
        }
    }
}