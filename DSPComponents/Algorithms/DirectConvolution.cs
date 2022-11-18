﻿using System;
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
            var total = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
            var startIndex = Math.Min(InputSignal1.SamplesIndices.Min(), InputSignal2.SamplesIndices.Min());
            for(int i = 0; i < total; i++)
            {
                for(int k = 0; k < startIndex; k++)
                {

                }
            }
        }
    }
}
