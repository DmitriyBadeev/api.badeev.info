using System;
using System.Collections.Generic;
using Portfolio.Core.Interfaces;

namespace Portfolio.Core.Entities.Portfolio
{
    public class Work : IEntityBase
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Html { get; set; }

        public IEnumerable<TagWork> Tags { get; set; }

        public int CountViews { get; set; }

        public string Link { get; set; }

        public IEnumerable<Author> Authors { get; set; }

        public DateTime Date { get; set; }

        public string Task { get; set; }

        public string ImgPath { get; set; }
    }
}
