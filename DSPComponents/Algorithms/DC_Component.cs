using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), false);
            float Mean = InputSignal.Samples.Average();

            for (int i = 0; i < InputSignal.Samples.Count(); i++)
                OutputSignal.Samples.Add(InputSignal.Samples[i] - Mean);
        }
    }
}
