using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            try
            {
                if (InputSignal.FrequenciesAmplitudes.Count > 0)
                    InputSignal.FrequenciesAmplitudes.Add(2);
                else
                    InputSignal.FrequenciesAmplitudes.Add(2);
            }
            catch
            {
                InputSignal.FrequenciesAmplitudes = new List<float>() { 1 };
            }


            List<int> SamplesIndices = InputSignal.SamplesIndices;
            try
            {
                if (InputSignal.Frequencies.Count != 0)
                {
                    SamplesIndices = InputSignal.SamplesIndices.Select(x => x + (2 * (int)InputSignal.Frequencies[0])).ToList();
                }
            }
            catch { }
            finally
            {
                if (InputSignal.SamplesIndices.Count % 2 == 0)
                    SamplesIndices = Enumerable.Reverse(InputSignal.SamplesIndices.Select(x => x * -1).ToList()).ToList();

                OutputFoldedSignal = new Signal(Enumerable.Reverse(InputSignal.Samples).ToList()
               , SamplesIndices, true, new List<float>(), InputSignal.FrequenciesAmplitudes, new List<float>());
            }
            //if (InputSignal.SamplesIndices.Count % 2 == 0)
            //{
            //    SamplesIndices = Enumerable.Reverse(InputSignal.SamplesIndices.Select(x => x * -1).ToList()).ToList();
            //}
            //OutputFoldedSignal = new Signal(Enumerable.Reverse(InputSignal.Samples).ToList()
            //, SamplesIndices, true);
        }
    }
}
