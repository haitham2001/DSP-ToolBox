using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public struct Range
        {
            public float Start, End, Midpoint;
        }
        public override void Run()
        {
            OutputEncodedSignal = new List<string>();
            OutputIntervalIndices = new List<int>();
            OutputQuantizedSignal = new Signal(new List<float>(), false);
            OutputSamplesError = new List<float>();

            if (InputNumBits > 0)
                InputLevel = (int)Math.Pow((double)2, (double)InputNumBits);
            else if (InputLevel > 0)
                InputNumBits = (int)Math.Log(InputLevel, 2);

            float Max_Amp = InputSignal.Samples.Max();
            float Min_Amp = InputSignal.Samples.Min();
            float res = (Max_Amp - Min_Amp) / InputLevel;

            List<Range> Level = new List<Range>();
            for (int i = 1; i < InputLevel + 1; i++)
            {
                Level[i].Start.Equals(Min_Amp);
                Min_Amp += res;
                Level[i].End.Equals(Min_Amp);
                Level[i].Midpoint.Equals((Level[i].Start + Level[i].End) / 2);
            }

            for (int i = 0; i < InputSignal.Samples.Count(); i++)
            {
                for (int j = 1; j < InputLevel + 1; j++)
                {
                    if (InputSignal.Samples[i] >= Level[j].Start && InputSignal.Samples[i] <= Level[j].End)
                    {
                        OutputIntervalIndices.Add(j);
                        OutputQuantizedSignal.Samples.Add(Level[j].Midpoint);
                        OutputSamplesError.Add(Level[j].Midpoint - InputSignal.Samples[i]);
                    }
                }

            }
        }
    }
}
