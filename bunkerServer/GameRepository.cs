using bunkerServer.DTOs;
using System.Data.SqlClient;

namespace bunkerServer
{
    public class GameRepository : IGameRepository
    {
        public async Task UpdateUserCardsInDatabase(string uidUser, UserCardsDTO userCards)
        {

            using (SqlConnection connection = new SqlConnection(DbFunctions.connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                UPDATE Cards
                SET card1 = @card1,
                    card2 = @card2,
                    card3 = @card3,
                    card4 = @card4,
                    card5 = @card5,
                    card6 = @card6
                WHERE uid_user = @uidUser";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@uidUser", uidUser);
                    command.Parameters.AddWithValue("@card1", userCards.Card1);
                    command.Parameters.AddWithValue("@card2", userCards.Card2);
                    command.Parameters.AddWithValue("@card3", userCards.Card3);
                    command.Parameters.AddWithValue("@card4", userCards.Card4);
                    command.Parameters.AddWithValue("@card5", userCards.Card5);
                    command.Parameters.AddWithValue("@card6", userCards.Card6);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateIsOpen(IsOpenDTO isOpenDTO)
            {

            using (SqlConnection connection = new SqlConnection(DbFunctions.connectionString))
            {
                await connection.OpenAsync();

                string query = @"
            UPDATE IsOpen
            SET card11 = @Card11,
                card22 = @Card22,
                card33 = @Card33,
                card44 = @Card44,
                card55 = @Card55,
                card66 = @Card66
            WHERE uid_user = @Uid_User";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Uid_User", isOpenDTO.Uid_User);
                    command.Parameters.AddWithValue("@Card11", isOpenDTO.Card11);
                    command.Parameters.AddWithValue("@Card22", isOpenDTO.Card22);
                    command.Parameters.AddWithValue("@Card33", isOpenDTO.Cars33);
                    command.Parameters.AddWithValue("@Card44", isOpenDTO.Cars44);
                    command.Parameters.AddWithValue("@Card55", isOpenDTO.Cars55);
                    command.Parameters.AddWithValue("@Card66", isOpenDTO.Cars66);


                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Data updated in is_open table successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update data in IsOpen table.");
                    }
                }
            }
        }

        public async Task SaveVote(UserVoteDTO userVote)
        {

            using (SqlConnection connection = new SqlConnection(DbFunctions.connectionString))
            {
                await connection.OpenAsync();

                string query = @"
            UPDATE users
            SET vote = @Vote
            WHERE uid_user = @Uid_User";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Uid_User", userVote.Uid_user);
                    command.Parameters.AddWithValue("@Vote", userVote.Vote);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Vote updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update vote.");
                    }
                }
            }
        }

        public async Task UpdateVotesAndRemoveUser(string uidLobby, string mostVotedUser)
        {

            using (SqlConnection connection = new SqlConnection(DbFunctions.connectionString))
            {
                await connection.OpenAsync();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    // Удаляем записи из таблицы is_ope
                    string deleteIsOpenQuery = @"
                DELETE FROM IsOpen
                WHERE uid_user = @mostVotedUser";

                    using (SqlCommand command = new SqlCommand(deleteIsOpenQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@mostVotedUser", mostVotedUser);
                        await command.ExecuteNonQueryAsync();
                    }

                    // Удаляем записи из таблицы cards
                    string deleteCardsQuery = @"
                DELETE FROM Cards
                WHERE uid_user = @mostVotedUser";

                    using (SqlCommand command = new SqlCommand(deleteCardsQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@mostVotedUser", mostVotedUser);
                        await command.ExecuteNonQueryAsync();
                    }

                    // Обновляем голоса пользователей
                    string updateVotesQuery = @"
                UPDATE users
                SET vote = ''
                WHERE uid_lobby = @uidLobby";

                    using (SqlCommand command = new SqlCommand(updateVotesQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@uidLobby", uidLobby);
                        await command.ExecuteNonQueryAsync();
                    }

                    // Удаляем пользователя с максимальным голосом
                    string deleteUserQuery = @"
                DELETE FROM users
                WHERE uid_user = @mostVotedUser";

                    using (SqlCommand command = new SqlCommand(deleteUserQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@mostVotedUser", mostVotedUser);
                        await command.ExecuteNonQueryAsync();
                    }

                    transaction.Commit();
                }
            }
        }


        public async Task<List<UserVoteDTO>> GetVotesByLobby(string uidLobby)
        {

            using (SqlConnection connection = new SqlConnection(DbFunctions.connectionString))
            {
                await connection.OpenAsync();

                string query = @"
            SELECT uid_user, vote
            FROM users
            WHERE uid_lobby = @uidLobby";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@uidLobby", uidLobby);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        List<UserVoteDTO> votes = new List<UserVoteDTO>();
                        while (reader.Read())
                        {
                            UserVoteDTO vote = new UserVoteDTO
                            {
                                Uid_user = reader.GetString(reader.GetOrdinal("uid_user")),
                                Vote = reader.GetString(reader.GetOrdinal("vote"))
                            };
                            votes.Add(vote);
                        }
                        return votes;
                    }
                }
            }
        }


    }
}
