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
using Twilio.AspNet.Common;
using Twilio.Clients;
using Twilio.Types;
using CallTrax.Mappers;
using Newtonsoft.Json;

namespace CallTrax.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallTraxController : TwilioController
    {
        // GET: api/<IVRController>
        [HttpGet]
        [Route("json/{callSid}")]
        public string Json(string callSid, string digits)
        {
            VoiceRequest request = new VoiceRequest
            {
                CallSid = callSid,
                Digits = digits,
                AccountSid = "MyAccount",
                From = "+13054575008",
                To = "+18001234567",
                CallStatus = "in-progress",
                ApiVersion = "v1",
                Direction = "inbound",
                CallerName = "Reinaldo Gutierrez"
            };
            return JsonConvert.SerializeObject(request);
        }
        [HttpPost]
        [Route("flow")]
        public TwiMLResult Inbound(VoiceRequest request)
        {
            var response = new VoiceResponse();

            try
            {
                //Save Call Information
                response = ProcessInboundCall(request);

            }
            catch (Exception)
            {

                throw;
            }
  
            return TwiML(response);
        }
        // GET: api/<IVRController>
        [HttpPost]
        [Route("callback/{stepId}")]
        public TwiMLResult Callback([FromRoute()] long stepId, [FromBody()] VoiceRequest request)
        {
            var response = new VoiceResponse();

            try
            {
                using (var context = new CallTraxContext())
                {
                    var step = context.CallFlowStepGather.Where(s => s.CallFlowStepId == stepId).FirstOrDefault();

                    if (step == null)
                    {
                        //TODO
                    }
                    else
                    {
                        string gatherValueName = "";

                        CallFlowStepOption  option = context.CallFlowStepOption.Where(o => o.CallFlowStepId == stepId && o.OptionValue == request.Digits).FirstOrDefault();

                        if (option == null)
                        {
                            //TODO
                        }
                        else
                        {
                            gatherValueName = option.OptionDescription;

                            //Saving customer choice
                            CallGather gather = new CallGather
                            {
                                CallSid = request.CallSid,
                                CallFlowStepId = step.CallFlowStepId,
                                GatherValue = request.Digits,
                            };
                            context.CallGather.Add(gather);
                            //Saving Action
                            context.CallAction.Add(new CallAction
                            {
                                CallSid = request.CallSid,
                                ActionDateTime = DateTime.Now,
                                ActionDescription = "Answer: " + request.Digits + " - " + gatherValueName
                            });
                            context.SaveChanges();

                            CallFlowStep nextStep = context.CallFlowStep.Where(s => s.CallFlowStepId == option.NextCallFlowStepId).FirstOrDefault();
                            if (nextStep == null)
                            {
                                //TODO
                            }
                            else
                            {
                                response = GetResponse(request.CallSid, nextStep);
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
        private VoiceResponse ProcessInboundCall(VoiceRequest request)
        {
            var response = new VoiceResponse();

            try
            {
                using (var context = new CallTraxContext())
                {
                    var tollfree = context.Tollfree.Where(t => t.TollfreeNumber == request.To).FirstOrDefault();

                    if (tollfree == null)
                    {
                        //No flow can be determine based on this 
                        //Log the error
                        response.Reject();
                    }
                    else
                    {
                        var welcomeStep = context.CallFlowStep.Where(s => s.CallFlowId == tollfree.CallFlowId && s.IsWelcomeStep).FirstOrDefault();

                        if (welcomeStep == null)
                        {
                            //TODO: Log the error
                            response.Reject();
                        }
                        else
                        {
                            //Saving New Call
                            SaveCall(request, welcomeStep.CallFlowId);

                            response = GetResponse(request.CallSid,welcomeStep);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }
        private VoiceResponse GetResponse(string CallSid, CallFlowStep step)
        {
            var response = new VoiceResponse();
            try
            {
                using (var context = new CallTraxContext())
                {
                    switch (step.CallFlowStepType.ToStepType())
                    {
                        case Enums.StepType.Gather:
                            var gather = GetGatherStep(step);
                            response.Append(gather);
                            //Save Call Action
                            CallFlowStepGather stepGather = context.CallFlowStepGather.Where(g => g.CallFlowStepId == step.CallFlowStepId).FirstOrDefault();
                            if(stepGather == null)
                            {
                                //TODO
                            }
                            else
                            {
                                context.CallAction.Add(new CallAction
                                {
                                    CallSid = CallSid,
                                    ActionDateTime = DateTime.Now,
                                    ActionDescription = "Question: " + stepGather.GatherName
                                });
                                context.SaveChanges();
                            }
                            
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

                                //Save ACtion
                                context.CallAction.Add(new CallAction
                                {
                                    CallSid = CallSid,
                                    ActionDateTime = DateTime.Now,
                                    ActionDescription = "Call transfered: " +  phone.DialPhoneNumber 
                                });
                                context.SaveChanges();
                            }

                            break;
                        case Enums.StepType.Hangup:
                            var hangupSay = context.CallFlowStepSay.Where(s => s.CallFlowStepId == step.CallFlowStepId).FirstOrDefault();
                            if (hangupSay != null)
                            {
                                response.Say(hangupSay.SayText);
                            }
                            response.Hangup();
                            //Save ACtion
                            context.CallAction.Add(new CallAction
                            {
                                CallSid = CallSid,
                                ActionDateTime = DateTime.Now,
                                ActionDescription = "Call hang up."
                            });
                            context.SaveChanges();

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
        private Gather GetGatherStep(CallFlowStep step)
        {
            try
            {
                string url1 = Url.ActionLink("callback", "calltrax").ToLower();
                int pos = url1.IndexOf("calltrax");
                string url = url1.Substring(0, pos);
                url += (url.IndexOf("api") == -1) ? "api/" : "";
                url += "calltrax/callback/" + step.CallFlowStepId.ToString();

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
        private void SaveCall(VoiceRequest request, long flowId)
        {
            try
            {
                using( var context = new CallTraxContext())
                {
                    Call call = context.Call.Where(c => c.CallSid == request.CallSid).FirstOrDefault();

                    if (call == null) //New Call
                    {
                        Call newcall = CallMapper.VoiceRequestToCall(request);
                        newcall.CallFlowId = flowId;
                        context.Add(newcall);
                    }
                    else
                    {
                        call.CallStatus = (short)request.CallStatus.ToCallStatus();
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
