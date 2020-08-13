using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallSurveyQuestion
    {
        public CallSurveyQuestion()
        {
            CallSurveyAnswer = new HashSet<CallSurveyAnswer>();
        }

        public long CallSurveyQuestionId { get; set; }
        public long CallSurveyId { get; set; }
        public long CallFlowStepId { get; set; }
        public short QuestionType { get; set; }

        public virtual CallFlowStepGather CallFlowStep { get; set; }
        public virtual CallSurvey CallSurvey { get; set; }
        public virtual SurveyQuestionType QuestionTypeNavigation { get; set; }
        public virtual ICollection<CallSurveyAnswer> CallSurveyAnswer { get; set; }
    }
}
