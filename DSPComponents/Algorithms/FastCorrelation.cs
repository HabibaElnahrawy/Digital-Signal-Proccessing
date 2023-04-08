using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            DiscreteFourierTransform fourierTransform = new DiscreteFourierTransform();
            InverseDiscreteFourierTransform InversefourierTransform = new InverseDiscreteFourierTransform();

            if (InputSignal2 == null) InputSignal2 = InputSignal1;

     
            double sum_1 = 0;
            double sum_2 = 0;
            for(int i=0;i<InputSignal1.Samples.Count();i++)
            {
                sum_1 += InputSignal1.Samples[i]* InputSignal1.Samples[i];
                sum_2 += InputSignal2.Samples[i] * InputSignal2.Samples[i];
            }
            double normalization_factor = Math.Sqrt(sum_1 * sum_2) / InputSignal1.Samples.Count;

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
            for(int i = 0; i < x1_amp.Count; i++)
            {
                out_amp.Add(x1_amp[i] * x2_amp[i]);
                out_freq.Add(-x1_freq[i] + x2_freq[i]);
            }

            Signal input_freq_domain = new Signal(false, null, out_amp, out_freq);
            InversefourierTransform.InputFreqDomainSignal = input_freq_domain;
            InversefourierTransform.Run();
            OutputNonNormalizedCorrelation = InversefourierTransform.OutputTimeDomainSignal.Samples;
            OutputNormalizedCorrelation = new List<float>(OutputNonNormalizedCorrelation);
            for (int i =0; i< OutputNonNormalizedCorrelation.Count; i++)
            {
                OutputNonNormalizedCorrelation[i] /= InputSignal1.Samples.Count;
                OutputNormalizedCorrelation[i] = OutputNormalizedCorrelation[i] / (float)normalization_factor / InputSignal1.Samples.Count;
        }








        }
    }
}