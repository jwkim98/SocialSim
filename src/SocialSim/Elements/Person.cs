using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Dynamic;
using System.Text;
using System.Collections.Concurrent;
using System.Data.SqlTypes;

namespace SocialSim.Elements
{


    /// <summary>
    /// Person class contains basic information that each person should have
    /// This plays role of basic unit in the simulation
    /// </summary>
    class Person
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"> Id unique to this person </param>
        /// <param name="selflessness"> Degree pf selflessness of this person </param>
        /// <param name="selfishness"> Degree of selfishness of this person </param>
        /// <param name="money"> Degree of health of this person Only </param>
        /// <param name= " power"> Amount of social power this person has </param>
        public Person(uint id, float selflessness, float selfishness, int money, uint power)
        {
            Id= id;
            Selflessness = selflessness;
            Selfishness = selfishness;
            Money = money;
            Power = power;
        }

        /// <summary>
        /// Adds relationship to the person
        /// </summary>
        /// <param name="relationship"> relationship between people </param>
        public void AddRelationShip(Relationship relationship)
        {
            _relationships.Add(relationship);
        }

        /// <summary>
        /// Gets Enumerator for relationships
        /// </summary>
        /// <returns> Enumerator of the relationship </returns>
        public IEnumerator<Relationship> GetRelationshipEnumerator()
        {
            return _relationships.GetEnumerator();
        }

        public uint Id { get; }

        public int Money;

        public uint Power;

        public float Selflessness { get; }

        public float Selfishness { get; }

        private ConcurrentBag<Relationship> _relationships;

    }
}
