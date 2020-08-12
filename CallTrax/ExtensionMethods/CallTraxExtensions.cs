using CallTrax.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.Rest.Trunking.V1;

namespace CallTrax.ExtensionMethods
{
    public static class CallTraxExtensions
    {
        public static StepType  ToStepType(this short i)
        {
            return (StepType)i;
        }
        public static CallDirection ToCallDirection(this string s)
        {
            switch (s.ToLower())
            {
                case "inbound":
                    return CallDirection.Inbound;
                case "outbound-api":
                    return CallDirection.OutboundApi;
                case "outbound-dial":
                    return CallDirection.OutboundDial;
                default:
                    return CallDirection.Inbound;
            }
        }
        public static CallStatus ToCallStatus(this string s)
        {
            switch(s.ToLower())
            {
                case "queued":
                    return CallStatus.Queued;
                case "ringing":
                    return CallStatus.Ringing;
                case "in-progress":
                    return CallStatus.InProgress;
                case "completed":
                    return CallStatus.Completed;
                case "busy":
                    return CallStatus.Busy;
                case "failed":
                    return CallStatus.Failed;
                case "no-answer":
                    return CallStatus.NoAnswer;
                case "canceled":
                    return CallStatus.Cancelled;
                default:
                    return CallStatus.InProgress;
            }
        }
    }
}
