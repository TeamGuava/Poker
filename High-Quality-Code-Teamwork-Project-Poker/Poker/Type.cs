namespace Poker
{
    public class Type
    {
        public Type(double power, double current)
        {
            Power = power;
            Current = current;
        }

        public double Power { get; set; }

        public double Current { get; set; }
    }
}
