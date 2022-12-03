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

            int size1 = InputSignal2.Samples.Count - 1;
            int size2 = InputSignal1.Samples.Count - 1;

            for (int i = 0; i < size1; i++)
                InputSignal1.Samples.Add(0);

            for (int i = 0; i < size2; i++)
                InputSignal2.Samples.Add(0);

            DiscreteFourierTransform dft1 = new DiscreteFourierTransform();
            dft1.InputTimeDomainSignal = InputSignal1;
            dft1.Run();

            DiscreteFourierTransform dft2 = new DiscreteFourierTransform();
            dft2.InputTimeDomainSignal = InputSignal2;
            dft2.Run();

            for (int i = 0; i < dft1.complexes.Count; i++)
                samples.Add(Complex.Multiply(dft1.complexes[i], dft2.complexes[i]));

            InverseDiscreteFourierTransform Idft = new InverseDiscreteFourierTransform();
            Idft.InputFreqDomainSignal = new Signal(false, new List<float>(), new List<float>(), new List<float>());
            Idft.Samples = samples;
            Idft.Run();

            OutputConvolvedSignal.Samples = Idft.OutputTimeDomainSignal.Samples;

        }
    }
}
