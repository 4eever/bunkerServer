using System.ComponentModel.DataAnnotations;

namespace bunkerServer
{
    public class User
    {
        [Key] //Uid_user является первичным ключом
        public string Uid_user { get; set; } = "";

        public string Uid_lobby { get; set; } = "";
        public string User_name { get; set; } = "";
        public int Avatar { get; set; }

        public int Card1 { get; set; }
        public int Card2 { get; set; }
        public int Card3 { get; set; }
        public int Card4 { get; set; }
        public int Card5 { get; set; }
        public int Card6 { get; set; }

        public string Vote { get; set; } = "";
        public int Choice { get; set; }

        public User(string uid_user, string uid_lobby, string user_name, int avatar, int card1, int card2, int card3, int card4, int card5, int card6, string vote, int choice)
        {
            Uid_user = uid_user;
            Uid_lobby = uid_lobby;
            User_name = user_name;
            Avatar = avatar;
            Card1 = card1;
            Card2 = card2;
            Card3 = card3;
            Card4 = card4;
            Card5 = card5;
            Card6 = card6;
            Vote = vote;
            Choice = choice;
        }

        //создай пустой конструктор
        public User()
        {
        }
    }
}
