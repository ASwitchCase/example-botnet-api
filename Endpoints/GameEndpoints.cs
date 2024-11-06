using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints;
public static class GamesEndpoints {
    private static readonly List<GameDto> games =[
        new (
            1,
            "Street Fighter 2",
            "Fighting",
            19.99M,
            new DateOnly(1992,7,15)
        ),
        new (
            2,
            "The Legend of Zelda",
            "RPG",
            49.99M,
            new DateOnly(2016,7,15)
        ),
        new (
            3,
            "Super Smash Bro Ultimate",
            "Fighting",
            9.99M,
            new DateOnly(2017,7,15)
        ),
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app){
        var group = app.MapGroup("games")
            .WithParameterValidation();

        group.MapGet("/",()=> games);
        group.MapGet("/{id}",(int id)=> {
            // Nullable type
            GameDto? game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        }).WithName("GetGame");

        group.MapPost("/",(CreateGameDto newGame) => {
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );

            games.Add(game);
            return Results.CreatedAtRoute("GetGame",new {id = game.Id}, game);
        });

        group.MapPut("/{id}",(int id, UpdateGameDto updatedGameDto) =>{
            var index = games.FindIndex(game => game.Id == id);

            if(index == -1){
                return Results.NotFound();
            }
            
            games[index] = new GameDto(
                id,
                updatedGameDto.Name,
                updatedGameDto.Genre,
                updatedGameDto.Price,
                updatedGameDto.ReleaseDate
            );

            return Results.NoContent();
        });

        group.MapDelete("/{id}",(int id) => {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });
        return group;
    }
}