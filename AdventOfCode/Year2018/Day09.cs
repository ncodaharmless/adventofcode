using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Year2018
{
    public class Day09
    {
        LinkedList<int> _Circle = new LinkedList<int>();
        LinkedListNode<int> _CurrentMarble;
        int _CurrentPlayer;

        void Next()
        {
            _CurrentMarble = _CurrentMarble.Next ?? _Circle.First;
        }

        void Previous()
        {
            _CurrentMarble = _CurrentMarble.Previous ?? _Circle.Last;
        }

        public void Run()
        {
            int playerCount = 476;
            int lastMarbleValue = 71431 * 100;

            long[] playerScore = new long[playerCount];

            _CurrentMarble = _Circle.AddLast(0);
            for (int i = 1; i < lastMarbleValue; i++)
            {
                if (i % 23 == 0)
                {
                    playerScore[_CurrentPlayer] += i;
                    for (int x = 0; x < 7; x++)
                        Previous();
                    playerScore[_CurrentPlayer] += _CurrentMarble.Value;
                    var tmp = _CurrentMarble;
                    Next();
                    _Circle.Remove(tmp);
                }
                else
                {
                    Next();
                    _CurrentMarble = _Circle.AddAfter(_CurrentMarble, i);
                }

                _CurrentPlayer = (_CurrentPlayer + 1) % playerCount;
            }

            Console.WriteLine("Max score " + playerScore.Max());
        }


    }
}