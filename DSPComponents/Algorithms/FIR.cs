using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public struct win_fun { public string name; public double N;}

        public override void Run()
        {
            List<float> fc = new List<float>();
            List<int> Indices = new List<int>();
            List<float> Multiply = new List<float>();

            fc = FC((InputCutOffFrequency == null) ? 0 : (float)InputCutOffFrequency, (InputF1 == null) ? 0 : (float)InputF1, (InputF2 == null) ? 0 : (float)InputF2 );

            win_fun w_f = get_window();

            for (int i = (-1 * ((int)w_f.N - 1) / 2); i <= ((int)w_f.N - 1) / 2; i++)
            {
                double h_d = Filters_hd(Math.Abs(i), fc[0], fc[0], (fc.Count == 1) ? 0 : fc[1]);
                double w = calculate_Window(w_f, Math.Abs(i));
                Multiply.Add((float)(h_d*w));
                Indices.Add(i);
            }


            OutputHn = new Signal(Multiply, false);
            OutputHn.SamplesIndices = Indices;

            DirectConvolution dc = new DirectConvolution();
            dc.InputSignal1 = InputTimeDomainSignal;
            dc.InputSignal2 = OutputHn;
            dc.Run();
            for (int i = 0; i < dc.OutputConvolvedSignal.Samples.Count; i++)
                dc.OutputConvolvedSignal.Samples[i] = dc.OutputConvolvedSignal.Samples[i];

            OutputYn = dc.OutputConvolvedSignal;
        }

        #region Filters(hd)
        // calculate hd(n)
        public double Filters_hd(int n ,float fc , float fc_1, float fc_2)
        {
            if (InputFilterType == FILTER_TYPES.LOW)
                return (float)((n == 0) ? (2 * fc) : term(n,fc));

            else if (InputFilterType == FILTER_TYPES.HIGH)
                return (float)((n == 0) ? (1 - (2 * fc)) : (-1 * term(n, fc)));

            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
                return (float)((n == 0) ? (2 * (fc_2 - fc_1)) : (term(n, fc_2) - term(n, fc_1)));

            else 
                return (float)((n == 0) ? (1 - (2 * (fc_2 - fc_1))) : (term(n, fc_1) - term(n, fc_2)));
        }

        public double term(int n, float fc)
        {
            return (double)((Math.Sin((double)n * 2 * Math.PI * fc)) / (Math.PI * n));
        }

        #endregion

        #region Update FC
        public List<float> FC(float fc, float fc_1, float fc_2)
        {
            List<float> Fc_values = new List<float>();

            if (InputFilterType == FILTER_TYPES.LOW)
                Fc_values.Add((fc + (InputTransitionBand / 2))/ InputFS);

            else if (InputFilterType == FILTER_TYPES.HIGH)
                Fc_values.Add((fc - (InputTransitionBand / 2)) / InputFS);

            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                Fc_values.Add((fc_1 - (InputTransitionBand / 2)) / InputFS);
                Fc_values.Add((fc_2 + (InputTransitionBand / 2)) / InputFS);
            }
            else 
            {
                Fc_values.Add((fc_1 + (InputTransitionBand / 2)) / InputFS);
                Fc_values.Add((fc_2 - (InputTransitionBand / 2)) / InputFS);
            }
            return Fc_values;
        }
        #endregion

        #region Name of window function
        public win_fun get_window()
        {
            InputTransitionBand = InputTransitionBand / InputFS;

            win_fun window = new win_fun();
            if (InputStopBandAttenuation <= 21)
            {
                window.name = "Rectangular";
                window.N = (int)Math.Ceiling(0.9 / InputTransitionBand);
                
            }
            else if (InputStopBandAttenuation <= 44)
            {
                window.name = "Hanning";
                window.N = (int)Math.Ceiling(3.1 / InputTransitionBand);
            }
            else if (InputStopBandAttenuation <= 53)
            {
                window.name = "Hamming";
                window.N = (int)Math.Ceiling(3.3 / InputTransitionBand);
            }
            else
            {
                window.name = "Blackman";
                window.N = (int)Math.Ceiling(5.5 / InputTransitionBand);
            }
            window.N = (window.N % 2 == 0) ? window.N + 1 : window.N;
            return window;
        }
        #endregion

        #region calculate window
        public double calculate_Window(win_fun wind, int n)
        {
            if (wind.name == "Rectangular")
                return 1.0;
            else if (wind.name == "Hanning")
                return (0.5 + 0.5 * (Math.Cos((2 * n * Math.PI) / wind.N)));
            else if (wind.name == "Hamming")
                return (0.54 + 0.46 * (Math.Cos((2 * n * Math.PI) / wind.N)));
            else
                return (0.42 + 0.5 * (Math.Cos((2 * n * Math.PI) / (wind.N - 1))) + 0.08 * (Math.Cos((4 * n * Math.PI) / (wind.N - 1))));
        }
        #endregion
    }
}
