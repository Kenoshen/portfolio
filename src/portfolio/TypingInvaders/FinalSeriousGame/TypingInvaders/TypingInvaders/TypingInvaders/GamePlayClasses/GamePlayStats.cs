using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TypingInvaders
{
    public class GamePlayStats
    {
        public int NumberOfWords;
        public int NumberOfMistakes;
        public float WordsPerMinute;
        public float StartTime;
        public float EndTime;
        public float TotalTime;
        public float AvgRhythm;
        public float TotalPercentageCorrect;

        public List<TypingBoxStats> EachWordStats;

        public GamePlayStats()
        {
            NumberOfWords = 0;
            NumberOfMistakes = 0;
            WordsPerMinute = 0;
            StartTime = 0;
            EndTime = 0;
            TotalTime = 0;
            AvgRhythm = 0;
            TotalPercentageCorrect = 0;

            EachWordStats = new List<TypingBoxStats>();
        }

        public void CalculateStats()
        {
            ZeroOutValues();

            #region Calculate NumberOfWords
            NumberOfWords = EachWordStats.Count;
            #endregion

            #region Calculate NumberOfMistakes
            foreach (TypingBoxStats stat in EachWordStats)
            {
                NumberOfMistakes += stat.NumberOfMistakes;
            }
            #endregion

            #region Calculate Times
            StartTime = EachWordStats[0].TimeStartedWord;
            EndTime = EachWordStats[EachWordStats.Count - 1].TimeFinishedWord;
            TotalTime = EndTime - StartTime;
            #endregion

            #region Calculate WordsPerMinute
            WordsPerMinute = TotalTime / NumberOfWords;
            WordsPerMinute /= 1000f;
            WordsPerMinute = 60f / WordsPerMinute;
            WordsPerMinute = (float)Math.Floor(WordsPerMinute);
            #endregion

            #region Calculate TotalPercentageCorrect
            foreach (TypingBoxStats stat in EachWordStats)
            {
                TotalPercentageCorrect += stat.PercentageCorrect;
            }
            TotalPercentageCorrect = TotalPercentageCorrect / NumberOfWords;
            TotalPercentageCorrect *= 100;
            TotalPercentageCorrect = (float)Math.Ceiling(TotalPercentageCorrect);
            #endregion

            #region Calculate AvgRhythm
            foreach (TypingBoxStats stat in EachWordStats)
            {
                AvgRhythm += stat.Rhythm;
            }
            AvgRhythm = AvgRhythm / NumberOfWords;
            AvgRhythm = (float)Math.Floor(AvgRhythm);
            #endregion
        }

        public void ZeroOutValues()
        {
            NumberOfWords = 0;
            NumberOfMistakes = 0;
            WordsPerMinute = 0;
            StartTime = 0;
            EndTime = 0;
            TotalTime = 0;
            AvgRhythm = 0;
            TotalPercentageCorrect = 0;
        }

        public void ZeroOutGameStats()
        {
            EachWordStats = new List<TypingBoxStats>();
        }
    }
}

