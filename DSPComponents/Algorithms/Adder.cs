using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), false);
            int max = 0;
            int CountLessThanMax = 0;
            for(int i = 0;i<InputSignals.Count;i++)
            {
                if(max < InputSignals[i].Samples.Count)
                {
                    if (max != 0)
                        CountLessThanMax++;
                    max = InputSignals[i].Samples.Count;
                }
                else if (max > InputSignals[i].Samples.Count)
                    CountLessThanMax++;
                else
                    continue;
            }
            if (CountLessThanMax > 0)
            {
                for(int i = 0; i < InputSignals.Count;i++)
                {
                    if (InputSignals[i].Samples.Count < max)
                    {
                        int TheDifference = max - InputSignals[i].Samples.Count;
                        for(int j = 0; j < TheDifference; j++)
                        {
                            InputSignals[i].Samples.Add(0);
                        }
                    }
                }
            }
            for (int i = 0; i < InputSignals[0].Samples.Count; i++)
            {
                float SampleResult = 0;
                for(int j = 0; j < InputSignals.Count; j++)
                {
                    SampleResult += InputSignals[j].Samples[i];
                }
                OutputSignal.Samples.Add(SampleResult);
            }
            
        }
    }
}