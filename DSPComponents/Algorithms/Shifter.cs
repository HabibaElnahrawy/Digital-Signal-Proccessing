﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            List<int> list_indix = new List<int>();

            if (! InputSignal.Periodic)
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    list_indix.Add(InputSignal.SamplesIndices[i] + (ShiftingValue * -1));

                }
                

            }
            else
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    list_indix.Add(InputSignal.SamplesIndices[i] + ShiftingValue);
                }
            }
            OutputShiftedSignal = new Signal(InputSignal.Samples , list_indix, false);

        }
    }
    }

