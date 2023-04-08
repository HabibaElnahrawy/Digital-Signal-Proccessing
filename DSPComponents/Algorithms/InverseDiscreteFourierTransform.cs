using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            //x(k)=sum men n=0 to N-1 (x(nT)e^(-jk2pin/N))
            //x(n(nT))==> value beta3 el sample
            //N==> total; nb of samples
            //k==> rakam elli be7sbo dlwa2ty 
            OutputTimeDomainSignal = new Signal(new List<float>(), false);
            //OutputTimeDomainSignal.FrequenciesAmplitudes = new List<float>();
          //  OutputTimeDomainSignal.FrequenciesPhaseShifts = new List<float>();


            // public Complex (double real, double imaginary);
            Complex j = new Complex(0, 1);
            Complex power = new Complex();
            Complex valueSample = new Complex();

            //N : total nb of samples
            int N = InputFreqDomainSignal.FrequenciesAmplitudes.Count();
            double arg = ((2) * Math.PI) / N;
            for (int k = 0; k < N; k++)
            {
                Complex x = new Complex();
                for (int n = 0; n < N; n++)
                {
                    double RealPower = arg * k * n;
                    power = Complex.Multiply(RealPower, j);
                    //Math.Exp(power);
                    // double valueSample = Complex.FromPolarCoordinates(x.magnetude, x.phase);
                   double Magnitude = InputFreqDomainSignal.FrequenciesAmplitudes[n];
                    double Phase = InputFreqDomainSignal.FrequenciesPhaseShifts[n];
                    valueSample = Complex.FromPolarCoordinates(Magnitude,Phase);
                    x += Complex.Multiply(valueSample, Complex.Exp(power));
                }
                float sample =(float)(x.Real);
                sample /= N;
                OutputTimeDomainSignal.Samples.Add((float)sample);
            }
        }
    }
}
