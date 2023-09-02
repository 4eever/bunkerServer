using bunkerServer.DTOs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bunkerServer.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserCreateController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserCreateController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpPost]
        public async Task<ActionResult> PostUser([FromBody] UserDTO userDTO)//Метод создания user и lobby
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await _userRepository.AddUser(userDTO);

            return CreatedAtAction(nameof(GetUser), new { id = user.Uid_user }, user);
        }

        [HttpGet("uid_user")]
        public async Task<ActionResult> GetUser([FromQuery] string uid_user)
        {
            User currentUser = await _userRepository.GetCurrentUser(uid_user);
            return Ok(currentUser);
        }

        [HttpPost("with-lobby")]
        public async Task<ActionResult> PostUserWithLobby([FromBody] UserDTO userDTO)//Метод создания user с существующем lobby
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Проверяем наличие других пользователей с таким же uid_lobby
            User user = new User(userDTO.Uid_user, userDTO.Uid_lobby, userDTO.User_name, 0, 0, 0, 0, 0, 0, 0, "", 0);
            bool hasUsersWithSameLobby = await _userRepository.HasUsersWithLobby(userDTO.Uid_lobby, user);

            if (hasUsersWithSameLobby)
            {

                return CreatedAtAction(nameof(GetUser), new { id = user.Uid_user }, user);
            }
            else
            {
                return BadRequest("No users with the specified uid_lobby exist.");
            }
        }

        [HttpGet("get-by-lobby")]//Метод получения всех user с заданным uid_lobby
        public async Task<ActionResult<List<User>>> GetUsersByLobby([FromQuery] string uid_lobby)
        {
            var users = await _userRepository.GetUsersByLobby(uid_lobby);
            return Ok(users);
        }

        [HttpGet("get-cards")]//Метод получения информации о своих картах user
        public async Task<ActionResult<UserCardsDTO>> GetUserCards([FromQuery] string uid_user)
        {
            var userCards = await _userRepository.GetUserCards(uid_user);
            return Ok(userCards);
        }
    }
}
