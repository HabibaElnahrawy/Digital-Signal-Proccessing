using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            //x(k)=sum men n=0 to N-1 (x(nT)e^(-jk2pin/N))
            //x(n(nT))==> value beta3 el sample
            //N==> total; nb of samples
            //k==> rakam elli be7sbo dlwa2ty 
            OutputFreqDomainSignal = new Signal(new List<float>(), false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = new List<float>();
            OutputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>();

            
            // public Complex (double real, double imaginary);
            Complex j = new Complex(0, 1);
            Complex power = new Complex();

            //N : total nb of samples
            int N = InputTimeDomainSignal.Samples.Count();
            double arg = ((-2) * Math.PI )/ N;
            for (int k=0;k<N;k++)
            {
                Complex x = new Complex();
                for (int n=0;n<N;n++)
                {
                    double valueSample = InputTimeDomainSignal.Samples[n];
                    double RealPower = arg * k * n;
                    power = Complex.Multiply(RealPower, j);
                    //Math.Exp(power);
                    x +=Complex.Multiply(valueSample,Complex.Exp(power));
                }
                OutputFreqDomainSignal.FrequenciesAmplitudes.Add((float)x.Magnitude);
                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)x.Phase);
            }
            double w = (2 * Math.PI) / (N * (1 / InputSamplingFrequency));


        }
    }
}
