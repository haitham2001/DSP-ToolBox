using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), false);
            double N = InputSignal.Samples.Count;

            /*
             ---------------------------DCT---------------------------
             F(K) = a(k) * (sum (PI * (2n+1)*(k) / 2N ) from 0 to N-1)
             a(k) = sqrt(1 / N) if k =0
                    sqrt(2 / N) otherwise
            ----------------------------------------------------------
             */

            for (int k = 0; k < N; k++)
            {
                double sum_value = 0;
                for (int n = 0; n < N; n++)
                    sum_value += InputSignal.Samples[n] * Math.Cos(((Math.PI * ((2 * n) + 1) * k) / (2 * N)));

                if (k == 0)
                    sum_value *= Math.Sqrt(1 / N);
                else
                    sum_value *= Math.Sqrt(2 / N);

                OutputSignal.Samples.Add((float)sum_value);
            }
        }
    }
}
