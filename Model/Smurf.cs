namespace Model
{
    using System;

    public interface ISmurf
    {
        Guid   Id      { get; set; }
        string Ability { get; set; }
    }

    public class Smurf : ISmurf
    {
        public Guid   Id      { get; set; }
        public string Ability { get; set; }
    }

    public class SmurfLeader : Smurf
    {
        public int Rank { get; set; }
    }

    public class PapaSmurf : SmurfLeader
    {
        public int Age { get; set; }
    }

    public class Smurfette : Smurf
    {
        public int Suitors { get; set; }
    }

    public class HeftySmurf : SmurfLeader
    {
        public double Weight { get; set; }
    }

    public class BrainySmurf : Smurf
    {
        public int QI { get; set; }
    }
}