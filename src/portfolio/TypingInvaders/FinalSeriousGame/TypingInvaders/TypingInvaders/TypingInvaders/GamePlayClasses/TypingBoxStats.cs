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
    public class TypingBoxStats
    {
        public int NumberOfLettersInWord;
        public List<float> TimeBetweenEachCorrectStroke;       
        public float TimeStartedWord;
        public float TimeFinishedWord;
        public int NumberOfMistakes;
        
        public float TimeToCompleteWord;
        public float AvgTimeBetweenCorrectStrokes;
        public float Rhythm;
        public float PercentageCorrect;
        
        public TypingBoxStats()
        {
            AvgTimeBetweenCorrectStrokes = 0;
            TimeToCompleteWord = 0;
            TimeStartedWord = 0;
            TimeFinishedWord = 0;
            NumberOfMistakes = 0;
            Rhythm = 0;
            PercentageCorrect = 0;
            NumberOfLettersInWord = 0;
            TimeBetweenEachCorrectStroke = new List<float>();
        }

        public void CalculateStats()
        {
            #region Zero out values
            AvgTimeBetweenCorrectStrokes = 0;
            Rhythm = 0;
            PercentageCorrect = 0;
            #endregion

            #region Calculate TimeToCompleteWord
            TimeToCompleteWord = TimeFinishedWord - TimeStartedWord;
            #endregion

            #region Calculate AvgTimeBetweenCorrectStrokes
            foreach (float time in TimeBetweenEachCorrectStroke)
            {
                AvgTimeBetweenCorrectStrokes += time;
            }
            AvgTimeBetweenCorrectStrokes = AvgTimeBetweenCorrectStrokes / TimeBetweenEachCorrectStroke.Count;
            #endregion

            #region Calculate PercentageCorrect
            PercentageCorrect = (float)NumberOfLettersInWord / (float)(NumberOfMistakes + NumberOfLettersInWord);
            #endregion

            #region Calculate Rhythm
            List<float> DifferenceFromAvg = new List<float>();

            foreach (float time in TimeBetweenEachCorrectStroke)
            {
                DifferenceFromAvg.Add((float)Math.Abs(time - AvgTimeBetweenCorrectStrokes));
            }

            foreach (float time in DifferenceFromAvg)
            {
                Rhythm += time;
            }

            Rhythm = Rhythm / DifferenceFromAvg.Count;
            #endregion
        }

        public void ZeroOutValues()
        {
            NumberOfLettersInWord = 0;
            TimeBetweenEachCorrectStroke = new List<float>();
            TimeToCompleteWord = 0;
            TimeStartedWord = 0;
            TimeFinishedWord = 0;
            NumberOfMistakes = 0;

            AvgTimeBetweenCorrectStrokes = 0;
            Rhythm = 0;
            PercentageCorrect = 0;
        }
    }
}
