using System;
using System.Collections.Generic;
using System.Text;
using SocialSim.Elements;

namespace SocialSim.Model
{
    /// <summary>
    /// Model class defines computation model for the simulation
    /// It includes some static methods used in computation
    /// </summary>
    abstract class Model
    {

        /// <summary>
        /// Computes action degree of subject person when given relationship is activated
        /// </summary>
        /// <param name="subjectPerson"></param>
        /// <param name="relationship"></param>
        /// <returns></returns>
        public static double ComputeAction(Person subjectPerson, Relationship relationship)
        {
            return subjectPerson.Selflessness + subjectPerson.Selfishness * (relationship.Relation - 1) + Hyperparemeters.RandomDegree * Random.NextDouble();
        }

        /// <summary>
        /// Computes Degree of two people encountering together
        /// </summary>
        /// <param name="relationshipDescriptor"> object describing two people's relationships in both directions '</param>
        /// <returns></returns>
        public static double ComputeEncounterDegree(RelationshipDescriptor relationshipDescriptor)
        {
            Tuple<uint, uint> idTuple = relationshipDescriptor.GetPeopleIdTuple();
            Relationship relationshipA = relationshipDescriptor.GetRelationship(idTuple.Item1);
            Relationship relationshipB = relationshipDescriptor.GetRelationship(idTuple.Item2);
            double minRelation = Math.Min(relationshipA.Relation, relationshipB.Relation);

            return minRelation + Hyperparemeters.RelationDegree * (minRelation + 1) +
                   Hyperparemeters.RandomDegree * Random.NextDouble();
        }

        private static readonly Random Random = new Random();
    }
}
