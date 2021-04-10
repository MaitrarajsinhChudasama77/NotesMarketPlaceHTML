using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Notes_MarketPlace.Context;
using PagedList;
using PagedList.Mvc;

namespace Notes_MarketPlace.Models
{
    public class DashboardModel
    {
        public IPagedList<NoteDetail> Note { get; set; }
        public IPagedList<NoteDetail> Note2 { get; set; }
    }
}