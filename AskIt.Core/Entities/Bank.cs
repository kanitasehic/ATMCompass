﻿namespace ATMCompass.Core.Entities
{
    public class Bank
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Website { get; set; }

        public string? Email { get; set; }

        public virtual IEnumerable<ATM> ATMs { get; set; }
    }
}
