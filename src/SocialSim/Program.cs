using System;

namespace SocialSim
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string peopleFilePath = "input\\People.csv";
            string relationshipFilePath = "input\\Relationship.csv";

            Engine.Engine engine = new Engine.Engine(new Model.Model());
            
            engine.ReadPeopleFile(peopleFilePath);
            engine.ReadRelationshipFile(relationshipFilePath);
            
            engine.Run(10, 5, "output");
        }
    }
}