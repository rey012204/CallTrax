using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class SurveyQuestionType
    {
        public SurveyQuestionType()
        {
            CallSurveyQuestion = new HashSet<CallSurveyQuestion>();
        }

        public short SurveyQuestionTypeId { get; set; }
        public string SureveyQuestionTypeName { get; set; }

        public virtual ICollection<CallSurveyQuestion> CallSurveyQuestion { get; set; }
    }
}
