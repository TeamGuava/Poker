using Poker.Contracts;

namespace Poker
{
    public class Type : IType
    {
        public Type(int power, double current)
        {
            this.Power = power;
            this.Current = current;
        }

        public int Power { get; set; }

        public double Current { get; set; }
    }
}
