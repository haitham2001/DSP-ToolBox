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

            float Max = InputSignal.Samples.Max();
            float Min = InputSignal.Samples.Min();
            float res = (Max - Min) / InputLevel;

            List<Range> Level = new List<Range>();
            Range r;

            for (int i = 0; i < InputLevel; i++)
            {
                r.Start = Min;
                r.End =Min + res;
                r.Midpoint = (r.Start + r.End) / 2.000f;
                Min = r.End;
                Level.Add(r);
            }


            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {

                for (int j = 0; j < InputLevel; j++)    
                {
                    if (InputSignal.Samples[i] >= Level[j].Start && InputSignal.Samples[i] < Level[j].End)
                    {
                        OutputIntervalIndices.Add(j + 1);
                        OutputQuantizedSignal.Samples.Add(Level[j].Midpoint);
                        OutputSamplesError.Add(Level[j].Midpoint - InputSignal.Samples[i]);
                        OutputEncodedSignal.Add(Convert.ToString(j, 2).PadLeft(InputNumBits, '0'));
                        break;
                    }
                    if (InputSignal.Samples[i] == (float)Math.Round(Level[InputLevel - 1].End, 2))
                    {
                        OutputIntervalIndices.Add(InputLevel);
                        OutputQuantizedSignal.Samples.Add(Level[InputLevel - 1].Midpoint);
                        OutputSamplesError.Add(Level[InputLevel - 1].Midpoint - InputSignal.Samples[i]);
                        OutputEncodedSignal.Add(Convert.ToString(InputLevel - 1, 2).PadLeft(InputNumBits, '0'));
                        break;
                    }
                }
            }
        }
    }
}
