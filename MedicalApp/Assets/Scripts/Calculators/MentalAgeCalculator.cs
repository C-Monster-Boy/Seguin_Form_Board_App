using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MedicalApp.Core
{
    /***
     * Based on mental age table shown in this video https://www.youtube.com/watch?v=8FZFP7_Q-XQ
     * Based on shortest solve time
     */
    public class MentalAgeCalculator
    {
        Dictionary<int, TimeRange> mentalAgeRefDict;

        public MentalAgeCalculator()
        {
            mentalAgeRefDict = new Dictionary<int, TimeRange>()
            {
                [3] = new TimeRange(58.0f, float.PositiveInfinity),
                [4] = new TimeRange(48.0f, 58.0f),
                [5] = new TimeRange(36.3f, 48.0f),
                [6] = new TimeRange(27.2f, 36.3f),
                [7] = new TimeRange(25.0f, 27.2f),
                [8] = new TimeRange(21.0f, 25.0f),
                [9] = new TimeRange(18.9f, 21.0f),
                [10] = new TimeRange(17.4f, 18.9f),
                [11] = new TimeRange(16.2f, 17.4f),
                [12] = new TimeRange(15.7f, 16.2f),
                [13] = new TimeRange(14.5f, 15.7f),
                [14] = new TimeRange(14.2f, 14.5f),
                [15] = new TimeRange(13.8f, 14.2f),
                [16] = new TimeRange(0, 13.8f)
            };
        }

        public int GetMentalAge(float seguinSolveTime)
        {
            foreach (KeyValuePair<int, TimeRange> entry in mentalAgeRefDict)
            {
                if (entry.Value.IsInRange(seguinSolveTime))
                {
                    return entry.Key;
                }
            }

            return -1;
        }

    }

    public class TimeRange
    {
        public float min;
        public float max;

        public TimeRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public bool IsInRange(float time)
        {
            return time >= min && time < max;
        }

    }
}

