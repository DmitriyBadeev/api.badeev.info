using System;

namespace Portfolio.API.Mutations.InputTypes.Work
{
    public class CreateWorkInput
    {
        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Html { get; set; }

        public string Link { get; set; }

        public DateTime Date { get; set; }

        public string Task { get; set; }

        public string ImgPath { get; set; }
    }
}
