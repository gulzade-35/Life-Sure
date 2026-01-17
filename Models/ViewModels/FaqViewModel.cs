using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LifeSure.Models.ViewModels
{
    public class FaqViewModel
    {
        public string Prompt { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}