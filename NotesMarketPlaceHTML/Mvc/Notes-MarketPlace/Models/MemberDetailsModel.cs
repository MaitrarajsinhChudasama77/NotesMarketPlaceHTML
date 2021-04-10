using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Notes_MarketPlace.Context;
using PagedList;
using PagedList.Mvc;

namespace Notes_MarketPlace.Models
{
    public class MemberDetailsModel
    {
        public IPagedList<NoteDetail> Note { get; set; }
        public UserProfile Profile { get; set; }
        public Context.User User { get; set; }
    }
}