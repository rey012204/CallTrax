using CallTrax.ExtensionMethods;
using CallTrax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio.AspNet.Common;

namespace CallTrax.Mappers
{
    public class CallMapper
    {
        public static Call VoiceRequestToCall(VoiceRequest request)
        {
            try
            {
                return new Call
                {
                    CallSid = request.CallSid,
                    AccountSid = request.AccountSid,
                    FromPhoneNumber = request.From,
                    ToPhoneNumber = request.To,
                    CallStatus = (short)request.CallStatus.ToCallStatus(),
                    ApiVersion = request.ApiVersion,
                    Direction = (short)request.Direction.ToCallDirection(),
                    ForwardedFrom = request.ForwardedFrom,
                    CallerName = request.CallerName
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
