using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            OutputConvolvedSignal = new Signal(new List<float>(), new List<int>(), false);
            int startIndex;
            int endIndex;
            List<float> tempValues = new List<float>();
            List<int> tempIndices = new List<int>();
            bool isZeroes = false;
            if (InputSignal1.SamplesIndices != null)
            {
                startIndex = InputSignal1.SamplesIndices.Min() + InputSignal2.SamplesIndices.Min();
                endIndex = InputSignal1.SamplesIndices.Max() + InputSignal2.SamplesIndices.Max();
            }
            else
            {
                startIndex = 0;
                endIndex = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                float yOfK = 0;
                for(int k = startIndex; k <= endIndex; k++)
                {
                    int xIndex;
                    int hIndex;
                    if(InputSignal1.SamplesIndices != null)
                    {
                        xIndex = InputSignal1.SamplesIndices.IndexOf(k);
                        hIndex = InputSignal2.SamplesIndices.IndexOf(i - k);
                    }
                    else 
                    {
                        xIndex = k;
                        hIndex = i - k;
                    }
                    if (xIndex == -1 || hIndex == -1)
                        continue;

                    yOfK += (InputSignal1.Samples[xIndex] * InputSignal2.Samples[hIndex]);
                }

                if (yOfK == 0)
                {
                    isZeroes = true;
                    tempValues.Add(yOfK);
                    tempIndices.Add(i);
                    if (i == endIndex)
                        break;
                }
                else
                {
                    if (isZeroes)
                    {
                        OutputConvolvedSignal.Samples.AddRange(tempValues);
                        OutputConvolvedSignal.SamplesIndices.AddRange(tempIndices);
                        tempValues.Clear();
                        tempIndices.Clear();
                        isZeroes = false;
                    }
                    OutputConvolvedSignal.Samples.Add(yOfK);
                    OutputConvolvedSignal.SamplesIndices.Add(i);
                }
            }
        }
    }
}
