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
            _random = new Random();
        }


        public void ClearRelationships()
        {
            int size = PeopleList.Count;
            for (int index = 0; index < size; ++index)
            { 
                PeopleList[index].ClearRelationships();
            }
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
                double meetProb = subjectRelationship.Frequency;

                if (_random.NextDouble() > meetProb)
                    continue;

                int targetPersonId = subjectRelationship.To;
                Person targetPerson = PeopleList[targetPersonId];

                Relationship targetRelationship = targetPerson.GetRelationshipTo(subjectPersonId);

                double subjectActionDegree = _model.ComputeActionDegree(subjectPerson, subjectRelationship);
                double targetActionDegree = _model.ComputeActionDegree(targetPerson, targetRelationship);

                Stance subjectStance = _model.GetStance(subjectActionDegree);
                Stance targetStance = _model.GetStance(targetActionDegree);

                _model.ComputeAction(ref subjectPerson, ref targetPerson, ref subjectRelationship, ref targetRelationship,
                    subjectStance, targetStance);

                subjectRelationship.HasComputed = true;
                targetRelationship.HasComputed = true;

                subjectPerson.RelationshipList[index] = subjectRelationship;
                targetPerson.SetRelationship(targetRelationship);

                PeopleList[subjectPersonId] = subjectPerson;
                PeopleList[targetPersonId] = targetPerson;
            }
        }

        public void Rumor(int personIndex)
        {
            Person person = PeopleList[personIndex];
            int descriptorIndex = personIndex / 2;

            // Relationship relationship = RelationShipDescriptorList[descriptorIndex].GetRelationship(person.Id);
        }

        public List<RelationshipDescriptor> RelationShipDescriptorList { get; set; }

        public List<Person> PeopleList { get; set; }

        private readonly Model.Model _model;

        private List<RelationshipDescriptor> _temporaryDescriptorList;

        private List<Person> _temporaryPeopleList;

        private Random _random;
    }
}