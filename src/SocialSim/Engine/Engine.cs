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
        public Engine(Model.Model model, List<RelationshipDescriptor> relationShipDescriptorList,
            List<Person> peopleList)
        {
            // Check if number of people matches
            if (relationShipDescriptorList.Count != peopleList.Count)
            {
                throw new ArgumentException("Size of relationship and peopleList does not match");
            }

            _model = model;
            RelationShipDescriptorList = relationShipDescriptorList;
            PeopleList = peopleList;
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

                Tuple<int, int> peopleIdTuple = relationShipDescriptor.GetPeopleIdTuple();

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
        /// <param name="subjectPersonId"> subjectPersonId of relationship to compute</param>
        public void Meet(int subjectPersonId)
        {
            var subjectPerson = PeopleList[subjectPersonId];

            int size = subjectPerson.RelationshipList.Count;

            for(int index = 0; index < size; ++index)
            {
                Relationship subjectRelationship = subjectPerson.RelationshipList[index];

                int targetPersonId = subjectRelationship.To;
                Person targetPerson = PeopleList[targetPersonId];

                Relationship targetRelationship = targetPerson.GetRelationshipTo(subjectPersonId);

                double subjectActionDegree = _model.ComputeActionDegree(subjectPerson, subjectRelationship);
                double targetActionDegree = _model.ComputeActionDegree(targetPerson, targetRelationship);

                Stance subjectStance = _model.GetStance(subjectActionDegree);
                Stance targetStance = _model.GetStance(targetActionDegree);

                _model.ComputeAction(ref subjectPerson, ref targetPerson, ref subjectRelationship, ref targetRelationship,
                    subjectStance, targetStance);

                subjectPerson.RelationshipList[index] = subjectRelationship;
                targetPerson.SetRelationship(targetRelationship,targetPersonId);

                PeopleList[subjectPersonId] = subjectPerson;
                PeopleList[targetPersonId] = targetPerson;
            }
        }

        public void Rumor(int personIndex)
        {
            Person person = PeopleList[personIndex];
            int descriptorIndex = personIndex / 2;

            //Relationship relationship = RelationShipDescriptorList[descriptorIndex].GetRelationship(person.Id);

        }

        public List<RelationshipDescriptor> RelationShipDescriptorList { get; set; }

        public List<Person> PeopleList { get; set; }

        private readonly Model.Model _model;

        private List<RelationshipDescriptor> _temporaryDescriptorList;

        private List<Person> _temporaryPeopleList;
    }
}