using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            List<Complex> samples = new List<Complex>();
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            DiscreteFourierTransform dft1 = new DiscreteFourierTransform();
            DiscreteFourierTransform dft2 = new DiscreteFourierTransform();
            dft1.InputTimeDomainSignal = InputSignal1;
            dft1.Run();

            if (InputSignal2 != null)
            {
                if (InputSignal2.Samples.Count != InputSignal1.Samples.Count)
                {
                    int count = Math.Abs(InputSignal2.Samples.Count - InputSignal1.Samples.Count);
                    for (int i = 0; i < count; i++)
                        InputSignal2.Samples.Add(0);
                }
                dft2.InputTimeDomainSignal = InputSignal2;
                dft2.Run();
            }

            for (int i = 0; i < dft1.complexes.Count; i++)
                samples.Add(Complex.Multiply(Complex.Conjugate(dft1.complexes[i]), (InputSignal2 != null ? dft2.complexes[i] : dft1.complexes[i])));

            InverseDiscreteFourierTransform Idft = new InverseDiscreteFourierTransform();
            Idft.InputFreqDomainSignal = new Signal(false, new List<float>(), new List<float>(), new List<float>());
            Idft.Samples = samples;
            Idft.Run();

            float squareRoot = 0;

            if (InputSignal2 == null)
            {
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    squareRoot += InputSignal1.Samples[i] * InputSignal1.Samples[i];
                squareRoot /= InputSignal1.Samples.Count;
            }
            else
            {
                float sum_1 = 0;
                float sum_2 = 0;
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                {
                    sum_1 += (float)(Math.Pow(InputSignal1.Samples[i], 2));
                    sum_2 += (float) (Math.Pow(InputSignal2.Samples[i], 2));
                }
                squareRoot = sum_1 * sum_2;
                squareRoot = (float)(Math.Sqrt(squareRoot) / (float)InputSignal1.Samples.Count);
            }

            for (int i = 0; i < samples.Count; i++)
            {
                OutputNonNormalizedCorrelation.Add(Idft.OutputTimeDomainSignal.Samples[i]/samples.Count);
                OutputNormalizedCorrelation.Add(OutputNonNormalizedCorrelation[i] / squareRoot);
            }

        }
    }
}