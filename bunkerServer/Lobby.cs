namespace bunkerServer
{
    public class Lobby
    {
        public string Uid_lobby { get; set; } = "";

        public Lobby(string uid_lobby)
        {
            Uid_lobby = uid_lobby;
        }
    }
}
