using System;
using System.Collections.Generic;
using System.IO;
using SocialSim.Elements;
using SocialSim.Model;


namespace SocialSim.Engine
{
    /// <summary>
    /// This class computes given relationships and people with _model given as a parameter
    /// </summary>
    class Engine
    {
        public void ClearRelationships()
        {
            int size = PeopleList.Count;
            for (int index = 0; index < size; ++index)
            {
                PeopleList[index].ClearRelationships();
            }
        }

        public void ReadPeopleFile(String path)
        {
            using (StreamReader fs = new StreamReader(path))
            {
                var str = fs.ReadLine();
                var values = str.Split(",");
                int id, money, power;
                float selflessness, selfishness;
                int.TryParse(values[0], out id);
                int.TryParse(values[3], out money);
                int.TryParse(values[4], out power);

                selflessness = float.Parse(values[1]);
                selfishness = float.Parse(values[2]);

                PeopleList.Add(new Person(id, selflessness, selfishness, money, power));
            }
        }

        public void ReadRelationshipFile(string path)
        {
            using (StreamReader fs = new StreamReader(path))
            {
                var str = fs.ReadLine();
                var values = str.Split(",");
                int from, to;
                double relation, frequency;
                int.TryParse(values[0], out from);
                int.TryParse(values[1], out to);

                relation = double.Parse(values[1]);
                frequency = double.Parse(values[2]);

                PeopleList[from].AddRelationship(new Relationship(from, to, relation, frequency));
            }
        }

        public void Run(int epochs)
        {
            int size = PeopleList.Count;

            for (int epoch = 0; epoch < epochs; ++epoch)
            {
                for (int i = 0; i < size; ++i)
                {
                    Meet(i);
                }

                for (int i = 0; i < size; ++i)
                {
                    for (int j = 0; j < size; ++j)
                    {
                        Rumor(i, j);
                    }
                }
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

            for (int index = 0; index < size; ++index)
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

                _model.ComputeAction(ref subjectPerson, ref targetPerson, ref subjectRelationship,
                    ref targetRelationship,
                    subjectStance, targetStance);

                subjectRelationship.HasComputed = true;
                targetRelationship.HasComputed = true;

                subjectPerson.RelationshipList[index] = subjectRelationship;
                targetPerson.SetRelationship(targetRelationship);

                PeopleList[subjectPersonId] = subjectPerson;
                PeopleList[targetPersonId] = targetPerson;
            }
        }

        public void Rumor(int subjectPersonId, int targetPersonId)
        {
            Person subjectPerson = PeopleList[subjectPersonId];
            int relationshipSize = subjectPerson.RelationshipList.Count;

            Relationship relationshipToTarget = subjectPerson.GetRelationshipTo(targetPersonId);
            double relationshipUpdateAmount = 0;

            for (int index = 0; index < relationshipSize; ++index)
            {
                if (index == targetPersonId)
                    continue;

                if (PeopleList[subjectPerson.RelationshipList[index].To].HasRelationship(targetPersonId))
                {
                    relationshipUpdateAmount += PeopleList[subjectPerson.RelationshipList[index].To]
                        .GetRelationshipTo(targetPersonId).UpdateAmount;
                }
            }

            double rumor = (relationshipToTarget.Relation + 1) * relationshipUpdateAmount;
            // TODO : How should we normalize this?
            relationshipToTarget.Relation -= rumor;
        }

        public List<Person> PeopleList { get; set; }

        private readonly Model.Model _model;

        private Random _random;
    }
}