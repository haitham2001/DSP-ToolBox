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
            List<float> signal_samples = new List<float>();
            if(InputSignals[0].Samples.Count != InputSignals[1].Samples.Count)
            {
                int TheDifference = Math.Abs(InputSignals[0].Samples.Count - InputSignals[1].Samples.Count);
                int MaxNumber = Math.Max(InputSignals[0].Samples.Count , InputSignals[1].Samples.Count);
                if (InputSignals[0].Samples.Count > InputSignals[1].Samples.Count)
                {
                    for (int i = TheDifference; i < MaxNumber; i++)
                        InputSignals[1].Samples.Add(0);
                }
                else if(InputSignals[0].Samples.Count < InputSignals[1].Samples.Count)
                {
                    for (int i = TheDifference; i < MaxNumber; i++)
                        InputSignals[0].Samples.Add(0);
                }
            }
            for (int i = 0; i < InputSignals[1].Samples.Count; i++)
            {
                signal_samples.Add(InputSignals[0].Samples[i] + InputSignals[1].Samples[i]);
            }
            OutputSignal = new Signal(signal_samples, false);
        }
    }
}