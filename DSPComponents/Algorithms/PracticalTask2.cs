﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Net;
using System.Data.Common;

namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public string folder_path = @"C:\Users\Hani\Desktop\Github\DSP\Signals\";
        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);
            Signal Output;
            //Display the signal

            FIR fir = new FIR();
            fir.InputTimeDomainSignal = InputSignal;
            fir.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_PASS;
            fir.InputStopBandAttenuation = 50;
            fir.InputTransitionBand = 500;
            fir.InputF1 = miniF;
            fir.InputF2 = maxF;
            fir.InputFS = Fs;
            fir.Run();

            // save signal
            saveSignal("Fir Signal.ds", fir.OutputYn);

            Sampling sample = new Sampling();
            if (Fs >= maxF * 2)
            {
                sample.InputSignal = fir.OutputYn;
                sample.L = L;
                sample.M = M;
                sample.Run();
                Fs = newFs;
                Output = sample.OutputSignal;

                //save the signal
                saveSignal("Sampling Signal.ds", sample.OutputSignal);
            }
            else
            {
                Output = fir.OutputYn;
                Console.WriteLine("Fs is < maxmuim frequency");
            }
            
            DC_Component dc = new DC_Component();
            dc.InputSignal = Output;
            dc.Run();

            //save the signal
            saveSignal("Signal after remove dc.ds", dc.OutputSignal);

            Normalizer normalizer = new Normalizer();
            normalizer.InputSignal = dc.OutputSignal;
            normalizer.InputMinRange = -1;
            normalizer.InputMaxRange = 1;
            normalizer.Run();

            //save the signal
            saveSignal("Signal after normalize.ds", normalizer.OutputNormalizedSignal);

            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            dft.InputTimeDomainSignal = normalizer.OutputNormalizedSignal;
            dft.InputSamplingFrequency = Fs;
            dft.Run();
            for (int i = 0; i < dft.OutputFreqDomainSignal.Frequencies.Count; i++)
                dft.OutputFreqDomainSignal.Frequencies[i] = (float)Math.Round(dft.OutputFreqDomainSignal.Frequencies[i], 1);

            OutputFreqDomainSignal = dft.OutputFreqDomainSignal;

            //save the signal
            saveSignal("FreqDomainSignal.ds", OutputFreqDomainSignal);

        }

        public void saveSignal(string fileName, Signal signal)
        {
            // Save File to .txt  
            FileStream fileStream = new FileStream(folder_path + fileName, FileMode.Create, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            if (fileName == "FreqDomainSignal.ds")
            {
                streamWriter.WriteLine(1);
                streamWriter.WriteLine(0);
                streamWriter.WriteLine(signal.Frequencies.Count);
                for (int i = 0; i < signal.Frequencies.Count; i++)
                    streamWriter.WriteLine(signal.Frequencies[i] + " " + signal.FrequenciesAmplitudes[i] + " " + signal.FrequenciesPhaseShifts[i]);
            }
            else
            {
                streamWriter.WriteLine(0);
                streamWriter.WriteLine(0);
                streamWriter.WriteLine(signal.Samples.Count);
                for (int i = 0; i < signal.Samples.Count; i++)
                    streamWriter.WriteLine(signal.SamplesIndices[i] + " " + signal.Samples[i]);
            }
            streamWriter.Flush();
            streamWriter.Close();
        }
        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
