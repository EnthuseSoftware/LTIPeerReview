using LtiLibrary.Core.Lti1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LTIPeerReview.Models
{
    public class ToolModel
    {
        public string ConsumerSecret { get; set; }
        public LtiRequest LtiRequest { get; set; }

    }
    
}