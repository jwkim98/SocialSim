using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Dynamic;
using System.Text;
using System.Collections.Concurrent;
using System.Data.SqlTypes;
using SocialSim.Model;

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
        /// <param name="power"> Amount of social power this person has </param>
        public Person(int groupId, int id, float selflessness, float selfishness, int money, int power)
        {
            GroupId = groupId;
            Id = id;
            Selflessness = selflessness;
            Selfishness = selfishness;
            Money = money;
            Power = power;
            RelationshipList = new List<Relationship>(Hyperparameter.MaxRelationshipCount);
        }

        /// <summary>
        /// Adds relationship to the person
        /// </summary>
        /// <param name="relationship"> relationship between people </param>
        public void AddRelationship(Relationship relationship)
        {
            RelationshipList.Add(relationship);
        }

        /// <summary>
        /// Checks if this person has relationship with given person ID
        /// </summary>
        /// <param name="personId"> Person ID to find relationship for</param>
        /// <returns> True if this person has relationship with id with personId false if not </returns>
        public bool HasRelationship(int personId)
        {
            foreach (Relationship relationship in RelationshipList)
            {
                if (relationship.To == personId)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Sets relationship between people
        /// </summary>
        /// <param name="relationship"> relationship from this person to other </param>
        public void SetRelationship(Relationship relationship)
        {
            if (relationship.From != Id)
                throw new ArgumentException("Relationship does not match with this person");

            int size = RelationshipList.Count;
            for (int index = 0; index < size; ++index)
            {
                if (RelationshipList[index].To == relationship.To)
                {
                    RelationshipList[index] = relationship;
                }
            }
        }

        public void ClearRelationships()
        {
            int size = RelationshipList.Count;
            for (int index = 0; index < size; ++index)
            {
                RelationshipList[index].HasComputed = false;
                RelationshipList[index].UpdateAmount = 0;
            }
        }

        /// <summary>
        /// Gets Enumerator for relationships
        /// </summary>
        /// <returns> Enumerator of the relationship </returns>
        public IEnumerator<Relationship> GetRelationshipEnumerator()
        {
            return RelationshipList.GetEnumerator();
        }

        /// <summary>
        /// Gets relationship with target person ID given as parameter
        /// </summary>
        /// <param name="targetPersonId"> Person's ID to search for </param>
        /// <returns> relationship with target person Id with targetPersonId </returns>
        public Relationship GetRelationshipTo(int targetPersonId)
        {
            foreach (Relationship relationship in RelationshipList)
            {
                if (relationship.To == targetPersonId)
                {
                    return relationship;
                }
            }

            return new Relationship(Id, targetPersonId, 0, 0);
        }

        public int GroupId { get; }

        public int Id { get; }

        public int Money;

        public int Power;

        public float Selflessness { get; }

        public float Selfishness { get; }

        public List<Relationship> RelationshipList { get; }
    }
}