using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }
        public List<Complex> complexes { get; set; }
        public override void Run()
        {
            complexes = new List<Complex>();
            OutputFreqDomainSignal = new Signal(false, new List<float>(), new List<float>(), new List<float>());
            int SizeOfSignals = InputTimeDomainSignal.Samples.Count;
            for (int k = 0; k < SizeOfSignals; k++)
            {
                float Real = 0;
                float Imaginary = 0;
                int n = 0;
                foreach (float sample in InputTimeDomainSignal.Samples)
                {
                    double SingleSample = (2 * k * n * Math.PI) / SizeOfSignals;
                    Real += (float)(sample * Math.Cos(SingleSample));
                    Imaginary += (float)(sample * Math.Sin(SingleSample) * -1);
                    n++;
                }
                double Amplitude = Math.Sqrt((Real * Real) + (Imaginary * Imaginary));
                double PhaseShift = Math.Atan2(Imaginary , Real);
                complexes.Add(new Complex(Real, Imaginary));
                OutputFreqDomainSignal.Frequencies.Add(k);
                OutputFreqDomainSignal.FrequenciesAmplitudes.Add((float)Amplitude);
                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)PhaseShift);
            }
        }
    }
}
