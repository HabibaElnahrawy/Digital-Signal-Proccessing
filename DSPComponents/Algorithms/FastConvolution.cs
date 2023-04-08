using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
         
            DiscreteFourierTransform fourierTransform = new DiscreteFourierTransform();
            InverseDiscreteFourierTransform InversefourierTransform = new InverseDiscreteFourierTransform();

            //if (InputSignal2 == null) InputSignal2 = InputSignal1;

            int N1 = InputSignal1.Samples.Count;
            int N2 = InputSignal2.Samples.Count;
            int desired_length = N1 + N2 - 1;
            for (int i = N1; i < desired_length; i++) InputSignal1.Samples.Add(0);
            for (int i = N2; i < desired_length; i++) InputSignal2.Samples.Add(0);

            fourierTransform.InputTimeDomainSignal = InputSignal1;
            fourierTransform.Run();
            List<float> x1_amp = new List<float>();
            List<float> x1_freq = new List<float>();
            x1_amp = fourierTransform.OutputFreqDomainSignal.FrequenciesAmplitudes;
            x1_freq = fourierTransform.OutputFreqDomainSignal.FrequenciesPhaseShifts;

            fourierTransform.InputTimeDomainSignal = InputSignal2;
            fourierTransform.Run();
            List<float> x2_amp = new List<float>();
            List<float> x2_freq = new List<float>();
            x2_amp = fourierTransform.OutputFreqDomainSignal.FrequenciesAmplitudes;
            x2_freq = fourierTransform.OutputFreqDomainSignal.FrequenciesPhaseShifts;

            List<float> out_amp = new List<float>();
            List<float> out_freq = new List<float>();
           
            for (int i = 0; i < x1_amp.Count; i++)
            {

               out_amp.Add(x1_amp[i] * x2_amp[i]);
               out_freq.Add(x1_freq[i] + x2_freq[i]);
            }
 
            Signal input_freq_domain = new Signal(false, null, out_amp, out_freq);
            InversefourierTransform.InputFreqDomainSignal = input_freq_domain;
            InversefourierTransform.Run();
            OutputConvolvedSignal = InversefourierTransform.OutputTimeDomainSignal;

  

        }
    }
}
