using bunkerServer.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace bunkerServer
{
    public interface IUserRepository
    {
        public Task<User> AddUser(UserDTO userDTO);
        public Task<User> GetCurrentUser([FromQuery] string param);
        public Task<bool> HasUsersWithLobby(string uidLobby, User user);
        public Task<List<User>> GetUsersByLobby(string uidLobby);
        public Task<UserCardsDTO> GetUserCards(string uidUser);

    }
}
