using System;
using System.Collections.Generic;
using System.Text;

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
        public Relationship(uint from, uint to, double relation, uint frequency)
        {
            From = from;
            To = to;
            Relation = relation;
            Frequency = frequency;
        }

        public uint From { get; }
        public uint To { get; }
        public double Relation;
        public float Frequency;
    }
}
