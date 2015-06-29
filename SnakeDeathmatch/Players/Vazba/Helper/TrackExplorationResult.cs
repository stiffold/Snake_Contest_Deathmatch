using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Players.Vazba.Strategies;

namespace SnakeDeathmatch.Players.Vazba.Helper
{
    public class TrackExplorationResult
    {
        public int Depth;
        public decimal AliveProbability;

        public TrackExplorationResult(int depth, decimal aliveProbability)
        {
            Depth = depth;
            AliveProbability = aliveProbability;
        }

        public override string ToString()
        {
            return string.Format("Depth: {0}, AliveProbability: {1:0.0000000000}", Depth, AliveProbability);
        }

        public int CompareTo(TrackExplorationResult other)
        {
            if (Depth > other.Depth)
                return 1;

            if (Depth == other.Depth && AliveProbability > other.AliveProbability)
                return 1;

            if (Depth == other.Depth && AliveProbability == other.AliveProbability)
                return 0;

            return -1;
        }

        public static bool operator <(TrackExplorationResult result1, TrackExplorationResult result2)
        {
            return result1.CompareTo(result2) < 0;
        }

        public static bool operator >(TrackExplorationResult result1, TrackExplorationResult result2)
        {
            return result1.CompareTo(result2) > 0;
        }

        public static bool operator >=(TrackExplorationResult result1, TrackExplorationResult result2)
        {
            return result1.CompareTo(result2) >= 0;
        }

        public static bool operator <=(TrackExplorationResult result1, TrackExplorationResult result2)
        {
            return result1.CompareTo(result2) <= 0;
        }

        public static bool operator ==(TrackExplorationResult result1, TrackExplorationResult result2)
        {
            return result1.Equals(result2);
        }

        public static bool operator !=(TrackExplorationResult result1, TrackExplorationResult result2)
        {
            return !result1.Equals(result2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return CompareTo((TrackExplorationResult)obj) == 0;
        }

        public override int GetHashCode()
        {
            return Depth.GetHashCode() * 17 + AliveProbability.GetHashCode();
        }

        public static TrackExplorationResult BestPossibleResult { get { return new TrackExplorationResult(Strategy2.MyWTF, 1); } }
        public static TrackExplorationResult WorstPossibleResult { get { return new TrackExplorationResult(0, 0); } }
    }
}
