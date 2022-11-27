using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public List<float> correlation(Signal FirstInput, Signal SecondInput)
        {
            int i = 0;
            List<float> Output = new List<float>();
            do
            {
                float head = 0;
                List<float> multiplied = new List<float>();
                for (int j = 0; j < FirstInput.Samples.Count; j++)
                    multiplied.Add(FirstInput.Samples[j] * SecondInput.Samples[j]);

                if (FirstInput.Periodic == true)
                {
                    head = SecondInput.Samples[0];
                    SecondInput.Samples.Add(head);
                    SecondInput.Samples.RemoveAt(0);
                }
                else
                {
                    SecondInput.Samples.Add(head);
                    SecondInput.Samples.RemoveAt(0);
                }
                Output.Add(multiplied.Sum() / FirstInput.Samples.Count);
                i++;
            } while (i < FirstInput.Samples.Count);
            return Output;
        }
        public override void Run()
        {
            OutputNormalizedCorrelation = new List<float>();
            float squareRoot = 0;
            if (InputSignal2 == null)
            {
                Signal newSignal = new Signal(new List<float>(InputSignal1.Samples), InputSignal1.Periodic);
                OutputNonNormalizedCorrelation = correlation(InputSignal1, newSignal);
                for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    squareRoot += InputSignal1.Samples[i] * InputSignal1.Samples[i];
                squareRoot /= InputSignal1.Samples.Count;
            }
            else
            {
                OutputNonNormalizedCorrelation = correlation(InputSignal1, InputSignal2);
                List<float> r1 = correlation(InputSignal1, InputSignal1);
                List<float> r2 = correlation(InputSignal2, InputSignal2);
                squareRoot = (float)Math.Sqrt(r1[0] * r2[0]);
            }
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
                OutputNormalizedCorrelation.Add((OutputNonNormalizedCorrelation[i] / squareRoot));
        }
        
    }
}