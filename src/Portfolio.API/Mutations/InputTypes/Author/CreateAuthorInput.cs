﻿namespace Portfolio.API.Mutations.InputTypes.Author
{
    public class CreateAuthorInput
    {
        public string Name { get; set; }

        public string Role { get; set; }

        public string Link { get; set; }

        public int WorkId { get; set; }
    }
}
