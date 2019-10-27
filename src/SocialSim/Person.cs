using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Dynamic;
using System.Text;
using System.Collections.Concurrent;

namespace SocialSim
{

    //! Defines relationship between two people
    class Relationship
    {
        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="trust"> Degree of trust </param>
        /// <param name="blame"> Degree of blame </param>
        public Relationship(float trust, float blame)
        {
            Trust = trust;
            Blame = blame;
        }

        public float Trust;
        public float Blame;

    }

    class Person
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"> ID unique to this person </param>
        /// <param name="selflessness"> Degree pf selflessness of this person </param>
        /// <param name="selfishness"> Degree of selfishness of this person </param>
        /// <param name="health"> Degree of health of this person Only '</param>
        public Person(uint id, float selflessness, float selfishness, uint health)
        {
            ID= id;
            Selflessness = selflessness;
            Selfishness = selfishness;
            Health = health;
        }

        public void AddRelationShip(Relationship relationship)
        {
            _relationships.Add(relationship);
        }

        public IEnumerator<Relationship> GetRelationshipEnumerator()
        {
            return _relationships.GetEnumerator();
        }

        public uint ID;

        public uint Health;

        public float Selflessness { get; }

        public float Selfishness { get; }

        private ConcurrentBag<Relationship> _relationships;

    }
}
