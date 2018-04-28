using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
    {

        PowerAttackConfig config;

        public PowerAttackConfig Config
        {
            set
            {
                config = value;
            }
        }

        public void Use()
        {
            throw new System.NotImplementedException();
        }

        // Use this for initialization
        void Start()
        {
            print("Power attack behaviour attached to " + gameObject.name);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
