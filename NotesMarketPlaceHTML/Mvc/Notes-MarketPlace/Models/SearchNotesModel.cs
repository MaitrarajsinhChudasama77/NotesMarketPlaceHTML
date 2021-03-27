using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Notes_MarketPlace.Context;
using PagedList;
using PagedList.Mvc;

namespace Notes_MarketPlace.Models
{
    public class SearchNotesModel
    {
        public List<AddEditCategory> Category { get; set; }

        public List<AddEditCountry> Country { get; set; }

        public List<AddEditType> Type { get; set; }

        public List<string> University { get; set; }

        public List<string> Courses { get; set; }
        public double ? averagerating { get; set; }
        public int ? totalrating { get; set; }

        public IPagedList<NoteDetail> Note { get; set; }
    }
}