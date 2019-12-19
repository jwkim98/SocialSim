using System;
using System.Collections.Generic;
using System.Text;

namespace SocialSim.Model
{
    class Hyperparameter
    {
        /// <summary>
        ///  Degree of random value being multiplied
        /// </summary>
        public static readonly double RandomDegree = 0.5;

        public static readonly double RelationDegree = 1;

        public static readonly int MaxRelationshipCount = 100;

        public static readonly int MinimumStealBetweenEvil = 100;
        public static readonly int MaximumStealBetweenEvil = 150;

        public static readonly int MinimumStealBetweenGoodEvil = 150;
        public static readonly int MaximumStealBetweenGoodEvil = 250;

        public static readonly double MinimumRelationshipDecreaseBetweenEvil = 0.1;
        public static readonly double MinimumRelationshipDecreaseGoodEvil = 0.2;

        public static readonly double RelationshipIncreaseBetweenGood = 1.5;
        public static readonly double RelationshipDecreaseRatio = 0.01;

        public static readonly double StanceThresholdBetweenGoodEvil = 0.0;

        public static readonly double RelationshipRatioToFrequency = 0.3;

        public static readonly double OtherPersonRatio = 1.0 / 75;
    }
}
