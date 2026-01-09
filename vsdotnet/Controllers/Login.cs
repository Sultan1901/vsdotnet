using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using MongoDB.Driver;

using vsdotnet.data;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMongoCollection<User>? _user;
    private readonly IConfiguration _configuration;
    public AuthController(MongoDbService mongoDbService, IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _user = mongoDbService.Database?.GetCollection<User>("user");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequestDto authRequest)
    {

        var filter = Builders<User>.Filter.Eq(x => x.Username, authRequest.Username);
        var user = _user.Find(filter).FirstOrDefault();

        if (user != null) 
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, authRequest.Username),
                 new Claim(ClaimTypes.Role, "User"),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new AuthResponseDto { Token = tokenString });
        }

        return Unauthorized();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<User?>> GetById(string id)
    {
        var filter = Builders<User>.Filter.Eq(x => x.Id, id);
        var user = _user.Find(filter).FirstOrDefault();
        return user is not null ? Ok(user) : NotFound();
    }
    [HttpPost("register")]
    public async Task<ActionResult> Post(User user)
    {
        await _user.InsertOneAsync(user);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }
}
