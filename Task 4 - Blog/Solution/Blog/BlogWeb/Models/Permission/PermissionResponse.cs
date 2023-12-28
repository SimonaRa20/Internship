﻿namespace BlogWeb.Models.Permission
{
    public class PermissionResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int PermissionId { get; set; }
        public bool WriteArticals { get; set; }
        public bool RateArticals { get; set; }
        public bool WriteComments { get; set; }
    }
}
