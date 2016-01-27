namespace Poker.PokerMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using Poker.Contracts;

    public class HandRanking : IHandRanking
    {
        public HandRanking()
        {
            this.Win = new List<Type>();
            this.Reserve = new int[17];
        }

        public List<Type> Win { get; private set; }

        public int[] Reserve { get; set; }

        public IType WinningHand { get; set; }

        public double Type { get; set; }

        /// <summary>
        /// From the participant's cards and these on the desk are tried to find a straingt flush.
        /// </summary>
        /// <param name="currentGameParticipant">The participant who is on turn.</param>
        public void TryFindingStraightFlush(
            IGameParticipant currentGameParticipant,
            int[] st1,
            int[] st2,
            int[] st3,
            int[] st4)
        {
            if (st1.Length >= 5)
            {
                if (st1[0] + 4 == st1[4])
                {
                    currentGameParticipant.Type = 8;
                    currentGameParticipant.Power =
                        (int)(st1.Max() / 4 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }

                if (st1[0] == 0 &&
                    st1[1] == 9 &&
                    st1[2] == 10 &&
                    st1[3] == 11 &&
                    st1[0] + 12 == st1[4])
                {
                    currentGameParticipant.Type = 9;
                    currentGameParticipant.Power =
                        (int)(st1.Max() / 4 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }

            if (st2.Length >= 5)
            {
                if (st2[0] + 4 == st2[4])
                {
                    currentGameParticipant.Type = 8;
                    currentGameParticipant.Power =
                        (int)(st2.Max() / 4 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }

                if (st2[0] == 0 && st2[1] == 9 &&
                    st2[2] == 10 && st2[3] == 11 &&
                    st2[0] + 12 == st2[4])
                {
                    currentGameParticipant.Type = 9;
                    currentGameParticipant.Power =
                        (int)(st2.Max() / 4 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }

            if (st3.Length >= 5)
            {
                if (st3[0] + 4 == st3[4])
                {
                    currentGameParticipant.Type = 8;
                    currentGameParticipant.Power =
                        (int)(st3.Max() / 4 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }

                if (st3[0] == 0 &&
                    st3[1] == 9 &&
                    st3[2] == 10 &&
                    st3[3] == 11 &&
                    st3[0] + 12 == st3[4])
                {
                    currentGameParticipant.Type = 9;
                    currentGameParticipant.Power =
                        (int)(st3.Max() / 4 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }

            if (st4.Length >= 5)
            {
                if (st4[0] + 4 == st4[4])
                {
                    currentGameParticipant.Type = 8;
                    currentGameParticipant.Power =
                        (int)(st4.Max() / 4 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }

                if (st4[0] == 0 &&
                    st4[1] == 9 &&
                    st4[2] == 10 &&
                    st4[3] == 11 &&
                    st4[0] + 12 == st4[4])
                {
                    currentGameParticipant.Type = 9;
                    currentGameParticipant.Power =
                        (int)(st4.Max() / 4 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }
        }

        /// <summary>
        /// From the participant's cards and these on the desk are tried to find a four of a kind.
        /// </summary>
        /// <param name="currentGameParticipant">The participant who is on turn.</param>
        public void TryFindingFourOfAKind(
            IGameParticipant currentGameParticipant,
            int[] straight)
        {
            //if (currentGameParticipant.Type >= -1)
            //{
            for (int j = 0; j <= 3; j++)
            {
                if (straight[j] / 4 == straight[j + 1] / 4 &&
                    straight[j] / 4 == straight[j + 2] / 4 &&
                    straight[j] / 4 == straight[j + 3] / 4)
                {
                    currentGameParticipant.Type = 7;
                    currentGameParticipant.Power =
                        (int)(straight[j] / 4 * 4 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }

                if (straight[j] / 4 == 0 &&
                    straight[j + 1] / 4 == 0 &&
                    straight[j + 2] / 4 == 0 &&
                    straight[j + 3] / 4 == 0)
                {
                    currentGameParticipant.Type = 7;
                    currentGameParticipant.Power =
                        (int)(13 * 4 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }
        }

        /// <summary>
        /// From the participant's cards and these on the desk are tried to find a full house.
        /// </summary>
        /// <param name="currentGameParticipant">The participant who is on turn.</param>
        public void TryFindingFullHouse(
            IGameParticipant currentGameParticipant,
            int[] straight)
        {
            this.Type = currentGameParticipant.Power;
            for (int j = 0; j <= 12; j++)
            {
                var fh = straight.Where(o => o / 4 == j).ToArray();
                if (fh.Length == 3)
                {
                    if (fh.Length == 2)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            currentGameParticipant.Type = 6;
                            currentGameParticipant.Power =
                                (int)(13 * 2 + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power,
                                currentGameParticipant.Type);
                            break;
                        }

                        if (fh.Max() / 4 > 0)
                        {
                            currentGameParticipant.Type = 6;
                            currentGameParticipant.Power =
                                (int)(fh.Max() / 4 * 2 + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power,
                                currentGameParticipant.Type);
                            break;
                        }
                    }

                    if (fh.Max() / 4 == 0)
                    {
                        currentGameParticipant.Power = 13;
                        j = -1;
                    }
                    else
                    {
                        currentGameParticipant.Power = fh.Max() / 4;
                        j = -1;
                    }
                }
            }

            if (currentGameParticipant.Type != 6)
            {
                currentGameParticipant.Power = (int)this.Type;
            }
        }

        /// <summary>
        /// From the participant's cards and these on the desk are tried to find a flush.
        /// </summary>
        /// <param name="currentGameParticipant">The participant who is on turn.</param>
        public void TryFindingFlush(IGameParticipant currentGameParticipant, int[] straight)
        {
            // should be here?
            var f1 = straight.Where(o => o % 4 == 0).ToArray();
            var f2 = straight.Where(o => o % 4 == 1).ToArray();
            var f3 = straight.Where(o => o % 4 == 2).ToArray();
            var f4 = straight.Where(o => o % 4 == 3).ToArray();

            if (f1.Length == 3 || f1.Length == 4)
            {
                if (this.Reserve[0] % 4 == this.Reserve[0 + 1] % 4 &&
                    this.Reserve[0] % 4 == f1[0] % 4)
                {
                    currentGameParticipant.Type = 5;

                    if (this.Reserve[0] / 4 > f1.Max() / 4)
                    {
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0] + currentGameParticipant.Type * 100);
                    }

                    if (this.Reserve[0 + 1] / 4 > f1.Max() / 4)
                    {
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0 + 1] + currentGameParticipant.Type * 100);
                    }
                    else if (this.Reserve[0] / 4 < f1.Max() / 4 &&
                        this.Reserve[0 + 1] / 4 < f1.Max() / 4)
                    {
                        currentGameParticipant.Power =
                            (int)(f1.Max() + currentGameParticipant.Type * 100);
                    }

                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }

            if (f1.Length == 4) // different cards in hand
            {
                if (this.Reserve[0] % 4 != this.Reserve[0 + 1] % 4 &&
                    this.Reserve[0] % 4 == f1[0] % 4)
                {
                    currentGameParticipant.Type = 5;

                    if (this.Reserve[0] / 4 > f1.Max() / 4)
                    {
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0] + currentGameParticipant.Type * 100);
                    }
                    else
                    {
                        currentGameParticipant.Power =
                            (int)(f1.Max() + currentGameParticipant.Type * 100);
                    }

                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }

                if (this.Reserve[0 + 1] % 4 != this.Reserve[0] % 4 &&
                    this.Reserve[0 + 1] % 4 == f1[0] % 4)
                {
                    currentGameParticipant.Type = 5;
                    if (this.Reserve[0 + 1] / 4 > f1.Max() / 4)
                    {
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0 + 1] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                    else
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(f1.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                }
            }

            if (f1.Length == 5)
            {
                if (this.Reserve[0] % 4 == f1[0] % 4 &&
                    this.Reserve[0] / 4 > f1.Min() / 4)
                {
                    currentGameParticipant.Type = 5;
                    currentGameParticipant.Power =
                        (int)(this.Reserve[0] + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }

                if (this.Reserve[0 + 1] % 4 == f1[0] % 4 &&
                    this.Reserve[0 + 1] / 4 > f1.Min() / 4)
                {
                    currentGameParticipant.Type = 5;
                    currentGameParticipant.Power =
                        (int)(this.Reserve[0 + 1] + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
                else if (this.Reserve[0] / 4 < f1.Min() / 4 &&
                    this.Reserve[0 + 1] / 4 < f1.Min())
                {
                    currentGameParticipant.Type = 5;
                    currentGameParticipant.Power =
                        (int)(f1.Max() + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }

            if (f2.Length == 3 || f2.Length == 4)
            {
                if (this.Reserve[0] % 4 == this.Reserve[0 + 1] % 4 &&
                    this.Reserve[0] % 4 == f2[0] % 4)
                {
                    if (this.Reserve[0] / 4 > f2.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }

                    if (this.Reserve[0 + 1] / 4 > f2.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0 + 1] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                    else if (this.Reserve[0] / 4 < f2.Max() / 4 &&
                        this.Reserve[0 + 1] / 4 < f2.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power = (int)(f2.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                }
            }

            if (f2.Length == 4) // different cards in hand
            {
                if (this.Reserve[0] % 4 != this.Reserve[0 + 1] % 4 &&
                    this.Reserve[0] % 4 == f2[0] % 4)
                {
                    if (this.Reserve[0] / 4 > f2.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                    else
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(f2.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                }

                if (this.Reserve[0 + 1] % 4 != this.Reserve[0] % 4 && this.Reserve[0 + 1] % 4 == f2[0] % 4)
                {
                    if (this.Reserve[0 + 1] / 4 > f2.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0 + 1] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                    else
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(f2.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                }
            }

            if (f2.Length == 5)
            {
                if (this.Reserve[0] % 4 == f2[0] % 4 &&
                    this.Reserve[0] / 4 > f2.Min() / 4)
                {
                    currentGameParticipant.Type = 5;
                    currentGameParticipant.Power =
                        (int)(this.Reserve[0] + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }

                if (this.Reserve[0 + 1] % 4 == f2[0] % 4 &&
                    this.Reserve[0 + 1] / 4 > f2.Min() / 4)
                {
                    currentGameParticipant.Type = 5;
                    currentGameParticipant.Power =
                        (int)(this.Reserve[0 + 1] + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
                else if (this.Reserve[0] / 4 < f2.Min() / 4 && this.Reserve[0 + 1] / 4 < f2.Min())
                {
                    currentGameParticipant.Type = 5;
                    currentGameParticipant.Power =
                        (int)(f2.Max() + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }

            if (f3.Length == 3 || f3.Length == 4)
            {
                if (this.Reserve[0] % 4 == this.Reserve[0 + 1] % 4 &&
                    this.Reserve[0] % 4 == f3[0] % 4)
                {
                    if (this.Reserve[0] / 4 > f3.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0] + currentGameParticipant.Type * 100);
                        this.AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }

                    if (this.Reserve[0 + 1] / 4 > f3.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0 + 1] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                    else if (this.Reserve[0] / 4 < f3.Max() / 4 && this.Reserve[0 + 1] / 4 < f3.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(f3.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                }
            }

            if (f3.Length == 4)//different cards in hand
            {
                if (this.Reserve[0] % 4 != this.Reserve[0 + 1] % 4 &&
                    this.Reserve[0] % 4 == f3[0] % 4)
                {
                    if (this.Reserve[0] / 4 > f3.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                    else
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(f3.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                }

                if (this.Reserve[0 + 1] % 4 != this.Reserve[0] % 4 &&
                    this.Reserve[0 + 1] % 4 == f3[0] % 4)
                {
                    if (this.Reserve[0 + 1] / 4 > f3.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0 + 1] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                    else
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(f3.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                }
            }

            if (f3.Length == 5)
            {
                if (this.Reserve[0] % 4 == f3[0] % 4 &&
                    this.Reserve[0] / 4 > f3.Min() / 4)
                {
                    currentGameParticipant.Type = 5;
                    currentGameParticipant.Power =
                        (int)(this.Reserve[0] + currentGameParticipant.Type * 100);
                    this.AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                }

                if (this.Reserve[0 + 1] % 4 == f3[0] % 4 &&
                    this.Reserve[0 + 1] / 4 > f3.Min() / 4)
                {
                    currentGameParticipant.Type = 5;
                    currentGameParticipant.Power =
                        (int)(this.Reserve[0 + 1] + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
                else if (this.Reserve[0] / 4 < f3.Min() / 4 &&
                    this.Reserve[0 + 1] / 4 < f3.Min())
                {
                    currentGameParticipant.Type = 5;
                    currentGameParticipant.Power =
                        (int)(f3.Max() + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }

            if (f4.Length == 3 || f4.Length == 4)
            {
                if (this.Reserve[0] % 4 == this.Reserve[0 + 1] % 4 &&
                    this.Reserve[0] % 4 == f4[0] % 4)
                {
                    if (this.Reserve[0] / 4 > f4.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }

                    if (this.Reserve[0 + 1] / 4 > f4.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0 + 1] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                    else if (this.Reserve[0] / 4 < f4.Max() / 4 &&
                        this.Reserve[0 + 1] / 4 < f4.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(f4.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                }
            }

            if (f4.Length == 4)//different cards in hand
            {
                if (this.Reserve[0] % 4 != this.Reserve[0 + 1] % 4 &&
                    this.Reserve[0] % 4 == f4[0] % 4)
                {
                    if (this.Reserve[0] / 4 > f4.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                    else
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(f4.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                }

                if (this.Reserve[0 + 1] % 4 != this.Reserve[0] % 4 &&
                    this.Reserve[0 + 1] % 4 == f4[0] % 4)
                {
                    if (this.Reserve[0 + 1] / 4 > f4.Max() / 4)
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(this.Reserve[0 + 1] + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                    else
                    {
                        currentGameParticipant.Type = 5;
                        currentGameParticipant.Power =
                            (int)(f4.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                }
            }

            if (f4.Length == 5)
            {
                if (this.Reserve[0] % 4 == f4[0] % 4 &&
                    this.Reserve[0] / 4 > f4.Min() / 4)
                {
                    currentGameParticipant.Type = 5;
                    currentGameParticipant.Power =
                        (int)(this.Reserve[0] + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }

                if (this.Reserve[0 + 1] % 4 == f4[0] % 4 &&
                    this.Reserve[0 + 1] / 4 > f4.Min() / 4)
                {
                    currentGameParticipant.Type = 5;
                    currentGameParticipant.Power =
                        (int)(this.Reserve[0 + 1] + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
                else if (this.Reserve[0] / 4 < f4.Min() / 4 &&
                    this.Reserve[0 + 1] / 4 < f4.Min())
                {
                    currentGameParticipant.Type = 5;
                    currentGameParticipant.Power =
                        (int)(f4.Max() + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }

            // ace
            if (f1.Length > 0)
            {
                if (this.Reserve[0] / 4 == 0 &&
                    this.Reserve[0] % 4 == f1[0] % 4 &&
                    f1.Length > 0)
                {
                    currentGameParticipant.Type = 5.5;
                    currentGameParticipant.Power =
                        (int)(13 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }

                if (this.Reserve[0 + 1] / 4 == 0 &&
                    this.Reserve[0 + 1] % 4 == f1[0] % 4
                    && f1.Length > 0)
                {
                    currentGameParticipant.Type = 5.5;
                    currentGameParticipant.Power =
                        (int)(13 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }

            if (f2.Length > 0)
            {
                if (this.Reserve[0] / 4 == 0 &&
                    this.Reserve[0] % 4 == f2[0] % 4 &&
                    f2.Length > 0)
                {
                    currentGameParticipant.Type = 5.5;
                    currentGameParticipant.Power =
                        (int)(13 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }

                if (this.Reserve[0 + 1] / 4 == 0 &&
                    this.Reserve[0 + 1] % 4 == f2[0] % 4 &&
                    f2.Length > 0)
                {
                    currentGameParticipant.Type = 5.5;
                    currentGameParticipant.Power =
                        (int)(13 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }

            if (f3.Length > 0)
            {
                if (this.Reserve[0] / 4 == 0 &&
                    this.Reserve[0] % 4 == f3[0] % 4 &&
                    f3.Length > 0)
                {
                    currentGameParticipant.Type = 5.5;
                    currentGameParticipant.Power =
                        (int)(13 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }

                if (this.Reserve[0 + 1] / 4 == 0 &&
                    this.Reserve[0 + 1] % 4 == f3[0] % 4 &&
                    f3.Length > 0)
                {
                    currentGameParticipant.Type = 5.5;
                    currentGameParticipant.Power =
                        (int)(13 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }

            if (f4.Length > 0)
            {
                if (this.Reserve[0] / 4 == 0 &&
                    this.Reserve[0] % 4 == f4[0] % 4 &&
                    f4.Length > 0)
                {
                    currentGameParticipant.Type = 5.5;
                    currentGameParticipant.Power =
                        (int)(13 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }

                if (this.Reserve[0 + 1] / 4 == 0 &&
                    this.Reserve[0 + 1] % 4 == f4[0] % 4)
                {
                    currentGameParticipant.Type = 5.5;
                    currentGameParticipant.Power =
                        (int)(13 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }
        }

        /// <summary>
        /// From the participant's cards and these on the desk are tried to find a straingt.
        /// </summary>
        /// <param name="currentGameParticipant">The participant who is on turn.</param>
        public void TryFindingStraight(
            IGameParticipant currentGameParticipant,
            int[] straight)
        {
            var op = straight.Select(o => o / 4).Distinct().ToArray();
            for (int j = 0; j < op.Length - 4; j++)
            {
                if (op[j] + 4 == op[j + 4])
                {
                    if (op.Max() - 4 == op[j])
                    {
                        currentGameParticipant.Type = 4;
                        currentGameParticipant.Power =
                            (int)(op.Max() + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                    else
                    {
                        currentGameParticipant.Type = 4;
                        currentGameParticipant.Power =
                            (int)(op[j + 4] + currentGameParticipant.Type * 100);
                        this.AddWin(currentGameParticipant.Power, currentGameParticipant.Type);
                    }
                }

                if (op[j] == 0 &&
                    op[j + 1] == 9 &&
                    op[j + 2] == 10 &&
                    op[j + 3] == 11 &&
                    op[j + 4] == 12)
                {
                    currentGameParticipant.Type = 4;
                    currentGameParticipant.Power =
                        (int)(13 + currentGameParticipant.Type * 100);
                    this.AddWin(
                        currentGameParticipant.Power, currentGameParticipant.Type);
                }
            }
        }

        /// <summary>
        /// From the participant's cards and these on the desk are tried to find a three of a kind.
        /// </summary>
        /// <param name="currentGameParticipant">The participant who is on turn.</param>
        public void TryFindingThreeOfAKind(IGameParticipant currentGameParticipant, int[] straight)
        {
            for (int j = 0; j <= 12; j++)
            {
                var fh = straight.Where(o => o / 4 == j).ToArray();
                if (fh.Length == 3)
                {
                    if (fh.Max() / 4 == 0)
                    {
                        currentGameParticipant.Type = 3;
                        currentGameParticipant.Power =
                            (int)(13 * 3 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power, currentGameParticipant.Type);
                    }
                    else
                    {
                        currentGameParticipant.Type = 3;
                        currentGameParticipant.Power =
                            (int)(fh[0] / 4 + fh[1] / 4 + fh[2] / 4 +
                            currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                }
            }
        }

        /// <summary>
        /// From the participant's cards and these on the desk are tried to find a two pair.
        /// </summary>
        /// <param name="currentGameParticipant">The participant who is on turn.</param>
        public void TryFindingTwoPair(IGameParticipant currentGameParticipant)
        {
            for (int tc = 16; tc >= 12; tc--)
            {
                int max = tc - 12;
                if (this.Reserve[0] / 4 != this.Reserve[0 + 1] / 4)
                {
                    for (int k = 1; k <= max; k++)
                    {
                        if (tc - k < 12)
                        {
                            max--;
                        }
                        else
                        {
                            if (this.Reserve[0] / 4 == this.Reserve[tc] / 4 &&
                                this.Reserve[0 + 1] / 4 == this.Reserve[tc - k] / 4 ||
                                this.Reserve[0 + 1] / 4 == this.Reserve[tc] / 4 &&
                                this.Reserve[0] / 4 == this.Reserve[tc - k] / 4)
                            {
                                if (this.Reserve[0] / 4 == 0)
                                {
                                    currentGameParticipant.Type = 2;
                                    currentGameParticipant.Power =
                                        (int)(13 * 4 + (this.Reserve[0 + 1] / 4) * 2 +
                                        currentGameParticipant.Type * 100);
                                    this.AddWin(
                                        currentGameParticipant.Power,
                                        currentGameParticipant.Type);
                                }

                                if (this.Reserve[0 + 1] / 4 == 0)
                                {
                                    currentGameParticipant.Type = 2;
                                    currentGameParticipant.Power =
                                        (int)(13 * 4 + (this.Reserve[0] / 4) * 2 +
                                        currentGameParticipant.Type * 100);
                                    this.AddWin
                                        (currentGameParticipant.Power,
                                        currentGameParticipant.Type);
                                }

                                if (this.Reserve[0 + 1] / 4 != 0 &&
                                    this.Reserve[0] / 4 != 0)
                                {
                                    currentGameParticipant.Type = 2;
                                    currentGameParticipant.Power =
                                        (int)((this.Reserve[0] / 4) * 2 +
                                        (this.Reserve[0 + 1] / 4) * 2 + currentGameParticipant.Type * 100);
                                    this.AddWin(
                                        currentGameParticipant.Power,
                                        currentGameParticipant.Type);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// From the participant's cards and these on the desk are tried to find a pair two pair.
        /// </summary>
        /// <param name="currentGameParticipant">The participant who is on turn.</param>
        public void TryFindingPairTwoPair(IGameParticipant currentGameParticipant)
        {
            for (int tc = 16; tc >= 12; tc--)
            {
                int max = tc - 12;
                for (int k = 1; k <= max; k++)
                {
                    if (tc - k < 12)
                    {
                        max--;
                    }

                    if (tc - k >= 12)
                    {
                        if (this.Reserve[tc] / 4 == this.Reserve[tc - k] / 4)
                        {
                            if (this.Reserve[tc] / 4 != this.Reserve[0] / 4 &&
                                this.Reserve[tc] / 4 != this.Reserve[0 + 1] / 4 &&
                                currentGameParticipant.Type == 1)
                            {
                                if (this.Reserve[0 + 1] / 4 == 0)
                                {
                                    currentGameParticipant.Type = 2;
                                    currentGameParticipant.Power =
                                        (int)((this.Reserve[0] / 4) * 2 +
                                        13 * 4 + currentGameParticipant.Type * 100);
                                    this.AddWin(
                                        currentGameParticipant.Power,
                                        currentGameParticipant.Type);
                                }

                                if (this.Reserve[0] / 4 == 0)
                                {
                                    currentGameParticipant.Type = 2;
                                    currentGameParticipant.Power =
                                        (int)((this.Reserve[0 + 1] / 4) * 2 +
                                        13 * 4 + currentGameParticipant.Type * 100);
                                    this.AddWin(
                                        currentGameParticipant.Power,
                                        currentGameParticipant.Type);
                                }

                                if (this.Reserve[0 + 1] / 4 != 0)
                                {
                                    currentGameParticipant.Type = 2;
                                    currentGameParticipant.Power =
                                        (int)((this.Reserve[tc] / 4) * 2 +
                                        (this.Reserve[0 + 1] / 4) * 2 + currentGameParticipant.Type * 100);
                                    this.AddWin(
                                        currentGameParticipant.Power,
                                        currentGameParticipant.Type);
                                }

                                if (this.Reserve[0] / 4 != 0)
                                {
                                    currentGameParticipant.Type = 2;
                                    currentGameParticipant.Power =
                                        (int)((this.Reserve[tc] / 4) * 2 +
                                        (this.Reserve[0] / 4) * 2 + currentGameParticipant.Type * 100);
                                    this.AddWin(
                                        currentGameParticipant.Power,
                                        currentGameParticipant.Type);
                                }
                            }

                            if (currentGameParticipant.Type == -1)
                            {

                                if (this.Reserve[0] / 4 > this.Reserve[0 + 1] / 4)
                                {
                                    if (this.Reserve[tc] / 4 == 0)
                                    {
                                        currentGameParticipant.Type = 0;
                                        currentGameParticipant.Power =
                                            (int)(13 + this.Reserve[0] / 4 +
                                            currentGameParticipant.Type * 100);
                                        // using AddWin method with current value = 1 was intended
                                        // to match with default game logic 
                                        this.AddWin(currentGameParticipant.Power, 1);
                                    }
                                    else
                                    {
                                        currentGameParticipant.Type = 0;
                                        currentGameParticipant.Power =
                                            (int)(this.Reserve[tc] / 4 +
                                            this.Reserve[0] / 4 + currentGameParticipant.Type * 100);
                                        this.AddWin(currentGameParticipant.Power, 1);
                                    }
                                }
                                else
                                {
                                    if (this.Reserve[tc] / 4 == 0)
                                    {
                                        currentGameParticipant.Type = 0;
                                        currentGameParticipant.Power =
                                            (int)(13 + this.Reserve[0 + 1] +
                                            currentGameParticipant.Type * 100);
                                        this.AddWin(currentGameParticipant.Power, 1);
                                    }
                                    else
                                    {
                                        currentGameParticipant.Type = 0;
                                        currentGameParticipant.Power =
                                            (int)(this.Reserve[tc] / 4 + this.Reserve[0 + 1] / 4 +
                                            currentGameParticipant.Type * 100);
                                        this.AddWin(currentGameParticipant.Power, 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// From the participant's cards and these on the desk are tried to find only a pair.
        /// </summary>
        /// <param name="currentGameParticipant">The participant who is on turn.</param>
        public void TryFindingPairFromHand(IGameParticipant currentGameParticipant)
        {
            if (this.Reserve[0] / 4 == this.Reserve[0 + 1] / 4)
            {
                    if (this.Reserve[0] / 4 == 0)
                    {
                        currentGameParticipant.Type = 1;
                        currentGameParticipant.Power =
                            (int)(13 * 4 + currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
                    else
                    {
                        currentGameParticipant.Type = 1;
                        currentGameParticipant.Power =
                            (int)((this.Reserve[0 + 1] / 4) * 4 +
                            currentGameParticipant.Type * 100);
                        this.AddWin(
                            currentGameParticipant.Power,
                            currentGameParticipant.Type);
                    }
            }

            for (int tc = 16; tc >= 12; tc--)
            {
                if (this.Reserve[0 + 1] / 4 == this.Reserve[tc] / 4)
                {
                        if (this.Reserve[0 + 1] / 4 == 0)
                        {
                            currentGameParticipant.Type = 1;
                            currentGameParticipant.Power =
                                (int)(13 * 4 + this.Reserve[0] / 4 +
                                currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power,
                                currentGameParticipant.Type);
                        }
                        else
                        {
                            currentGameParticipant.Type = 1;
                            currentGameParticipant.Power =
                                (int)((this.Reserve[0 + 1] / 4) * 4 +
                                this.Reserve[0] / 4 + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power,
                                currentGameParticipant.Type);
                        }
                }

                if (this.Reserve[0] / 4 == this.Reserve[tc] / 4)
                {
                        if (this.Reserve[0] / 4 == 0)
                        {
                            currentGameParticipant.Type = 1;
                            currentGameParticipant.Power = (int)(13 * 4 +
                                this.Reserve[0 + 1] / 4 + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power,
                                currentGameParticipant.Type);
                        }
                        else
                        {
                            currentGameParticipant.Type = 1;
                            currentGameParticipant.Power =
                                (int)((this.Reserve[tc] / 4) * 4 +
                                this.Reserve[0 + 1] / 4 + currentGameParticipant.Type * 100);
                            this.AddWin(
                                currentGameParticipant.Power,
                                currentGameParticipant.Type);
                        }
                }
            }
        }

        /// <summary>
        /// From the participant's cards and these on the desk are tried to find the highest card.
        /// </summary>
        /// <param name="currentGameParticipant">The participant who is on turn.</param>
        public void TryFindingHighCard(IGameParticipant currentGameParticipant)
        {
            if (currentGameParticipant.Type == -1)
            {

                if (this.Reserve[0]/4 > this.Reserve[0 + 1]/4)
                {
                    currentGameParticipant.Type = -1;
                    currentGameParticipant.Power = this.Reserve[0]/4;
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
                else
                {
                    currentGameParticipant.Type = -1;
                    currentGameParticipant.Power =
                        this.Reserve[0 + 1]/4;
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }

                if (this.Reserve[0]/4 == 0 || 
                    this.Reserve[0 + 1]/4 == 0)
                {
                    currentGameParticipant.Type = -1;
                    currentGameParticipant.Power = 13;
                    this.AddWin(
                        currentGameParticipant.Power,
                        currentGameParticipant.Type);
                }
            }
        }

        /// <summary>
        /// Adds a winning hand to the collection of <see cref="IType"/>.
        /// </summary>
        /// <param botIndex="power"></param>
        /// <param botIndex="current"></param>
        public void AddWin(int power, double current)
        {
            Type typeToAdd = new Type(power, current);
            this.Win.Add(typeToAdd);
            // Returns the best hand so far by sorting the Win collection.
            this.WinningHand = this.Win
                .OrderByDescending(op1 => op1.Current)
                .ThenByDescending(op1 => op1.Power)
                .First();
        }
    }
}
