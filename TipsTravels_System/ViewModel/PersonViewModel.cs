using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TipsTravels_System.Models;

namespace TipsTravels_System.ViewModel
{
    public class PersonViewModel : Person
    {
        public HttpPostedFileBase file { get; set; }
        public string PasswordAsString { get; set; }
    }
}