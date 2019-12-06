using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SocialSim.Elements;

namespace SocialSim.Model
{
    enum Stance
    {
        Good,
        Evil,
        Middle,
    }

    /// <summary>
    /// BasicModel class defines computation model for the simulation
    /// It includes some static methods used in computation
    /// </summary>
    class Model
    {
        public Model(double evilThreshold, double goodThreshold)
        {
            if (evilThreshold > goodThreshold)
                throw new ArgumentException("GoodThreshold should be always equal or greater than EvilThreshold");
            EvilThreshold = evilThreshold;
            GoodThreshold = goodThreshold;
        }

        /// <summary>
        /// Computes action degree of subject person when given relationship is activated
        /// </summary>
        /// <param name="subjectPerson"></param>
        /// <param name="relationship"></param>
        /// <returns></returns>
        public virtual double ComputeActionDegree(Person subjectPerson, Relationship relationship)
        {
            return subjectPerson.Selflessness + subjectPerson.Selfishness * (relationship.Relation - 1)
                                              + Hyperparameters.RandomDegree * Random.NextDouble();
        }

        /// <summary>
        /// Returns Stance that model person should have with threshold
        /// </summary>
        /// <param name="actionDegree"> ActionDegree representing person computed by ComputeActionDegree </param>
        /// <returns> Stance that person should have to the other </returns>
        public virtual Stance GetStance(double actionDegree)
        {
            if (actionDegree < EvilThreshold)
                return Stance.Evil;
            if (actionDegree < GoodThreshold)
                return Stance.Middle;
            return Stance.Good;
        }

        /// <summary>
        /// Computes Degree of two people encountering together
        /// </summary>
        /// <param name="relationshipDescriptor"> object describing two people's relationships in both directions '</param>
        /// <returns></returns>
        public virtual double ComputeEncounterDegree(RelationshipDescriptor relationshipDescriptor)
        {
            Tuple<int, int> idTuple = relationshipDescriptor.GetPeopleIdTuple();
            Relationship relationshipA = relationshipDescriptor.GetRelationship(idTuple.Item1);
            Relationship relationshipB = relationshipDescriptor.GetRelationship(idTuple.Item2);
            double minRelation = Math.Min(relationshipA.Frequency, relationshipB.Frequency);

            return minRelation + Hyperparameters.RelationDegree * (minRelation + 1) +
                   Hyperparameters.RandomDegree * Random.NextDouble();
        }

        public virtual void ComputeAction(ref Person firstPerson, ref Person secondPerson,
            ref Relationship firstRelationship,
            ref Relationship secondRelationship, Stance firstStance,
            Stance secondStance)
        {
            if (firstStance == Stance.Evil && secondStance == Stance.Evil)
            {
                //TODO : think about the action
            }

            if (firstStance == Stance.Good && secondStance == Stance.Evil)
            {
                //TODO : think about the action
            }

            if (firstStance == Stance.Good && secondStance == Stance.Evil)
            {

            }
        }

        protected static readonly Random Random = new Random();

        public double EvilThreshold { get; }

        public double GoodThreshold { get; }
    }
}