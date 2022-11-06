using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public struct Complex1
    {
        public double RealNum;
        public double ImaginaryNum;
        public Complex1(double RealNum, double ImaginaryNum)
        {
            this.RealNum = RealNum;
            this.ImaginaryNum = ImaginaryNum;  
        }
    }
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            OutputFreqDomainSignal = new Signal(false, new List<float>(), new List<float>(), new List<float>());
            int SizeOfSignals = InputTimeDomainSignal.Samples.Count;
            for (int k = 0; k < SizeOfSignals; k++)
            {
                Complex1 SingleFreq = new Complex1(0,0);
                int n = 0;
                foreach (float sample in InputTimeDomainSignal.Samples)
                {
                    double SingleSample = (2 * k * n * Math.PI) / SizeOfSignals;
                    SingleFreq.RealNum += sample * Math.Cos(SingleSample);
                    SingleFreq.ImaginaryNum += sample * Math.Sin(SingleSample) * -1;
                    n++;
                }
                double Amplitude = Math.Sqrt((SingleFreq.RealNum * SingleFreq.RealNum) + (SingleFreq.ImaginaryNum * SingleFreq.ImaginaryNum));
                double PhaseShift = Math.Atan2(SingleFreq.ImaginaryNum , SingleFreq.RealNum);
                OutputFreqDomainSignal.Frequencies.Add(k);
                OutputFreqDomainSignal.FrequenciesAmplitudes.Add((float)Amplitude);
                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)PhaseShift);
            }
        }
    }
}
