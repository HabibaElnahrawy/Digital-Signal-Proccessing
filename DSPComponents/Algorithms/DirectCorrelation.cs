using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();



            //  Correlation : measuring of similarity (dependency) between two signals



            List<float> list_signal1 = new List<float>();
            List<float> list_signal2 = new List<float>();
            List<float> corr = new List<float>();
            List<float> normalizecorr = new List<float>();



            float sum1 = 0, sum2 = 0, norm_sum = 0;
            int N;



            if (InputSignal2 != null)
            {
                // cross correlation ...
                N = InputSignal1.Samples.Count;



                list_signal1 = InputSignal1.Samples;
                list_signal2 = InputSignal2.Samples;

            }
            else
            {
                //AUTO CORRELACTION ...
                N = InputSignal1.Samples.Count;



                list_signal1 = InputSignal1.Samples;
                list_signal2 = InputSignal1.Samples;
            }



            // Normalization cross correlation ...
            for (int i = 0; i < N; i++)
            {
                sum1 = sum1 + (float)Math.Pow(list_signal1[i], 2);
                sum2 = sum2 + (float)Math.Pow(list_signal2[i], 2);
            }
            norm_sum = (float)Math.Sqrt((sum1 * sum2)) / N;
            //...........

            if (InputSignal1.Periodic == false)
            {
                // Not Periodic
                for (int j = 0; j < N; j++)
                {
                    float x = 0;
                    for (int n = 0; n < N - j; n++)
                    {
                        // sum( x1(n)*x2(n+j) )/N ...
                        x = x + (list_signal1[n] * list_signal2[n + j]);
                    }
                    corr.Add(x / N);
                }
            }
            else
            {
                // periodic
                for (int i = 0; i < N; i++)
                {
                    float x = 0;
                    int inc = 0;
                    for (int j = 0; j < N; j++)
                    {
                        if (i + j >= N)
                        {
                            x = x + (list_signal1[j] * list_signal2[inc]);
                            // to increase shift ...
                            inc++;
                        }
                        else
                        {
                            // sum( x1(n)*x2(n+j) )/N ...
                            x = x + (list_signal1[j] * list_signal2[j + i]);
                        }
                    }
                    corr.Add(x / N);
                }
            }

            for (int i = 0; i < corr.Count; i++)
            {
                normalizecorr.Add(corr[i] / norm_sum);
            }

            OutputNonNormalizedCorrelation = corr;
            OutputNormalizedCorrelation = normalizecorr;

        }
    }
}