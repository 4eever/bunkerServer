using System.ComponentModel.DataAnnotations;

namespace bunkerServer.DTOs
{
    public class UserDTO
    {
        [Key] //Uid_user является первичным ключом
        public string Uid_user { get; set; } = "";

        public string Uid_lobby { get; set; } = "";
        public string User_name { get; set; } = "";

    }
}
