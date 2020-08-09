using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using CallTrax.ExtensionMethods;
using Twilio.AspNet.Core;
using CallTrax.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CallTrax.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallTraxController : TwilioController
    {
        // GET: api/<IVRController>
        [HttpGet]
        [Route("flow/{id}")]
        public TwiMLResult Flow(long id)
        {
            var response = new VoiceResponse();

            try
            {
                using (var context = new CallTraxContext())
                {
                    var welcomeStep = context.CallFlowStep.Where(s => s.CallFlowId == id && s.IsWelcomeStep).FirstOrDefault();

                    if (welcomeStep == null)
                    {
                        //TODO
                    }
                    else
                    {
                        //Saving New Call
                        var call = new Call()
                        {
                            DateTimeReceived = DateTime.Now,
                            CallFlowId = welcomeStep.CallFlowId
                        };
                        context.Call.Add(call);
                        context.SaveChanges();
                        long callId = call.CallId;

                        response = GetResponse(callId,welcomeStep);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return TwiML(response);
        }
        // GET: api/<IVRController>
        [HttpGet]
        [Route("step/{callId}/{stepId}/{digits}")]
        public TwiMLResult Step(long callId, long stepId, string digits)
        {
            var response = new VoiceResponse();

            try
            {
                using (
                    var context = new CallTraxContext())
                {
                    var step = context.CallFlowStepGather.Where(s => s.CallFlowStepId == stepId).FirstOrDefault();

                    if (step == null)
                    {
                        //TODO
                    }
                    else
                    {
                        //Saving customer choise
                        CallGather gather = new CallGather
                        {
                            CallId = callId,
                            GatherName = step.GatherName,
                            GatherValue = digits
                        };
                        context.CallGather.Add(gather);
                        context.SaveChanges();

                        CallFlowStepOption  option = context.CallFlowStepOption.Where(o => o.CallFlowStepId == stepId && o.OptionValue == digits).FirstOrDefault();

                        if (option == null)
                        {
                            //TODO
                        }
                        else
                        {
                            CallFlowStep nextStep = context.CallFlowStep.Where(s => s.CallFlowStepId == option.NextCallFlowStepId).FirstOrDefault();
                            if (nextStep == null)
                            {

                            }
                            else
                            {
                                response = GetResponse(callId, nextStep);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return TwiML(response);
        }
        private VoiceResponse GetResponse(long callId, CallFlowStep step)
        {
            var response = new VoiceResponse();
            try
            {
                using (var context = new CallTraxContext())
                {
                    switch (step.CallFlowStepType.ToStepType())
                    {
                        case Enums.StepType.Gather:
                            var gather = GetGatherStep(callId, step);
                            response.Append(gather);
                            break;
                        case Enums.StepType.Dial:
                            var dialSay = context.CallFlowStepSay.Where(s => s.CallFlowStepId == step.CallFlowStepId).FirstOrDefault();
                            if (dialSay != null)
                            {
                                response.Say(dialSay.SayText);
                            }
                            CallFlowStepDial phone = context.CallFlowStepDial.Where(p => p.CallFlowStepId == step.CallFlowStepId).FirstOrDefault();
                            if(phone == null)
                            {
                                //TODO
                            }
                            else
                            {
                                response.Dial(phone.DialPhoneNumber);
                            }
                            break;
                        case Enums.StepType.Hangup:
                            var hangupSay = context.CallFlowStepSay.Where(s => s.CallFlowStepId == step.CallFlowStepId).FirstOrDefault();
                            if (hangupSay != null)
                            {
                                response.Say(hangupSay.SayText);
                            }
                            response.Hangup();
                            break;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
        private Gather GetGatherStep(long callId, CallFlowStep step)
        {
            try
            {
                string url1 = Url.ActionLink("step", "calltrax").ToLower();
                int pos = url1.IndexOf("calltrax");
                string url = url1.Substring(0, pos);
                url += (url.IndexOf("api") == -1) ? "api/" : "";
                url += "calltrax/step/" + callId + "/" + step.CallFlowStepId.ToString();

                using (var context = new CallTraxContext())
                {
                    string say = "";
                    var stepSay = context.CallFlowStepSay.Where(s => s.CallFlowStepId == step.CallFlowStepId).FirstOrDefault();
                    if (stepSay != null)
                    {
                        say = stepSay.SayText;
                    }

                    var gather = new Gather(action: new Uri(url), numDigits: 1);
                    if (!string.IsNullOrEmpty(say))
                    {
                        gather.Say(say);
                    }
                    return gather;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
