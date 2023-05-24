﻿namespace ATMCompass.Core.Models.Users.Responses
{
    public class UserManagerResponse
    {
        public string Message { get; set; }

        public bool IsSuccess { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public string? Token { get; set; }

        public DateTime? ExpireDate { get; set; }
    }
}
