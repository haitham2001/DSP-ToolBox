using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MultiplySignalByConstant : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputConstant { get; set; }
        public Signal OutputMultipliedSignal { get; set; }

        public override void Run()
        {
            OutputMultipliedSignal = new Signal(new List<float>(), new List<int>(), false);
            foreach (float sample in InputSignal.Samples)
            {
                OutputMultipliedSignal.Samples.Add(sample * InputConstant);
            }
        }
    }
}
