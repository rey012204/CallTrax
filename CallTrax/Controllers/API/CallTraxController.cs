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
using Microsoft.Extensions.Configuration;

namespace CallTrax.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallTraxController : TwilioController
    {
        IConfiguration configuration;

        public CallTraxController(IConfiguration config)
        {
            configuration = config;
        }

        // GET: api/<IVRController>
        [HttpGet]
        [Route("json/{callSid}/{digits}")]
        public string Json(string callSid, string digits)
        {
            try
            {
                string toPhone = "";
                using (var context = new CallTraxContext())
                {
                    Tollfree tollfree = context.Tollfree.Where(t => t.CallFlowId == 1).FirstOrDefault();
                    toPhone = tollfree.TollfreeNumber.Trim();
                }

                VoiceRequest request = new VoiceRequest
                {
                    CallSid = callSid,
                    Digits = digits,
                    AccountSid = "MyAccount",
                    From = "+13054575008",
                    To = (toPhone == null) ? "+18001234567" : toPhone,
                    CallStatus = "in-progress",
                    ApiVersion = "v1",
                    Direction = "inbound",
                    CallerName = "Reinaldo Gutierrez"
                };
                return JsonConvert.SerializeObject(request);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        [Route("inbound")]
        public TwiMLResult Inbound([FromBody()] VoiceRequest request)
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
                    var gatherStep = context.CallFlowStepGather.Where(s => s.CallFlowStepId == stepId).FirstOrDefault();

                    if (gatherStep == null)
                    {
                        //TODO
                    }
                    else
                    {
                        //Check if it's a Survey gather step
                        bool isSurveyStep = false;
                        var question = context.CallSurveyQuestion.Where(q => q.CallFlowStepId == gatherStep.CallFlowStepId).FirstOrDefault();
                        if (question != null)
                        {
                            isSurveyStep = true;
                            //Save Survey Answer
                            context.CallSurveyAnswer.Add(new CallSurveyAnswer
                            {
                                CallSid= request.CallSid,
                                CallSurveyQuestionId = question.CallSurveyQuestionId,
                                SurveyAnswer = request.Digits
                            });
                            context.SaveChanges();
                        }

                        string gatherValueName = "";
                        CallFlowStepOption  option = context.CallFlowStepOption.Where(o => o.CallFlowStepId == stepId && o.OptionValue == request.Digits).FirstOrDefault();
                        if(option == null && isSurveyStep)
                        {
                            //for survey check for first option, we go to next step no mather the value
                            option = context.CallFlowStepOption.Where(o => o.CallFlowStepId == stepId).FirstOrDefault();
                        }

                        if (option == null)
                        {
                            //Retry Process
                            if (isSurveyStep)
                            {
                                //TODO
                            }
                            else
                            {
                                CallFlowStepGatherRetry gatherretry = context.CallFlowStepGatherRetry.Where(gr => gr.CallFlowStepId == gatherStep.CallFlowStepId).FirstOrDefault();
                                if (AllowRetry(request, gatherStep))
                                {
                                    string retrysay = "";
                                    if (gatherretry != null) retrysay = gatherretry.RetrySay;
                                    response = GetResponse(request.CallSid, gatherStep.CallFlowStep);
                                    if (!string.IsNullOrEmpty(retrysay))
                                    {
                                        response.Say(retrysay);
                                    }
                                }
                                else
                                {
                                    response = GetResponse(request.CallSid, gatherretry.FailCallFlowStep);
                                }
                            }
                        }
                        else
                        {
                            gatherValueName = option.OptionDescription;

                            //Saving customer choice
                            CallGather gather = new CallGather
                            {
                                CallSid = request.CallSid,
                                CallFlowStepId = gatherStep.CallFlowStepId,
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

                            response = GetResponse(request.CallSid, welcomeStep);
                            
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
        private bool AllowRetry(VoiceRequest request, CallFlowStepGather step)
        {
            try
            {
                short maxAttempts = step.Attempts;
                short actualAttempts = 0;
                using (var context = new CallTraxContext())
                {
                    CallGatherAttempt retry = context.CallGatherAttempt.Where(r => r.CallSid == request.CallSid && r.CallFlowStepId == step.CallFlowStepId).FirstOrDefault();
                    if (retry == null)
                    {
                        actualAttempts += 1;
                        context.CallGatherAttempt.Add(new CallGatherAttempt
                        {
                            CallSid = request.CallSid,
                            CallFlowStepId = step.CallFlowStepId,
                            Attempts = actualAttempts
                        });

                    }
                    else
                    {
                        actualAttempts = retry.Attempts;
                        actualAttempts += 1;
                        retry.Attempts = actualAttempts;
                    }
                    context.SaveChanges();

                    return (actualAttempts < maxAttempts);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
