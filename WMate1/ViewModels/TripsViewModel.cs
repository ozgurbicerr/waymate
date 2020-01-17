using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMate1.Models;

namespace WMate1.ViewModels
{
    public class TripsViewModel
    {
        public List<Entry> EntryList { get; set; }
        public List<UserCredential> UserList { get; set; }
        public List<Comment> Commentses { get; set; }
    }
}