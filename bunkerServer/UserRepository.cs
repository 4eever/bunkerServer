using bunkerServer.DTOs;
using System.Data.SqlClient;

namespace bunkerServer
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> AddUser(UserDTO userDTO)
        {
            Lobby lobby = new Lobby(userDTO.Uid_lobby);
            User user = new User(userDTO.Uid_user, userDTO.Uid_lobby, userDTO.User_name, 0, 0, 0, 0, 0, 0, 0, "", 0);
            if (user != null)
            {
                DbFunctions.AddToLobbies(DbFunctions.connectionString, lobby.Uid_lobby);
                DbFunctions.AddToUsers(DbFunctions.connectionString, user.Uid_user, user.Uid_lobby, user.User_name, user.Avatar, user.Vote, user.Choice);
                DbFunctions.AddToCards(DbFunctions.connectionString, user.Uid_user, user.Card1, user.Card2, user.Card3, user.Card4, user.Card5, user.Card6);
                DbFunctions.AddToIsOpen(DbFunctions.connectionString, user.Uid_user, false, false, false, false, false, false);
            }
            return user;
        }

        public async Task<User> GetCurrentUser(string uid_user)
        {
            User user = DbFunctions.GetUserByUidUser(DbFunctions.connectionString, uid_user);
            return user;
        }

        public async Task<bool> HasUsersWithLobby(string uidLobby, User user)
        {

            using (SqlConnection connection = new SqlConnection(DbFunctions.connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT COUNT(*) FROM Users WHERE Uid_lobby = @uidLobby";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@uidLobby", uidLobby);
                    int userCount = (int)await command.ExecuteScalarAsync();

                    if (userCount > 0)
                    {
                        DbFunctions.AddToUsers(DbFunctions.connectionString, user.Uid_user, user.Uid_lobby, user.User_name, user.Avatar, user.Vote, user.Choice);
                        DbFunctions.AddToCards(DbFunctions.connectionString, user.Uid_user, user.Card1, user.Card2, user.Card3, user.Card4, user.Card5, user.Card6);
                        DbFunctions.AddToIsOpen(DbFunctions.connectionString, user.Uid_user, false, false, false, false, false, false);

                    }
                    return userCount > 0;
                }
            }
        }

        public async Task<List<User>> GetUsersByLobby(string uidLobby)
        {

            using (SqlConnection connection = new SqlConnection(DbFunctions.connectionString))
            {
                await connection.OpenAsync();

                string query = @"
            SELECT Users.*, Cards.*, IsOpen.*
            FROM Users
            JOIN Cards ON Users.uid_user = Cards.uid_user
            JOIN IsOpen ON Users.uid_user = IsOpen.uid_user
            WHERE Users.uid_lobby = @uidLobby";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@uidLobby", uidLobby);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        List<User> users = new List<User>();
                        while (reader.Read())
                        {
                            string Uid_user = reader.GetString(reader.GetOrdinal("Uid_user"));
                            string Uid_lobby = reader.GetString(reader.GetOrdinal("Uid_lobby"));
                            string User_name = reader.GetString(reader.GetOrdinal("User_name"));
                            int Avatar = reader.GetInt32(reader.GetOrdinal("Avatar"));
                            string Vote = reader.GetString(reader.GetOrdinal("Vote"));
                            int Choice = reader.GetInt32(reader.GetOrdinal("Choice"));
                            int Card1 = (reader.GetBoolean(reader.GetOrdinal("card11")) == false) ? 0 : reader.GetInt32(reader.GetOrdinal("card1"));
                            int Card2 = (reader.GetBoolean(reader.GetOrdinal("card22")) == false) ? 0 : reader.GetInt32(reader.GetOrdinal("card2"));
                            int Card3 = (reader.GetBoolean(reader.GetOrdinal("card33")) == false) ? 0 : reader.GetInt32(reader.GetOrdinal("card3"));
                            int Card4 = (reader.GetBoolean(reader.GetOrdinal("card44")) == false) ? 0 : reader.GetInt32(reader.GetOrdinal("card4"));
                            int Card5 = (reader.GetBoolean(reader.GetOrdinal("card55")) == false) ? 0 : reader.GetInt32(reader.GetOrdinal("card5"));
                            int Card6 = (reader.GetBoolean(reader.GetOrdinal("card66")) == false) ? 0 : reader.GetInt32(reader.GetOrdinal("card6"));
                            User user = new User(Uid_user, Uid_lobby, User_name, Avatar, Card1, Card2, Card3, Card4, Card5, Card6, Vote, Choice);
                            users.Add(user);
                        }
                        return users;
                    }
                }
            }
        }

        public async Task<UserCardsDTO> GetUserCards(string uidUser)
        {

            using (SqlConnection connection = new SqlConnection(DbFunctions.connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT card1, card2, card3, card4, card5, card6 FROM Cards WHERE uid_user = @uidUser";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@uidUser", uidUser);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.Read())
                        {
                            UserCardsDTO userCards = new UserCardsDTO
                            {
                                Card1 = reader.GetInt32(reader.GetOrdinal("card1")),
                                Card2 = reader.GetInt32(reader.GetOrdinal("card2")),
                                Card3 = reader.GetInt32(reader.GetOrdinal("card3")),
                                Card4 = reader.GetInt32(reader.GetOrdinal("card4")),
                                Card5 = reader.GetInt32(reader.GetOrdinal("card5")),
                                Card6 = reader.GetInt32(reader.GetOrdinal("card6"))
                            };
                            return userCards;
                        }
                        else
                        {
                            return null; // Пользователь не найден
                        }
                    }
                }
            }
        }
    }
}
