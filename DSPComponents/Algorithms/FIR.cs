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
        string window_type;

        public override void Run()
        {
            //InputCutOffFrequency--> hayd5ol f halt el low w el high

            //step 1--->agib el attenution f hagib el window
            //2
            //3--> InputTransitionBand /fs ---> N
            //N lazm tb2a odd

            OutputHn = new Signal(new List<float>(), new List<int>(), false);
            OutputYn = new Signal(new List<float>(), new List<int>(), false);
            int N = getting_N(InputStopBandAttenuation);
            OutputHn = new Signal(new List<float>(), new List<int>(), false);
            float? element;
            for (int i = -(N - 1) / 2; i <= (N - 1) / 2; i++)
            {
                element = get_window_function(N, window_type, i) * get_filter_Type(N, InputFilterType, i);
                OutputHn.Samples.Add((float)element);
                OutputHn.SamplesIndices.Add(i);
            }
            Console.WriteLine(InputFilterType);
            Console.WriteLine(window_type);
            Console.WriteLine(InputStopBandAttenuation);
            Console.WriteLine(N);

            DirectConvolution conv = new DirectConvolution();
            conv.InputSignal1 = InputTimeDomainSignal;
            conv.InputSignal2 = OutputHn;
            conv.Run();
            OutputYn = conv.OutputConvolvedSignal;

            for (int i = 0; i < OutputHn.Samples.Count(); i++)
            {
                Console.WriteLine(OutputYn.SamplesIndices[i] + " " + OutputYn.Samples[i]);
            }


        }

        public int getting_N(float stopBandattenuation)
        {

            double N = 0;

            if (stopBandattenuation <= 21)
            {
                N = 0.9 * InputFS / InputTransitionBand;
                window_type = "rectangular";
            }
            else if (stopBandattenuation <= 44)
            {
                N = 3.1 * InputFS / InputTransitionBand;
                window_type = "hanning";
            }
            else if (stopBandattenuation <= 53)
            {
                N = 3.3 * InputFS / InputTransitionBand;
                window_type = "hamming";
            }
            else if (stopBandattenuation <= 74)
            {
                window_type = "blackman";
                N = 5.5 * InputFS / InputTransitionBand;
            }

            N = Math.Ceiling(N);
            if (N % 2 == 0)
            {
                return (int)(N + 1);
            }
            else
            {
                return (int)(N);
            }



        }

        public float get_window_function(int N, string window_name, int n)
        {
            if (window_name == "rectangular")
            {
                return (float)(1);
            }
            else if (window_name == "hanning")
            {
                float result = (float)(0.5 + 0.5 * Math.Cos((float)(2 * Math.PI * n) / N));
                return result;
            }
            else if (window_name == "hamming")
            {
                float result = (float)(0.54 + 0.46 * Math.Cos((float)(2 * Math.PI * n) / N));
                return result;
            }
            else
            {
                float result = (float)(0.42 + 0.5 * Math.Cos((float)(2 * Math.PI * n) / (N - 1)) + 0.08 * Math.Cos((float)(4 * Math.PI * n) / (N - 1)));
                return result;
            }
        }

        public float get_filter_Type(int N, FILTER_TYPES filter_type, int n)
        {
            float fc = 0;
            float f1 = 0;
            float f2 = 0;
            if (filter_type == FILTER_TYPES.BAND_PASS )
            {
                f1 = (float)((InputF1 - (InputTransitionBand / 2)) / InputFS);
                f2 = (float)((InputF2 + (InputTransitionBand / 2)) / InputFS);
            }
            else if(filter_type == FILTER_TYPES.BAND_STOP)
            {
                f1 = (float)((InputF1 + (InputTransitionBand / 2)) / InputFS);
                f2 = (float)((InputF2 - (InputTransitionBand / 2)) / InputFS);
            }
            else if (filter_type == FILTER_TYPES.LOW) 
                fc = ((float)InputCutOffFrequency + (InputTransitionBand / 2)) / InputFS;
            else if (filter_type == FILTER_TYPES.HIGH)
                fc = ((float)InputCutOffFrequency - (InputTransitionBand / 2)) / InputFS;
            //Lowpass
            if (filter_type == FILTER_TYPES.LOW)
            {
                if (n == 0)
                {
                    return 2 * fc;
                }
                else
                {
                    return (float)(2 * fc * (Math.Sin(n * 2 * Math.PI * fc)) / (n * 2 * Math.PI * fc));

                }
            }
            //High Pass
            else if (filter_type == FILTER_TYPES.HIGH)
            {
                if (n == 0)
                {
                    return (1 - 2 * fc);
                }
                else
                {
                    return ((float)(-2 * fc * (Math.Sin(n * 2 * Math.PI * fc) / (n * 2 * Math.PI * fc))));

                }
            }
            //Bandpass
            else if (filter_type == FILTER_TYPES.BAND_PASS)
            {
                if (n == 0)
                {
                    return 2 * (f2 - f1);
                }
                else
                {
                    return ((float)(2 * f2 * (Math.Sin(n * 2 * Math.PI * f2) / (n * 2 * Math.PI * f2)) - (2 * f1 * (Math.Sin(n * 2 * Math.PI * f1) / (n * 2 * Math.PI * f1)))));

                }
            }
            //Bandstop
            else
            {
                if (n == 0)
                {
                    return 1-(2 * (f2 - f1));
                }
                else
                {
                    return (float)((2 * f1 * (Math.Sin(n * 2 * Math.PI * f1) / (n * 2 * Math.PI * f1))) - (2 * f2 * (Math.Sin(n * 2 * Math.PI * f2) / (n * 2 * Math.PI * f2))));

                }
            }
        }
    }
}


