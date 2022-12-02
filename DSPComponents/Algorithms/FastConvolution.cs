using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<Complex> samples  = new List<Complex>();
            OutputConvolvedSignal = new Signal(new List<float>(), false);
            List<Complex> dft_1 = new List<Complex>();
            List<Complex> dft_2 = new List<Complex>();

            int count = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
            int size1 = count - InputSignal1.Samples.Count;
            int size2 = count - InputSignal2.Samples.Count;

            for (int i = 0; i < size1; i++)
                InputSignal1.Samples.Add(0);

            for (int i = 0; i < size2; i++)
                InputSignal2.Samples.Add(0);

            DiscreteFourierTransform dft1 = new DiscreteFourierTransform();
            dft1.InputTimeDomainSignal = InputSignal1;
            dft1.Run();

            for (int i = 0; i < dft1.OutputFreqDomainSignal.Frequencies.Count; i++)
                dft_1.Add(Complex.FromPolarCoordinates(dft1.OutputFreqDomainSignal.FrequenciesAmplitudes[i],
                    dft1.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));


            DiscreteFourierTransform dft2 = new DiscreteFourierTransform();
            dft2.InputTimeDomainSignal = InputSignal2;
            dft2.Run();

            for (int i = 0; i < dft2.OutputFreqDomainSignal.Frequencies.Count; i++)
                dft_2.Add(Complex.FromPolarCoordinates(dft2.OutputFreqDomainSignal.FrequenciesAmplitudes[i],
                    dft2.OutputFreqDomainSignal.FrequenciesPhaseShifts[i]));

            for (int i = 0; i < dft_1.Count; i++)
                samples.Add(Complex.Multiply(dft_1[i], dft_2[i]));

            InverseDiscreteFourierTransform Idft = new InverseDiscreteFourierTransform();
            Idft.InputFreqDomainSignal = new Signal(false, new List<float>(), new List<float>(), new List<float>());
            for (int i = 0; i < samples.Count; i++)
            {
                double Amplitude = Math.Sqrt((samples[i].Real * samples[i].Real) + (samples[i].Imaginary * samples[i].Imaginary));
                double PhaseShift = Math.Atan2(samples[i].Imaginary, samples[i].Real);
                Idft.InputFreqDomainSignal.FrequenciesAmplitudes.Add((float)Amplitude);
                Idft.InputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)PhaseShift);

            }
            Idft.Run();

            OutputConvolvedSignal.Samples = Idft.OutputTimeDomainSignal.Samples;

        }
    }
}
