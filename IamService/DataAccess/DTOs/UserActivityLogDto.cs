using System;
namespace IamService.DataAccess.DTOs
{
    public class UserActivityLogDto
    {
        public int Id { get; set; }
        public string ActionType { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
