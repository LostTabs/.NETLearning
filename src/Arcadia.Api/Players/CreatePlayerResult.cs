namespace Arcadia.Api.Players;

public enum CreatePlayerStatus
{
    Created, 
    UsernameTaken, 
    EmailTaken
}

public record CreatePlayerResult(CreatePlayerStatus Status, Player? Player);