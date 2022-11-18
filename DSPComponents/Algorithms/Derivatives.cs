using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            FirstDerivative = new Signal(new List<float>(), false);
            SecondDerivative = new Signal(new List<float>(), false);
            for(int i = 0; i < InputSignal.Samples.Count - 1; i++)
            {
                float yOfFirstDerivative = InputSignal.Samples[i];
                float yOfSecondDerivative = InputSignal.Samples[i + 1]  - 2 * InputSignal.Samples[i];

                if (i - 1 >= 0)
                {
                    yOfFirstDerivative -= InputSignal.Samples[i - 1];
                    yOfSecondDerivative += InputSignal.Samples[i - 1];
                }

                FirstDerivative.Samples.Add(yOfFirstDerivative);
                SecondDerivative.Samples.Add(yOfSecondDerivative);
            }
        }
    }
}
