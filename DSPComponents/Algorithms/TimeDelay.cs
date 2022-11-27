using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            DirectCorrelation dc = new DirectCorrelation();
            dc.InputSignal1 = InputSignal1;
            dc.Run();
            float Max = dc.OutputNonNormalizedCorrelation.Max();

            List<float> multiplied = new List<float>();
            for (int j = 0; j < InputSignal1.Samples.Count; j++)
                multiplied.Add(InputSignal1.Samples[j] * InputSignal2.Samples[j]);

            int IndexOfMaxMult = dc.OutputNonNormalizedCorrelation.IndexOf(multiplied.Sum());
            int IndexOfMax = dc.OutputNonNormalizedCorrelation.IndexOf(Max);
            OutputTimeDelay = (IndexOfMax - IndexOfMaxMult) * InputSamplingPeriod;
        }
    }
}
