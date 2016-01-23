namespace Poker
{
    public class Type
    {
        public Type(double power, double current)
        {
            this.Power = power;
            this.Current = current;
        }

        public double Power { get; set; }

        public double Current { get; set; }
    }
}
