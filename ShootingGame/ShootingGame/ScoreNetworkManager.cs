using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

namespace ShootingGame
{
     public record ScoreRequest(int Rank, string Name, int Score);

    internal class ScoreNetworkManager
    {
    }
}
