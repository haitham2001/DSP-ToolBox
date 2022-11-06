using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            OutputTimeDomainSignal = new Signal(new List<float>(), false);
            List<Complex> Samples = new List<Complex>();
            int SizeOfSignals = InputFreqDomainSignal.FrequenciesAmplitudes.Count;
            for(int i = 0;i<SizeOfSignals;i++)
            {
                Samples.Add(Complex.FromPolarCoordinates(InputFreqDomainSignal.FrequenciesAmplitudes[i], InputFreqDomainSignal.FrequenciesPhaseShifts[i]));
            }
            for (int i = 0; i < SizeOfSignals; i++)
            {
                int n = 0;
                Complex sum = new Complex(0, 0);
                foreach (Complex Sample in Samples)
                {
                    double SingleSample = (2 * i * n * Math.PI) / SizeOfSignals;
                    sum += Sample * (Math.Cos(SingleSample) + (Complex.ImaginaryOne*Math.Sin(SingleSample)));
                    n++;
                }
                OutputTimeDomainSignal.Samples.Add((float)sum.Real/SizeOfSignals);
            }
        }
    }
}
