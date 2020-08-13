using System;
using System.Collections.Generic;

namespace CallTrax.Models
{
    public partial class CallSurvey
    {
        public CallSurvey()
        {
            CallSurveyQuestion = new HashSet<CallSurveyQuestion>();
        }

        public long CallSurveyId { get; set; }
        public long ClientId { get; set; }
        public string CallSurveyName { get; set; }

        public virtual Client Client { get; set; }
        public virtual ICollection<CallSurveyQuestion> CallSurveyQuestion { get; set; }
    }
}
