﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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

        public override void Run()
        {
            Signal input_signal = LoadSignal(SignalPath);

            ///////////////////////////////////////////////
            FIR FIR = new FIR();
            FIR.InputStopBandAttenuation = 50;
            FIR.InputTransitionBand = 500;
            FIR.InputFS = Fs;
            FIR.InputF1 = miniF;
            FIR.InputF2 = maxF;
            FIR.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_PASS;
            FIR.InputTimeDomainSignal = input_signal;

            FIR.Run();
            //////////////
            input_signal = FIR.OutputYn;

            //string full_path = @"E:\ds files\fir.ds";
            //using (StreamWriter writer = new StreamWriter(full_path)) {
            //    writer.WriteLine(1);
            //    writer.WriteLine(0);//periodic
            //    writer.WriteLine(input_signal.Frequencies.Count);
            //    for(int i=0;i< input_signal.Frequencies.Count;i++)
            //    {
            //        writer.Write(input_signal.Frequencies[i]);
            //        writer.Write(" ");
            //        writer.Write(input_signal.FrequenciesAmplitudes[i]);
            //        writer.Write(" ");
            //        writer.Write(input_signal.FrequenciesPhaseShifts[i]);
            //    }


            //}

            //fs>2*fmax
             ///////////////////////////////sampling///////////////////
            if (newFs >= 2 * maxF)
            {
                Sampling sample_value = new Sampling();
                sample_value.InputSignal = input_signal;
                sample_value.M = M;
                sample_value.L = L;
                sample_value.Run();
                input_signal = sample_value.OutputSignal;
            }
            ////////////////////write///////////////////////////
            ///

            //string full_path1 = @"E:\ds files\sampling.ds";
            //using (StreamWriter writer = new StreamWriter(full_path))
            //{
            //    writer.WriteLine(0);
            //    writer.WriteLine(0);//periodic
            //    writer.WriteLine(input_signal.Samples.Count);
            //    for (int i = 0; i < input_signal.Samples.Count; i++)
            //    {
            //        writer.Write(input_signal.Samples[i]);
            //        writer.Write(" ");
            //        writer.Write(input_signal.SamplesIndices[i]);
                   
            //    }


            //}

            /////////////////dc///////////////
            DC_Component dc_component = new DC_Component();
            dc_component.InputSignal = input_signal;
            dc_component.Run();
            input_signal = dc_component.OutputSignal;

            //string full_path2 = @"E:\ds files\dc.ds";
            //using (StreamWriter writer = new StreamWriter(full_path))
            //{
            //    writer.WriteLine(0);
            //    writer.WriteLine(0);//periodic
            //    writer.WriteLine(input_signal.Samples.Count);
            //    for (int i = 0; i < input_signal.Samples.Count; i++)
            //    {
            //        writer.Write(input_signal.Samples[i]);
            //        writer.Write(" ");
            //        writer.Write(input_signal.SamplesIndices[i]);

            //    }


            //}
            /////////////normalizer/////////////
            Normalizer normalizer = new Normalizer();
            normalizer.InputSignal = input_signal;
            normalizer.InputMinRange = -1;
            normalizer.InputMaxRange = 1;
            normalizer.Run();
            input_signal = normalizer.OutputNormalizedSignal;

            //string full_path3 = @"E:\ds files\norm.ds";
            //using (StreamWriter writer = new StreamWriter(full_path))
            //{
            //    writer.WriteLine(1);
            //    writer.WriteLine(0);//periodic
            //    writer.WriteLine(input_signal.Samples.Count);
            //    for (int i = 0; i < input_signal.Samples.Count; i++)
            //    {
            //        writer.Write(input_signal.Samples[i]);
            //        writer.Write(" ");
            //        writer.Write(input_signal.SamplesIndices[i]);

            //    }


            //}

            //////////////DFT///////////////
            DiscreteFourierTransform DFT = new DiscreteFourierTransform();
            DFT.InputSamplingFrequency = Fs;
            DFT.InputTimeDomainSignal = input_signal;



            DFT.Run();
            OutputFreqDomainSignal = DFT.OutputFreqDomainSignal;

            //string full_path4 = @"E:\ds files\dft.ds";
            //using (StreamWriter writer = new StreamWriter(full_path))
            //{
            //    writer.WriteLine(1);
            //    writer.WriteLine(0);//periodic
            //    writer.WriteLine(input_signal.Frequencies.Count);
            //    for (int i = 0; i < input_signal.Frequencies.Count; i++)
            //    {
            //        writer.Write(input_signal.Frequencies[i]);
            //        writer.Write(" ");
            //        writer.Write(input_signal.FrequenciesAmplitudes[i]);
            //        writer.Write(" ");
            //        writer.Write(input_signal.FrequenciesPhaseShifts[i]);
            //    }


            //}
            // throw new NotImplementedException();
            ///////////////////////////////////////////////////
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
