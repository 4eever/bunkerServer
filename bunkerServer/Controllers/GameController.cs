using bunkerServer.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bunkerServer.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;
        private readonly IUserRepository _userRepository;

        public GameController(IGameRepository gameRepository, IUserRepository userRepository)
        {
            _gameRepository = gameRepository;
            _userRepository = userRepository;
        }


        [HttpPost("start")]
        public async Task<ActionResult> StartGame([FromQuery] string lobbyName)//метод заупуска игры
        {
            List<User> users = await _userRepository.GetUsersByLobby(lobbyName);

            foreach (User user in users)
            {
                UserCardsDTO userCards = GenerateRandomCards();

                await _gameRepository.UpdateUserCardsInDatabase(user.Uid_user, userCards);
            }

            return Ok();
        }

        private UserCardsDTO GenerateRandomCards()
        {
            UserCardsDTO userCards = new UserCardsDTO();
            Random random = new Random();

            int card1Part1 = random.Next(18, 100);
            int card1Part2 = random.Next(2);
            int card1 = card1Part1 * 10 + card1Part2;
            userCards.Card1 = card1;

            userCards.Card2 = random.Next(1, 50);

            userCards.Card3 = random.Next(1, 183);

            userCards.Card4 = random.Next(1, 61);

            userCards.Card5 = random.Next(1, 118);

            userCards.Card6 = random.Next(1, 305);

            return userCards;
        }

        [HttpGet("revealcard")]
        public async Task<ActionResult> RevealCard(string uid_user, int choice)
        {
            User user = await _userRepository.GetCurrentUser(uid_user);

            IsOpenDTO isOpenDTO = new IsOpenDTO(uid_user, false, false, false, false, false, false);

            switch (choice)
            {
                case 1:
                    isOpenDTO.Card11 = true;
                    break;
                case 2:
                    isOpenDTO.Card22 = true;
                    break;
                case 3:
                    isOpenDTO.Cars33 = true;
                    break;
                case 4:
                    isOpenDTO.Cars44 = true;
                    break;
                case 5:
                    isOpenDTO.Cars55 = true;
                    break;
                case 6:
                    isOpenDTO.Cars66 = true;
                    break;
                default:
                    return BadRequest("Invalid choice");
            }

            // Сохраняем выбора в таблице is_ope
            await _gameRepository.UpdateIsOpen(isOpenDTO);

            return Ok();
        }

        [HttpPost("vote")] //Метод отправки голоса
        public async Task<ActionResult> Vote(UserVoteDTO userVote)
        {
            try
            {
                await _gameRepository.SaveVote(userVote);
                return Ok("Vote saved successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("getResult")] //Метод голосования
        public async Task<ActionResult<UserVoteDTO>> GetResult(string uid_lobby)
        {
            // Ждем 30 секунд
            await Task.Delay(TimeSpan.FromSeconds(30));

            // Получаем голоса по uid_lobby
            List<UserVoteDTO> votes = await _gameRepository.GetVotesByLobby(uid_lobby);

            // Подсчитываем голоса
            Dictionary<string, int> voteCounts = new Dictionary<string, int>();
            foreach (var vote in votes)
            {
                if (!string.IsNullOrEmpty(vote.Vote))
                {
                    if (voteCounts.ContainsKey(vote.Vote))
                    {
                        voteCounts[vote.Vote]++;
                    }
                    else
                    {
                        voteCounts[vote.Vote] = 1;
                    }
                }
            }

            // Находим пользователя с максимальным количеством голосов
            string mostVotedUser = voteCounts.OrderByDescending(kv => kv.Value).FirstOrDefault().Key;

            // Обновляем поля голосов в базе данных и удаляем пользователя с максимальным голосом
            await _gameRepository.UpdateVotesAndRemoveUser(uid_lobby, mostVotedUser);

            // Возвращаем пользователя с максимальным голосом
            return votes.FirstOrDefault(vote => vote.Uid_user == mostVotedUser);
        }



    }
}
