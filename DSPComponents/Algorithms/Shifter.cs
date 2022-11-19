using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            if (InputSignal.Periodic == true && InputSignal.FrequenciesAmplitudes.Count == 1)
                ShiftingValue *= -1;
            //else if (InputSignal.FrequenciesAmplitudes.Count == 0)

            //OutputShiftedSignal = new Signal(InputSignal.Samples, InputSignal.SamplesIndices.Select(x => x - ShiftingValue).ToList(), InputSignal.Periodic);

            List<float> shifts = new List<float>() { (float)ShiftingValue };

            OutputShiftedSignal = new Signal(InputSignal.Samples, InputSignal.SamplesIndices.Select(x => x - ShiftingValue).ToList(),
                false, shifts, InputSignal.FrequenciesAmplitudes, new List<float>());
        }
    }
}