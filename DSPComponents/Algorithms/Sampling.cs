using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
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

            List<float> Samples = new List<float>();
            OutputSignal = new Signal(new List<float>(), false);


            FIR fir_obj = new FIR();
            fir_obj.InputStopBandAttenuation = 50;
            fir_obj.InputTransitionBand = 500;
            fir_obj.InputFS = 8000;
            fir_obj.InputCutOffFrequency = 1500;
            fir_obj.InputFilterType = FILTER_TYPES.LOW;

            int size = InputSignal.Samples.Count;
            //up sample by L factor and then apply low pass filter.
            if (M == 0 && L != 0)   
            {
                for (int i = 0; i < size; i++)
                {
                    Samples.Add(InputSignal.Samples[i]);
                    if (i != InputSignal.Samples.Count - 1)
                    {
                        for (int j = 0; j < L - 1; j++)
                        {
                            Samples.Add(0);
                        }
                    }

                }
                fir_obj.InputTimeDomainSignal = new Signal(Samples, false);
                fir_obj.Run();
                OutputSignal = fir_obj.OutputYn;
            }

            //this means we want to change sample rate by fraction.
            //Thus, first up sample by L factor, apply low pass filter and then down sample by M factor.
            else if (M != 0 && L != 0)
            {
                int count = InputSignal.Samples.Count;
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    Samples.Add(InputSignal.Samples[i]);
                    if (i != count - 1)
                    {
                        for (int j = 0; j < L - 1; j++)
                        {
                            Samples.Add(0);
                        }
                    }

                }

                fir_obj.InputTimeDomainSignal = new Signal(Samples, false);
                fir_obj.Run();

                double size1 = fir_obj.OutputYn.Samples.Count;
                for (int i = 0; i < size1; i += M)
                {
                    OutputSignal.Samples.Add(fir_obj.OutputYn.Samples[i]);
                }
            }
            //then apply filter first and thereafter down sample by M factor.
            else if (M != 0 && L == 0)  
            {
                fir_obj.InputTimeDomainSignal = InputSignal;
                fir_obj.Run();
                OutputSignal = new Signal(new List<float>(), false);
                for (int i = 0; i < fir_obj.OutputYn.Samples.Count; i += M)
                {
                    OutputSignal.Samples.Add(fir_obj.OutputYn.Samples[i]);
                }
            }
            
            else
            {
                throw new Exception("error message");
            }

        }
    }

}