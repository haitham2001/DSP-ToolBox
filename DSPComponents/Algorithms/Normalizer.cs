using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            float max_val = InputSignal.Samples.Max();
            float min_val = InputSignal.Samples.Min();
            float range = max_val - min_val;
            OutputNormalizedSignal = new Signal(InputSignal.Samples, false);

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                    OutputNormalizedSignal.Samples[i] = (InputMaxRange - InputMinRange) * ((OutputNormalizedSignal.Samples[i] - min_val) / range) + InputMinRange;
            }
        }
    }
}
