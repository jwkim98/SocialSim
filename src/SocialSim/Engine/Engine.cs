using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Markup;
using SocialSim.Elements;
using SocialSim.Model;


namespace SocialSim.Engine
{
    /// <summary>
    /// This class computes given relationships and people with _simulationModel given as a parameter
    /// </summary>
    class Engine
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="simulationModel"> Model to use for simulation </param>
        public Engine(Model.Model simulationModel)
        {
            _simulationModel = simulationModel;
        }

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
            using StreamReader fs = new StreamReader(path);
            var str = fs.ReadLine();
            var values = str.Split(",");

            int.TryParse(values[0], out int groupId);
            int.TryParse(values[1], out int id);
            int.TryParse(values[4], out int money);
            int.TryParse(values[5], out int power);

            float selflessness = float.Parse(values[2]);
            float selfishness = float.Parse(values[3]);

            PeopleList.Add(new Person(groupId, id, selflessness, selfishness, money, power));
        }

        public void ReadRelationshipFile(string path)
        {
            using StreamReader fs = new StreamReader(path);
            var str = fs.ReadLine();
            var values = str.Split(",");

            int.TryParse(values[0], out int from);
            int.TryParse(values[1], out int to);

            double relation = double.Parse(values[1]);
            double frequency = double.Parse(values[2]);

            PeopleList[from].AddRelationship(new Relationship(from, to, relation, frequency));
        }

        public void WritePeopleFile(String filename)
        {
            var csv = new StringBuilder();
            
            foreach(Person person in PeopleList)
            {
                var newline = String.Format("{0},{1},{2},{3},{4}", person.Id.ToString(),
                    person.Selflessness.ToString(), person.Selflessness.ToString(), person.Money.ToString(),
                    person.Power.ToString());
                csv.Append(newline);
            }
            File.WriteAllText(filename, csv.ToString());
        }

        public void WriteRelationshipFile(String filename)
        {
            var csv = new StringBuilder();

            foreach (Person person in PeopleList)
            {
                foreach (Relationship relationship in person.RelationshipList)
                {
                    var newline = String.Format("{0},{1},{2},{3}", relationship.From.ToString(),
                        relationship.To.ToString(), relationship.Relation.ToString(), relationship.Frequency.ToString());
                    csv.Append(newline);
                }
            }
            File.WriteAllText(filename, csv.ToString());
        }

        public void Run(int epochs, int writeDuration, string outputDir)
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

                if (epochs % writeDuration == 0)
                {
                    var peopleFilePath = "People_" + (epochs / writeDuration).ToString();
                    var relationshipFilePath = "Relationship_" + (epochs / writeDuration).ToString();
                    
                    WritePeopleFile(Path.Combine(outputDir, peopleFilePath));
                    WriteRelationshipFile(Path.Combine(outputDir, relationshipFilePath));
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

                double subjectActionDegree = _simulationModel.ComputeActionDegree(subjectPerson, subjectRelationship);
                double targetActionDegree = _simulationModel.ComputeActionDegree(targetPerson, targetRelationship);

                Stance subjectStance = _simulationModel.GetStance(subjectActionDegree);
                Stance targetStance = _simulationModel.GetStance(targetActionDegree);

                _simulationModel.ComputeAction(ref subjectPerson, ref targetPerson, ref subjectRelationship,
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

            double rumor = (relationshipToTarget.Frequency) * relationshipUpdateAmount;
            // TODO : How should we normalize this?
            relationshipToTarget.Relation -= (rumor*4)/PeopleList.Count;
        }

        public List<Person> PeopleList { get; set; }

        private readonly Model.Model _simulationModel;

        private readonly Random _random = new Random();
    }
}