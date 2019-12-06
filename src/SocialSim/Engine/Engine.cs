using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Reflection.Metadata;
using System.Text;
using SocialSim.Elements;
using SocialSim.Model;


namespace SocialSim.Engine
{
    /// <summary>
    /// This class computes given relationships and people with _model given as a parameter
    /// </summary>
    class Engine
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model"> _model for computing Engine </param>
        /// <param name="relationShipDescriptorList"> List of relationships between people </param>
        /// <param name="peopleList"> List of people to simulate </param>
        public Engine(Model.Model model, List<RelationshipDescriptor> relationShipDescriptorList, List<Person> peopleList)
        {
            // Check if number of people matches
            if (relationShipDescriptorList.Count != peopleList.Count)
            {
                throw new ArgumentException("Size of relationship and peopleList does not match");
            }

            _model = model;
            RelationShipDescriptorList = relationShipDescriptorList;
            PeopleList= peopleList;
            _temporaryDescriptorList = new List<RelationshipDescriptor>(relationShipDescriptorList.Count);
        }

        private void SelectRelationshipSet()
        {
            Random rand = new Random();
            foreach (var relationShipDescriptor in RelationShipDescriptorList)
            {
                var relationShipTuple = relationShipDescriptor.GetRelationshipTuple();
                var minFrequency = Math.Min(relationShipTuple.Item1.Frequency, relationShipTuple.Item2.Frequency);
                var randValue = rand.NextDouble();

                Tuple<uint, uint> peopleIdTuple = relationShipDescriptor.GetPeopleIdTuple();

                Person subjectPerson = PeopleList.Find(person => person.Id == peopleIdTuple.Item1);
                Person targetPerson = PeopleList.Find(person => person.Id == peopleIdTuple.Item2);

                if (randValue < minFrequency)
                {
                    _temporaryDescriptorList.Add(relationShipDescriptor);
                    _temporaryPeopleList.Add(subjectPerson);
                    _temporaryPeopleList.Add(targetPerson);
                }
            }
        }

        private void ClearTemporaries()
        {
            _temporaryDescriptorList.Clear();
            _temporaryPeopleList.Clear();
        }

        /// <summary>
        /// Computes action of two people when they meet together
        /// </summary>
        /// <param name="index"> index of relationship to compute</param>
        /// <param name="firstOrSecond"> true if </param>
        public void Meet(int index, bool firstOrSecond)
        {
            if (firstOrSecond)
            {
                var descriptor = _temporaryDescriptorList[index];
                var firstPerson = _temporaryPeopleList[2 * index];
                var secondPerson = _temporaryPeopleList[2 * index + 1];

                Relationship firstRelationship = descriptor.GetRelationship(firstPerson.Id);
                Relationship secondRelationship = descriptor.GetRelationship(secondPerson.Id);

                double firstActionDegree = _model.ComputeActionDegree(firstPerson, firstRelationship);
                double secondActionDegree = _model.ComputeActionDegree(secondPerson, secondRelationship);

                Stance firstStance = _model.GetStance(firstActionDegree);
                Stance secondStance = _model.GetStance(secondActionDegree);

                _model.ComputeAction(ref firstPerson, ref secondPerson, ref firstRelationship, ref secondRelationship, ref firstStance, ref secondStance);

                descriptor.SetRelationship(firstPerson.Id, firstRelationship);
                descriptor.SetRelationship(secondPerson.Id, secondRelationship);

                _temporaryDescriptorList[index] = descriptor;
                _temporaryPeopleList[2 * index] = firstPerson;
                _temporaryPeopleList[2 * index + 1] = secondPerson;

            }
        }

        public List<RelationshipDescriptor> RelationShipDescriptorList { get; set; }

        public List<Person> PeopleList { get; set; }

        private readonly Model.Model _model;

        private List<RelationshipDescriptor> _temporaryDescriptorList;

        private List<Person> _temporaryPeopleList;
    }
}
