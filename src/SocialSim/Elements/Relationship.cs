using System;

namespace SocialSim.Elements
{
    /// <summary>
    /// Relationship class Indicates relationship between two people
    /// The action of these people are determined by parameters specified in this object
    /// </summary>
    class Relationship
    {
        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="from"> Id of the person that this relationship is originated from </param>
        /// <param name="to"> Id of the person that this relationship is targeting </param>
        /// <param name="relation"> Indicates how close are people specified in the relationship object </param>
        /// <param name="frequency"> Indicates how often specified people in this Relationship object meet  </param>
        public Relationship(int from, int to, double relation, uint frequency)
        {
            From = from;
            To = to;
            Relation = relation;
            Frequency = frequency;
        }

        public int From { get; }
        public int To { get; }
        public double Relation;
        public float Frequency;
    }

    /// <summary>
    /// Wrapper class that contains relationship object of each direction A->B and B->A
    /// </summary>
    class RelationshipDescriptor
    {
        /// <summary>
        /// Constructor for RelationshipDescriptor
        /// </summary>
        /// <param name="relationshipAtoB"> relationship object from A to B </param>
        /// <param name="relationshipBtoA"> relationship object from B to A</param>
        public RelationshipDescriptor(Relationship relationshipAtoB, Relationship relationshipBtoA)
        {
            if (relationshipAtoB.From == relationshipBtoA.To && relationshipAtoB.To == relationshipBtoA.From)
            {
                _relationshipAtoB = relationshipAtoB;
                _relationshipBtoA = relationshipBtoA;
            }
            else
            {
                throw new ArgumentException("relationship to each direction does not match");
            }
        }

        /// <summary>
        /// Checks if this RelationshipDescriptor contains relationship of given personId
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public bool HasRelationShip(int personId)
        {
            if (_relationshipAtoB.From == personId || _relationshipBtoA.From == personId)
                return true;
            return false;
        }

        /// <summary>
        /// Gets relationship between two people respect to given personId
        /// Throws ArgumentException of personId has no matches
        /// </summary>
        /// <param name="personId"> Id of the person </param>
        /// <returns> Relationship object between two people respect to given personId </returns>
        public Relationship GetRelationship(int personId)
        {
            if (!HasRelationShip(personId))
                throw new ArgumentException("No such personId defined in this RelationShipDescriptor");
            if (_relationshipAtoB.From == personId)
                return _relationshipAtoB;
            return _relationshipBtoA;
        }

        /// <summary>
        /// Sets new relationship for corresponding person. Throws ArgumentException if person ID has no matches
        /// </summary>
        /// <param name="personId"> PersonID </param>
        /// <param name="relationship"></param>
        public void SetRelationship(int personId, Relationship relationship)
        {
            if (!HasRelationShip(personId))
                throw new ArgumentException("No such personId to update in this RelationshipDescriptor");
            if (_relationshipAtoB.From == personId)
            {
                _relationshipAtoB = relationship;
            }
            else
            {
                _relationshipBtoA = relationship;
            }
        }

        /// <summary>
        /// Returns Tuple of Id of two people defined in this RelationShipDescriptor
        /// </summary>
        /// <returns> Tuple of Id of two people defined in this RelationShipDescriptor</returns>
        public Tuple<int, int> GetPeopleIdTuple()
        {
            return new Tuple<int, int>(_relationshipAtoB.From, _relationshipBtoA.From);
        }

        public Tuple<Relationship, Relationship> GetRelationshipTuple()
        {
            return new Tuple<Relationship, Relationship>(_relationshipAtoB, _relationshipBtoA);
        }

        private Relationship _relationshipAtoB;
        private Relationship _relationshipBtoA;
    }
}