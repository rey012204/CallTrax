using CallTrax.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CallTrax.ExtensionMethods
{
    public static class IntExtensions
    {
        public static StepType  ToStepType(this short i)
        {
            return (StepType)i;
        }
    }
}
