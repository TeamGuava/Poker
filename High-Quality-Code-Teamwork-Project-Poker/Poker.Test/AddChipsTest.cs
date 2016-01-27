namespace Poker.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AddChipsTest
    {
        private AddChips addChips;

        [TestInitialize]
        public void TestInnitialize()
        {
            addChips = new AddChips();
        }

        [TestCleanup]
        public void TestCleanup()
        {       
        }

        [TestMethod]
        public void NewChips_NegativeValue_Validation()
        {
            this.addChips.NewChips = -1;

            Assert.AreEqual(0, this.addChips.NewChips, "The chips value cannot be negative.");
        }

        [TestMethod]
        [Ignore]
        public void NewChips_AddingValueAboveTheMaximum()
        {
            const int MaxNumberOfChipsToAdd = 100 * 1000 * 1000;

            this.addChips.NewChips = MaxNumberOfChipsToAdd + 1;

            Assert.AreEqual(0, addChips.NewChips, "The were not added because they were above the maximum value.");
        }
    }
}
