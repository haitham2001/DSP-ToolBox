﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), new List<int>(), false);

            if (L > 0)
            {
                OutputSignal = up_samples(L, InputSignal);
                OutputSignal = initialize_fir(OutputSignal);

                if (M > 0)
                    OutputSignal = down_samples(M, OutputSignal);
            }
            else if (M > 0)
            {
                OutputSignal = initialize_fir(InputSignal);
                OutputSignal = down_samples(M, OutputSignal);
            }
            else
                Console.WriteLine("Can't do sampling with radio less than 1 !");
        }

        #region Up Sampling
        public Signal up_samples(int Up_count , Signal Input_signal)
        {
            int count = Input_signal.Samples.Count * Up_count;
            Signal Output_signal = new Signal(new float[count].ToList(), false);

            int index = 0;
            for (int i = 0; i < count ; i+=Up_count)
            {
                Output_signal.Samples[i] = Input_signal.Samples[index];
                OutputSignal.SamplesIndices.Add(index);
                index++;
            }
            return Output_signal; 
        }
        #endregion

        #region Down Sampling
        public Signal down_samples(int down_count , Signal Input_signal)
        {
            int count = (int)(Math.Ceiling(Input_signal.Samples.Count /(float)down_count));
            Signal Output_signal = new Signal(new List<float>(count), new List<int>(count), false);

            int index = 0;
            for (int i = 0; i < Input_signal.Samples.Count; i += down_count)
            {
                Output_signal.Samples.Add(Input_signal.Samples[i]);
                Output_signal.SamplesIndices.Add(index);
                index++;
            }

            return Output_signal;
        }
        #endregion

        #region initialize_fir
        public Signal initialize_fir(Signal input_signal)
        {
            FIR fir = new FIR();
            fir.InputTimeDomainSignal = input_signal;
            fir.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            fir.InputFS = 8000;
            fir.InputStopBandAttenuation = 50;
            fir.InputCutOffFrequency = 1500;
            fir.InputTransitionBand = 500;
            fir.Run();

            return fir.OutputYn;
        }
        #endregion
    }

}