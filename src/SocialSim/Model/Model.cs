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
                                              + Hyperparameter.RandomDegree * Random.NextDouble();
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

            return minRelation + Hyperparameter.RelationDegree * (minRelation + 1) +
                   Hyperparameter.RandomDegree * Random.NextDouble();
        }

        /// <summary>
        /// Computes action between two people and updates given reference parameters
        /// </summary>
        /// <param name="firstPerson"> First person object </param>
        /// <param name="secondPerson">Second person object </param>
        /// <param name="firstRelationship"> Relationship from first person to second person </param>
        /// <param name="secondRelationship"> Relationship from second person to first person </param>
        /// <param name="firstStance"> Stance of first person </param>
        /// <param name="secondStance"> Stance of second person </param>
        public virtual void ComputeAction(ref Person firstPerson, ref Person secondPerson,
            ref Relationship firstRelationship,
            ref Relationship secondRelationship, Stance firstStance,
            Stance secondStance)
        {
            Random random = new Random();

            if (firstStance == Stance.Evil && secondStance == Stance.Evil)
            {
                double victoryProbFirst = (double) firstPerson.Power / (firstPerson.Power + secondPerson.Power);
                double randomThreshold = random.NextDouble();

                int toSteal = random.Next(Hyperparameter.MinimumStealBetweenEvil,
                    Hyperparameter.MaximumStealBetweenEvil);

                double relationshipDecreaseAmount = (double) (toSteal - Hyperparameter.MinimumStealBetweenEvil) /
                                                    (Hyperparameter.MaximumStealBetweenEvil -
                                                     Hyperparameter.MinimumStealBetweenEvil);

                if (relationshipDecreaseAmount < Hyperparameter.MinimumRelationshipDecreaseBetweenEvil)
                {
                    relationshipDecreaseAmount = Hyperparameter.MinimumRelationshipDecreaseBetweenEvil;
                }

                if (randomThreshold < victoryProbFirst)
                {
                    if (secondPerson.Money > toSteal)
                    {
                        firstPerson.Money += toSteal;
                        secondPerson.Money -= toSteal;
                    }
                    else
                    {
                        firstPerson.Money += secondPerson.Money;
                        secondPerson.Money = 0;
                    }

                    Relationship relationship = secondPerson.GetRelationshipTo(firstPerson.Id);
                    relationship.Relation -= relationshipDecreaseAmount;
                    relationship.UpdateAmount = relationshipDecreaseAmount;
                    secondPerson.SetRelationship(relationship);
                }
                else
                {
                    if (firstPerson.Money > toSteal)
                    {
                        firstPerson.Money -= toSteal;
                        secondPerson.Money += toSteal;
                    }
                    else
                    {
                        secondPerson.Money += firstPerson.Money;
                        firstPerson.Money = 0;
                    }

                    Relationship relationship = firstPerson.GetRelationshipTo(secondPerson.Id);
                    relationship.Relation -= relationshipDecreaseAmount;
                    relationship.UpdateAmount = relationshipDecreaseAmount;
                    firstPerson.SetRelationship(relationship);
                }
            }

            if (firstStance == Stance.Good && secondStance == Stance.Evil)
            {
                int toSteal = random.Next(Hyperparameter.MinimumStealBetweenGoodEvil,
                    Hyperparameter.MaximumStealBetweenGoodEvil);

                double relationshipDecreaseAmount = (double) (toSteal - Hyperparameter.MinimumStealBetweenEvil) /
                                                    (Hyperparameter.MaximumStealBetweenEvil -
                                                     Hyperparameter.MinimumStealBetweenEvil);
                if (relationshipDecreaseAmount < Hyperparameter.MinimumRelationshipDecreaseGoodEvil)
                {
                    relationshipDecreaseAmount = Hyperparameter.MinimumRelationshipDecreaseGoodEvil;
                }

                if (firstPerson.Money > toSteal)
                {
                    firstPerson.Money += toSteal;
                    secondPerson.Money -= toSteal;
                }
                else
                {
                    firstPerson.Money += secondPerson.Money;
                    secondPerson.Money = 0;
                }

                Relationship relationship = firstPerson.GetRelationshipTo(secondPerson.Id);
                relationship.Relation -= relationshipDecreaseAmount;
                relationship.UpdateAmount = relationshipDecreaseAmount;
                firstPerson.SetRelationship(relationship);
            }

            if (firstStance == Stance.Evil && secondStance == Stance.Good)
            {
                int toSteal = random.Next(Hyperparameter.MinimumStealBetweenGoodEvil,
                    Hyperparameter.MaximumStealBetweenGoodEvil);

                double relationshipDecreaseAmount = (double) (toSteal - Hyperparameter.MinimumStealBetweenEvil) /
                                                    (Hyperparameter.MaximumStealBetweenEvil -
                                                     Hyperparameter.MinimumStealBetweenEvil);
                if (relationshipDecreaseAmount < Hyperparameter.MinimumRelationshipDecreaseGoodEvil)
                {
                    relationshipDecreaseAmount = Hyperparameter.MinimumRelationshipDecreaseGoodEvil;
                }

                if (secondPerson.Money > toSteal)
                {
                    secondPerson.Money += toSteal;
                    firstPerson.Money -= toSteal;
                }
                else
                {
                    secondPerson.Money += firstPerson.Money;
                    firstPerson.Money = 0;
                }

                Relationship relationship = secondPerson.GetRelationshipTo(firstPerson.Id);
                relationship.Relation -= relationshipDecreaseAmount;
                relationship.UpdateAmount = relationshipDecreaseAmount;
                secondPerson.SetRelationship(relationship);
            }

            if (firstStance == Stance.Good && secondStance == Stance.Good)
            {
                Relationship firstToSecondRelationship = firstPerson.GetRelationshipTo(secondPerson.Id);
                Relationship secondToFirstRelationship = secondPerson.GetRelationshipTo(firstPerson.Id);

                double firstRelation = firstToSecondRelationship.Relation;
                double secondRelation = secondToFirstRelationship.Relation;

                firstToSecondRelationship.Relation += (firstToSecondRelationship.Relation + 1) *
                                                      Hyperparameter.RelationshipIncreaseBetweenGood;
                secondToFirstRelationship.Relation += (secondToFirstRelationship.Relation + 1) *
                                                      Hyperparameter.RelationshipIncreaseBetweenGood;

                firstToSecondRelationship.Relation =
                    (firstToSecondRelationship.Relation > 1) ? 1 : firstToSecondRelationship.Relation;

                secondToFirstRelationship.Relation =
                    (secondToFirstRelationship.Relation > 1) ? 1 : secondToFirstRelationship.Relation;

                firstToSecondRelationship.UpdateAmount = firstToSecondRelationship.Relation - firstRelation;
                secondToFirstRelationship.UpdateAmount = secondToFirstRelationship.Relation - secondRelation;

                firstPerson.SetRelationship(firstToSecondRelationship);
                secondPerson.SetRelationship(secondToFirstRelationship);
            }
        }

        protected static readonly Random Random = new Random();

        public double EvilThreshold { get; }

        public double GoodThreshold { get; }
    }
}