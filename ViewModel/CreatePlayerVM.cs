using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TopSpin2.Models;

namespace TopSpin2
{
    public class CreatePlayerVM
    {
        public virtual Player Player { get; set; }

        public virtual HttpPostedFileBase PhotoFile { get; set; }
    }
}