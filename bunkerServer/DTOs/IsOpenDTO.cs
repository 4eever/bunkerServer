namespace bunkerServer.DTOs
{
    public class IsOpenDTO
    {
        public string Uid_User { get; set; }
        public bool Card11 { get; set; }
        public bool Card22 { get; set; }
        public bool Cars33 { get; set; }
        public bool Cars44 { get; set; }
        public bool Cars55 { get; set; }
        public bool Cars66 { get; set; }

        public IsOpenDTO(string uid_user, bool card1, bool card2, bool card3, bool card4, bool card5, bool card6)
        {
            Uid_User = uid_user;
            Card11 = card1;
            Card22 = card2;
            Cars33 = card3;
            Cars44 = card4;
            Cars55 = card5;
            Cars66 = card6;
        }
    }
}
