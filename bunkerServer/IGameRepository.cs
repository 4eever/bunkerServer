using bunkerServer.DTOs;
using System.Data.SqlClient;

namespace bunkerServer
{
    public interface IGameRepository
    {
        public Task UpdateUserCardsInDatabase(string uidUser, UserCardsDTO userCards);
        public Task UpdateIsOpen(IsOpenDTO isOpenDTO);
        public Task SaveVote(UserVoteDTO userVote);
        public Task UpdateVotesAndRemoveUser(string uidLobby, string mostVotedUser);
        public Task<List<UserVoteDTO>> GetVotesByLobby(string uidLobby);
    }
}
