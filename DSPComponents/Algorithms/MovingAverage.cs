using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            OutputAverageSignal = new Signal(new List<float>(), false);
            for(int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float y = 0;

                if (i + InputWindowSize > InputSignal.Samples.Count)
                    break;

                for(int j = 0; j < InputWindowSize; j++)
                    y += InputSignal.Samples[i + j];

                y /= InputWindowSize;
                OutputAverageSignal.Samples.Add(y);
            }
        }
    }
}
