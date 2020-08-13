using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallSurveyAnswer
    {
        public long CallSurveyAnswerId { get; set; }
        public string CallSid { get; set; }
        public long CallSurveyQuestionId { get; set; }
        public string SurveyAnswer { get; set; }

        public virtual Call CallS { get; set; }
        public virtual CallSurveyQuestion CallSurveyQuestion { get; set; }
    }
}
